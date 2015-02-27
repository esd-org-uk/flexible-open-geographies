using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class TypeHierarchiesWithLabelsForType : IQueryEnumerable<TypeHierarchyWithLabels>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;

        public TypeHierarchiesWithLabelsForType(IContextFactory contextFactory, string typeCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
        }

        public IEnumerable<TypeHierarchyWithLabels> Fetch()
        {
            using (var context = _contextFactory.Create())
                return ParentTypes(context).Concat(ChildTypes(context));
        }

        private IEnumerable<TypeHierarchyWithLabels> ChildTypes(IFogContext context)
        {
            return context.TypeHierarchies.AsNoTracking()
                          .Where(x => x.TypeCode == _typeCode)
                          .Select(x => new TypeHierarchyWithLabels
                          {
                              TypeCode = x.TypeCode,
                              ChildTypeCode = x.ChildTypeCode,
                              TypeLabel = x.AreaType.Label,
                              ChildTypeLabel = x.ChildAreaType.Label,
                              IsPrimary = x.IsPrimary,
                              CoversWhole = x.CoversWhole
                          })
                          .ToList();
        }

        private IEnumerable<TypeHierarchyWithLabels> ParentTypes(IFogContext context)
        {
            return context.TypeHierarchies.AsNoTracking()
                          .Where(x => x.ChildTypeCode == _typeCode)
                          .Select(x => new TypeHierarchyWithLabels
                          {
                              TypeCode = x.TypeCode,
                              ChildTypeCode = x.ChildTypeCode,
                              TypeLabel = x.AreaType.Label,
                              ChildTypeLabel = x.ChildAreaType.Label,
                              IsPrimary = x.IsPrimary,
                              CoversWhole = x.CoversWhole
                          })
                          .ToList();
        }
    }
}
