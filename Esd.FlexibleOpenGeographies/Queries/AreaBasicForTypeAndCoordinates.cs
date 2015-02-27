using Esd.FlexibleOpenGeographies.Dtos;
using Npgsql;
using System.Data;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaBasicForTypeAndCoordinates : IQuerySingle<AreaBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly double _x;
        private readonly double _y;

        public AreaBasicForTypeAndCoordinates(IContextFactory contextFactory, string typeCode, double x, double y)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _x = x;
            _y = y;
        }

        public AreaBasic Find()
        {
            return new TypeCodesForAreaTypeGroup(_contextFactory, _typeCode)
                .Fetch()
                .Select(AreaForType)
                .FirstOrDefault(x => x != null);
        }

        private AreaBasic AreaForType(string typeCode)
        {
            AreaBasic area = null;
            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT area_code FROM area WHERE ST_INTERSECTS(ST_SetSRID(ST_Point(:x, :y), 4326), shape) AND area_type_code = :typecode";
                    command.CommandType = CommandType.Text;
                    var typeCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "typecode",
                        Value = typeCode
                    };
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
                    command.Parameters.Add(typeCodeParameter);
                    command.Parameters.Add(xParameter);
                    command.Parameters.Add(yParameter);
                    using (var reader = command.ExecuteReader())
                        if (reader.Read())
                            area = new AreaBasic
                            {
                                Code = (string)reader["area_code"]
                            };
                }
                connection.Close();
            }
            return SetFields(area, typeCode);
        }

        private AreaBasic SetFields(AreaBasic area, string typeCode)
        {
            if (area == null || area.Code == null) return area;
            using (var context = _contextFactory.Create())
            {
                var areaDetail = context.AreaDetails.AsNoTracking().SingleOrDefault(x => x.Code == area.Code && x.TypeCode == typeCode);
                if (areaDetail != null)
                {
                    area.Id = areaDetail.Id;
                    area.Label = areaDetail.Label;
                }
            }
            return area;
        }
    }
}
