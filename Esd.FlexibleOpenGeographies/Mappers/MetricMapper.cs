using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public class MetricMapper
    {
        public static Metric MapBasic(MetricBasic metric)
        {
            return new Metric
            {
                MetricTypeIdentifier = metric.MetricTypeIdentifier,
                PeriodIdentifier = metric.PeriodIdentifier,
                AreaIdentifier = metric.AreaIdentifier,
                AreaTypeIdentifier = metric.AreaTypeIdentifier,
                Value = metric.Value
            };
        }

        public static MetricBasic Map(Metric metric)
        {
            return new MetricBasic
            {
                MetricTypeIdentifier = metric.MetricTypeIdentifier,
                PeriodIdentifier = metric.PeriodIdentifier,
                AreaIdentifier = metric.AreaIdentifier,
                AreaTypeIdentifier = metric.AreaTypeIdentifier,
                Value = metric.Value
            };
        }
    }
}
