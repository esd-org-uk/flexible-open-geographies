using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ChildTypesForAreaType : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;

        public ChildTypesForAreaType(IContextFactory contextFactory, string typeCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.TypeHierarchies.AsNoTracking()
                              .Where(x => x.TypeCode == _typeCode)
                              .Select(x => new AreaTypeBasic{Code = x.ChildTypeCode, Label = x.ChildAreaType.Label})
                              .ToList();
        }
    }
}
