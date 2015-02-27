using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class TypeHierarchiesForAreaType : IQueryEnumerable<TypeHierarchyBasic>
    {
        private readonly IContextFactory _contextFactory;
        private string _typeCode;

        public TypeHierarchiesForAreaType(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForTypeCode(string typeCode)
        {
            _typeCode = typeCode;
        }

        public IEnumerable<TypeHierarchyBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.TypeHierarchies.AsNoTracking()
                              .Where(hierarchy => hierarchy.TypeCode == _typeCode)
                              .Select(hierarchy => new TypeHierarchyBasic
                                       {
                                           TypeCode = hierarchy.TypeCode,
                                           ChildTypeCode = hierarchy.ChildTypeCode,
                                           IsPrimary = hierarchy.IsPrimary,
                                           CoversWhole = hierarchy.CoversWhole
                                       })
                              .ToList();
        }
    }
}
