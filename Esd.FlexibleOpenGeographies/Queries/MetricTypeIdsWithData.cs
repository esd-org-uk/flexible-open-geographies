using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class MetricTypeIdsWithData : IQueryEnumerable<string>
    {
        private readonly IContextFactory _contextFactory;

        public MetricTypeIdsWithData(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<string> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.Metrics.AsNoTracking()                              
                              .Select(m => m.MetricTypeIdentifier)
                              .Distinct()
                              .ToList();
        }
    }
}
