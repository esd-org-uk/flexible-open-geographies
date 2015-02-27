using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class PeriodIdsWithData : IQueryEnumerable<string>
    {
        private readonly IContextFactory _contextFactory;

        public PeriodIdsWithData(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<string> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                return context.Metrics.AsNoTracking()
                              .Select(m => m.PeriodIdentifier)
                              .Distinct()
                              .ToList();
            }
        }
    }
}
