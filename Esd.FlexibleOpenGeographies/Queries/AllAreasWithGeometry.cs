using System.Collections.Generic;
using System.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AllAreasWithGeometry : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;

        public AllAreasWithGeometry(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            var results = new List<AreaBasicWithType>();
            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT area_code, area_type_code FROM area";
                    command.CommandType = CommandType.Text;
                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                            results.Add(new AreaBasicWithType
                            {
                                Code = (string)reader["area_code"],
                                TypeCode = (string)reader["area_type_code"]
                            });
                }
                connection.Close();
            }
            return results;
        }
    }
}
