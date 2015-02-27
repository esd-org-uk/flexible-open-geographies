using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ParentAreaTypesForType : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;

        public ParentAreaTypesForType(IContextFactory contextFactory, string typeCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.TypeHierarchies.AsNoTracking()
                              .Where(x => x.ChildTypeCode == _typeCode)
                              .Select(x => new AreaTypeBasic
                              {
                                  Code = x.TypeCode,
                                  Label = x.AreaType.Label
                              })
                              .ToList();
        }
    }
}
