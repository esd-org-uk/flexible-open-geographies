using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class FilterAreaIdsByAreaType : IQueryEnumerable<int>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IEnumerable<int> _areaIds;
        private readonly string _typeCode;

        public FilterAreaIdsByAreaType(IContextFactory contextFactory, IEnumerable<int> areaIds, string typeCode)
        {
            _contextFactory = contextFactory;
            _areaIds = areaIds ?? new List<int>();
            _typeCode = typeCode;
        }

        public IEnumerable<int> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking()
                              .Where(x => x.TypeCode == _typeCode && _areaIds.Contains(x.Id))
                              .Select(x => x.Id)
                              .ToList();
        }
    }
}
