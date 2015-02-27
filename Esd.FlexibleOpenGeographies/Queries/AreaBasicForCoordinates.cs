using Esd.FlexibleOpenGeographies.Dtos;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaBasicForCoordinates : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly double _x;
        private readonly double _y;

        public AreaBasicForCoordinates(IContextFactory contextFactory, double x, double y)
        {
            _contextFactory = contextFactory;
            _x = x;
            _y = y;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            List<AreaBasicWithType> areas = new List<AreaBasicWithType>();

            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT area_code, area_type_code FROM area WHERE ST_INTERSECTS(ST_SetSRID(ST_Point(:x, :y), 4326), shape) ORDER BY ST_Area(shape) ASC";
                    command.CommandType = CommandType.Text;
                    var xParameter = new NpgsqlParameter
                    {
                        DbType = DbType.Double,
                        ParameterName = "x",
                        Value = _x
                    };
                    var yParameter = new NpgsqlParameter
                    {
                        DbType = DbType.Double,
                        ParameterName = "y",
                        Value = _y
                    };
                    command.Parameters.Add(xParameter);
                    command.Parameters.Add(yParameter);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string code = (string)reader["area_code"];
                            string type = (string)reader["area_type_code"];
                            var areaBasicWithTypeForTypeAndCode = new AreaBasicWithTypeForTypeAndCode(_contextFactory, type, code);
                            areas.Add(areaBasicWithTypeForTypeAndCode.Find());
                        }
                    }
                }
            }
            return areas;
        }
    }
}
