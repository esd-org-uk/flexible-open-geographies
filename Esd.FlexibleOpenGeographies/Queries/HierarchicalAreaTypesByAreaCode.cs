using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class HierarchicalAreaTypesByAreaCode : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private string _code;

        public HierarchicalAreaTypesByAreaCode(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForCode(string code)
        {
            _code = code;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            var id = string.Empty;
            using (var context = _contextFactory.Create())
            {
                var area = context.AreaDetails.AsNoTracking().SingleOrDefault(a => a.Code == _code);
                if (area != null)
                {
                    id = area.TypeCode;
                }
            }
            
            var hierarchies = GetAllHierarchies(id);

            var queryFactory = new QueryFactory(_contextFactory);
            return queryFactory.CreateAreaTypesByTypesQuery(hierarchies).Fetch();
        }

        private List<TypeHierarchyBasic> GetAllHierarchies(string id)
        {
            var results = new List<TypeHierarchyBasic>();

            using (var context = _contextFactory.Create())
            {
                var parents = context.TypeHierarchies.AsNoTracking().Where(t => t.TypeCode == id).Select(TypeHierarchyMapper.MapBasic);
                results.AddRange(parents);
            }

            if (results.Count > 0)
            {
                var r = results.ToArray();
                foreach (var result in r)
                {
                    results.AddRange(GetAllHierarchies(result.ChildTypeCode));
                }
            }

            return results;
        }
    }
}
