using System.Linq;
using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class AddMetric : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly MetricBasic _metric;

        public AddMetric(IContextFactory contextFactory, MetricBasic metric)
        {
            _contextFactory = contextFactory;
            _metric = metric;
        }

        public void Execute()
        {
            if (_metric == null)
            {
                return;
            }

            using (var context = _contextFactory.Create())
            {
                Metric metric = context.Metrics.FirstOrDefault(m => m.AreaIdentifier == _metric.AreaIdentifier && m.MetricTypeIdentifier == _metric.MetricTypeIdentifier && m.PeriodIdentifier == _metric.PeriodIdentifier);

                if (metric == null)
                {
                    context.Metrics.Add(MetricMapper.MapBasic(_metric));
                }
                else
                {
                    metric.Value = _metric.Value;
                }
                
                context.SaveChanges();
            }
        }
    }
}
