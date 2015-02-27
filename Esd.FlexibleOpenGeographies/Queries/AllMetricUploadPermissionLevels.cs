using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AllMetricUploadPermissionLevels : IQueryEnumerable<AreaTypeMetricUploadPermissionLevel>
    {
        private readonly IContextFactory _contextFactory;

        public AllMetricUploadPermissionLevels(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<AreaTypeMetricUploadPermissionLevel> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.MetricUploadPermissionLevels.AsNoTracking()
                              .OrderBy(status => status.Id)
                              .Select(MetricUploadPermissionMapper.Map)
                              .ToList();
        }
    }
}
