using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class MetricCSVDownloadModel
    {
        public string MetricTypeIdentifier { get; set; }
        public string MetricTypeLabel { get; set; }
        public string PeriodIdentifier { get; set; }
        public string PeriodLabel { get; set; }
        public string AreaIdentifier { get; set; }
        public string AreaLabel { get; set; }
        public string AreaTypeIdentifier { get; set; }
        public string AreaTypeLabel { get; set; }
        public string Value { get; set; }
        public string Delete { get; set; }
        public string Source { get; set; }

        public MetricCSVDownloadModel() { }

        public MetricCSVDownloadModel(MetricBasic metric, MetricTypeBasic metricType, PeriodBasic period, AreaBasicWithType area, string source) 
            : this(metric, metricType, period, source)
        {
            AreaLabel = area.Label;
            AreaTypeLabel = area.TypeName;
        }

        public MetricCSVDownloadModel(MetricBasic metric, MetricTypeBasic metricType, PeriodBasic period, AreaBasicWithType area)
            : this(metric, metricType, period, area, "Input")
        {
        }

        public MetricCSVDownloadModel(MetricBasic metric, MetricTypeBasic metricType, PeriodBasic period, string source)
        {
            MetricTypeIdentifier = metric.MetricTypeIdentifier;
            MetricTypeLabel = metricType.Label;
            PeriodIdentifier = metric.PeriodIdentifier;
            PeriodLabel = period.Label;
            AreaIdentifier = metric.AreaIdentifier;
            AreaTypeIdentifier = metric.AreaTypeIdentifier;
            decimal value;
            Value = decimal.TryParse(metric.Value, out value) ? value.ToString("F"+metricType.OutputPrecision) : metric.Value;
            Delete = "False";
            Source = source;
        }
    }
}