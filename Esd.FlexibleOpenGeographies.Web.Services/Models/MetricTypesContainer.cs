using Esd.FlexibleOpenGeographies.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class MetricTypesContainer
    {
        [JsonProperty("metricType-array")]
        public MetricTypeBasicModel[] MetricTypes { get; set; }

        public MetricTypesContainer(IEnumerable<MetricTypeBasic> metricTypes)
        {
            MetricTypes = metricTypes.Select(metricType => new MetricTypeBasicModel(metricType)).ToArray();
        }
    }
}