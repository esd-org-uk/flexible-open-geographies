using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class MetricTypeBasicModel
    {
        public int Identifier { get; set; }
        public string Label { get; set; }
        public string Uri { get; set; }

        public MetricTypeBasicModel(MetricTypeBasic metricType)
        {
            Identifier = metricType.Identifier;
            Label = metricType.Label;
            Uri = "http://id.esd.org.uk/metricType/" + metricType.Identifier;
        }
    }
}