using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class MetricTypeSelectItem
    {
        public int Identifier { get; set; }
        public string Label { get; set; }

        public MetricTypeSelectItem(MetricTypeBasic metricType)
        {
            Identifier = metricType.Identifier;
            Label = metricType.Label + " (" + metricType.Identifier + ")";
        }
    }
}