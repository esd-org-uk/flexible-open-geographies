using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaTypesBasic : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly bool _includeGroups;

        public AreaTypesBasic(IContextFactory contextFactory, bool includeGroups)
        {
            _contextFactory = contextFactory;
            _includeGroups = includeGroups;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaTypes.AsNoTracking()
                              .Where(x => _includeGroups || !(x.IsGroup ?? false))
                              .OrderBy(x => x.Label)
                              .Select(AreaTypeMapper.MapBasic)
                              .ToList();
        }
    }
}
