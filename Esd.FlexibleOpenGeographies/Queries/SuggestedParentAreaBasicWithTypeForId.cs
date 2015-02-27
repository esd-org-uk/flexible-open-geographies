using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class SuggestedParentAreaBasicWithTypeForId : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _areaId;

        public SuggestedParentAreaBasicWithTypeForId(IContextFactory contextFactory, int areaId)
        {
            _contextFactory = contextFactory;
            _areaId = areaId;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            var results = new List<AreaBasicWithType>();
            var area = GetArea();
            if (area == null) return results;
            using (var connection = _contextFactory.CreatePostGisOpenConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "suggestAreas";
                    command.CommandType = CommandType.StoredProcedure;
                    var typesParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "areatypes",
                        Value = area.ParentTypes
                    };
                    var areaCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "childarea",
                        Value = area.AreaCode
                    };
                    var typeCodeParameter = new NpgsqlParameter
                    {
                        DbType = DbType.String,
                        ParameterName = "childtype",
                        Value = area.TypeCode
                    };
                    command.Parameters.Add(typesParameter);
                    command.Parameters.Add(areaCodeParameter);
                    command.Parameters.Add(typeCodeParameter);
                    using (var context = _contextFactory.Create())
                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            var typeCode = reader.GetString(reader.GetOrdinal("typecode"));
                            var areaCode = reader.GetString(reader.GetOrdinal("areacode"));
                            var details = GetDetails(typeCode, areaCode, context);
                            if (details != null) results.Add(details);
                        }
                }
                connection.Close();
            }
            return results;
        }

        private static AreaBasicWithType GetDetails(string typeCode, string areaCode, IFogContext context)
        {
            return context.AreaDetails.AsNoTracking()
                          .Select(x => new AreaBasicWithType
                          {
                              Id = x.Id,
                              Code = x.Code,
                              Label = x.Label,
                              TypeCode = x.TypeCode,
                              TypeName = x.AreaType.Label
                          })
                          .SingleOrDefault(x => x.TypeCode == typeCode && x.Code == areaCode);
        }

        private AreaForSuggestions GetArea()
        {
            using (var context = _contextFactory.Create())
            {
                var area = context.AreaDetails.AsNoTracking().SingleOrDefault(x => x.Id == _areaId);
                if (area == null) return null;
                var types = context.TypeHierarchies.AsNoTracking()
                                   .Where(x => x.ChildTypeCode == area.TypeCode)
                                   .Select(x => x.TypeCode)
                                   .ToList();
                if (!types.Any()) return null;
                return new AreaForSuggestions
                {
                    AreaCode = area.Code,
                    TypeCode = area.TypeCode,
                    ParentTypes = string.Join(",", types)
                };
            }
        }

        private class AreaForSuggestions
        {
            public string TypeCode { get; set; }
            public string AreaCode { get; set; }
            public string ParentTypes { get; set; }
        }
    }
}
