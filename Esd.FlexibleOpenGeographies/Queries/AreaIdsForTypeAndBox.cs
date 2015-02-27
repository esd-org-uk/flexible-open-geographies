using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaIdsForTypeAndBox : IQueryEnumerable<int>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly double _minX;
        private readonly double _minY;
        private readonly double _maxX;
        private readonly double _maxY;

        public AreaIdsForTypeAndBox(IContextFactory contextFactory, string typeCode, double minX, double minY, double maxX, double maxY)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _minX = minX;
            _minY = minY;
            _maxX = maxX;
            _maxY = maxY;
        }

        public IEnumerable<int> Fetch()
        {
            var codes = GetAreaCodes();
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking()
                              .Where(x => x.TypeCode == _typeCode && codes.Contains(x.Code))
                              .Select(x => x.Id)
                              .ToList();
        }

        private IList<string> GetAreaCodes()
        {
            var areas = new List<string>();
            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT area_code FROM area WHERE ST_INTERSECTS(ST_MakeEnvelope(:minx, :miny, :maxx, :maxy)::geography, shape) AND area_type_code = :typecode";
                    command.CommandType = CommandType.Text;
                    var typeCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "typecode",
                        Value = _typeCode
                    };
                    var minxParameter = new NpgsqlParameter
                    {
                        DbType = DbType.Double,
                        ParameterName = "minx",
                        Value = _minX
                    };
                    var minyParameter = new NpgsqlParameter
                    {
                        DbType = DbType.Double,
                        ParameterName = "miny",
                        Value = _minY
                    };
                    var maxxParameter = new NpgsqlParameter
                    {
                        DbType = DbType.Double,
                        ParameterName = "maxx",
                        Value = _maxX
                    };
                    var maxyParameter = new NpgsqlParameter
                    {
                        DbType = DbType.Double,
                        ParameterName = "maxy",
                        Value = _maxY
                    };
                    command.Parameters.Add(typeCodeParameter);
                    command.Parameters.Add(maxxParameter);
                    command.Parameters.Add(maxyParameter);
                    command.Parameters.Add(minxParameter);
                    command.Parameters.Add(minyParameter);
                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                            areas.Add((string)reader["area_code"]);
                }
                connection.Close();
            }
            return areas;
        }
    }
}
