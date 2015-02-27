using Npgsql;
using System.Data;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class GeoJsonForArea : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly string _code;
        private readonly string _name;

        public GeoJsonForArea(IContextFactory contextFactory, string typeCode, string code, string name)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _code = code;
            _name = name;
        }

        public string Find()
        {
            string result;
            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT ST_AsGeoJSON(shape) FROM area WHERE area_code = :areacode AND area_type_code = :typecode";
                    command.CommandType = CommandType.Text;
                    var areaCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "areacode",
                        Value = _code
                    };
                    var areaTypeCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "typecode",
                        Value = _typeCode
                    };
                    command.Parameters.Add(areaCodeParameter);
                    command.Parameters.Add(areaTypeCodeParameter);
                    var geoJson = command.ExecuteScalar();
                    if (!(geoJson is string) || string.IsNullOrWhiteSpace((string)geoJson))
                        result = null;
                    else
                        result = (string)geoJson;
                }
                connection.Close();
            }
            return GenerateJson(result);
        }

        private string GenerateJson(string fragment)
        {
            return fragment == null
                ? null
                : string.Format(
                    "{{\"type\": \"Feature\", \"geometry\": {0}, \"properties\": {{ \"name\": \"{1}\"}} }}",
                    fragment, _name);
        }
    }
}
