using Esd.FlexibleOpenGeographies.Dtos;
using System.Globalization;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class Column
    {
        public string Identifier { get; set; }
        public string Label { get; set; }

        public Column(string identifier, string label)
        {
            Identifier = identifier;
            Label = label;
        }

        public Column(MetricTypeBasic metricType)
        {
            Identifier = metricType.Identifier.ToString(CultureInfo.InvariantCulture);
            Label = metricType.Label;
        }

        public Column(PeriodBasic period)
        {
            Identifier = period.Identifier;
            Label = period.Label;
        }        
    }
}