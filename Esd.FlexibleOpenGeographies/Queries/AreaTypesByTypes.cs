using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaTypesByTypes : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private List<TypeHierarchyBasic> _typeHierarchy;

        public AreaTypesByTypes(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForHierarchies(List<TypeHierarchyBasic> typeHierarchy)
        {
            _typeHierarchy = typeHierarchy;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            var codes = new List<string>();

            foreach (TypeHierarchyBasic tH in _typeHierarchy)
            {
                codes.Add(tH.TypeCode);
            }

            using (var context = _contextFactory.Create())
                return context.AreaTypes.AsNoTracking()
                              .Where(type => codes.Contains(type.Code))
                              .OrderBy(type => type.Label)
                              .Select(AreaTypeMapper.MapBasic)
                              .ToList();
        }
    }
}
