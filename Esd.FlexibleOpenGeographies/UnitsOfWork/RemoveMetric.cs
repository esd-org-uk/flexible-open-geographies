using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class RemoveMetric : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly MetricBasic _metric;

        public RemoveMetric(IContextFactory contextFactory, MetricBasic metric)
        {
            _contextFactory = contextFactory;
            _metric = metric;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var tmp = context.Metrics.SingleOrDefault(m => m.AreaIdentifier == _metric.AreaIdentifier && m.MetricTypeIdentifier == _metric.MetricTypeIdentifier && m.PeriodIdentifier == _metric.PeriodIdentifier);
                context.Metrics.Remove(tmp);
                context.SaveChanges();
            }
        }
    }
}
