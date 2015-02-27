using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public class MetricTypeMapper
    {
        public static MetricTypeBasic MapBasic(MetricType metricType)
        {
            return new MetricTypeBasic
            {
                Identifier = metricType.Identifier,
                Label = metricType.Label,
                OutputPrecision = metricType.OutputPrecision,
                AggregatableByArea = metricType.AggregatableByArea
            };
        }
    }
}
