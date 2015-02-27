using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class Metric
    {
        public string Value { get; set; }

        public Metric(MetricBasic metric, int precision)
        {
            decimal value;
            Value = decimal.TryParse(metric.Value, out value) ? value.ToString("F" + precision) : metric.Value;
        }
    }
}