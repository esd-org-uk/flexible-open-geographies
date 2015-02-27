using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public class MetricAggregationMapper
    {
        public static MetricAggregation MapBasic(MetricAggregationBasic metricAggregation)
        {
            return new MetricAggregation
            {
                MetricTypeIdentifier = metricAggregation.MetricTypeIdentifier,
                TypeCode = metricAggregation.TypeCode,
                IsAggregable = metricAggregation.IsAggregable
            };
        }

        public static MetricAggregationBasic Map(MetricAggregation metricAggregation)
        {
            return new MetricAggregationBasic
            {
                MetricTypeIdentifier = metricAggregation.MetricTypeIdentifier,
                TypeCode = metricAggregation.TypeCode,
                IsAggregable = metricAggregation.IsAggregable
            };
        }
    }
}
