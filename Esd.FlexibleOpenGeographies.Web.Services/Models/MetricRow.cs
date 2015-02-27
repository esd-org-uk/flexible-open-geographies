using Esd.FlexibleOpenGeographies.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class MetricRow
    {
        [JsonProperty("area")]
        public Area OwnerArea { get; set; }
        [JsonProperty("values")]
        public Metric[] Metrics { get; set; }

        public MetricRow(AreaDetailsNoGeography area, string ownerAreaIdentifier, Metric[] metrics)
        {
            OwnerArea = new Area(area);
            OwnerArea.Identifier = ownerAreaIdentifier;
            Metrics = metrics;
        }
    }
}