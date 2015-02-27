using Esd.FlexibleOpenGeographies.Dtos;
using Newtonsoft.Json;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class MetricTypeContainer
    {
        [JsonProperty("metricType")]
        public MetricTypeBasicModel MetricType { get; set; }

        public MetricTypeContainer(MetricTypeBasic metricType)
        {
            MetricType = new MetricTypeBasicModel(metricType);
        }
    }
}