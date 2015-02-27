using Esd.FlexibleOpenGeographies.Dtos;
using Npgsql;
using System.Data;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class BoundingBoxForArea : IQuerySingle<BoundingBox>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly string _areaCode;

        public BoundingBoxForArea(IContextFactory contextFactory, string typeCode, string areaCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _areaCode = areaCode;
        }

        public BoundingBox Find()
        {
            BoundingBox bounds = null;
            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT ST_XMIN(extent) AS minx, ST_YMIN(extent) AS miny, ST_XMAX(extent) AS maxx, ST_YMAX(extent) AS maxy FROM (SELECT ST_EXTENT(shape) as extent from area where area_type_code = :typecode and area_code = :areacode) a";
                    command.CommandType = CommandType.Text;
                    var typeCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "typecode",
                        Value = _typeCode
                    };
                    var areaCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "areacode",
                        Value = _areaCode
                    };
                    command.Parameters.Add(typeCodeParameter);
                    command.Parameters.Add(areaCodeParameter);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read() && reader["minx"] is double)
                            bounds = new BoundingBox
                            {
                                MinimumX = (double)reader["minx"],
                                MaximumX = (double)reader["maxx"],
                                MinimumY = (double)reader["miny"],
                                MaximumY = (double)reader["maxy"]
                            };
                    }
                }
                connection.Close();
            }
            return bounds;
        }
    }
}
