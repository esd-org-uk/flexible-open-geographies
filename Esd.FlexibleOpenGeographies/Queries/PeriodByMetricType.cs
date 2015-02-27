using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class PeriodByMetricType : IQueryEnumerable<PeriodBasic>
    {
        private readonly IContextFactory _contextFactory;
        private int _code;

        public PeriodByMetricType(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForCode(string code)
        {
            _code = int.Parse(code);
        }

        public IEnumerable<PeriodBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                var metricType = context.MetricTypes.AsNoTracking().SingleOrDefault(mt => mt.Identifier == _code);

                if (metricType != null)
                {
                    return context.Periods.AsNoTracking().Where(p => p.Type == metricType.PeriodType && p.Start >= metricType.PeriodStart && p.End <= metricType.PeriodEnd).Select(PeriodMapper.MapBasic).ToList();
                }

                return null;
            }
        }
    }
}
