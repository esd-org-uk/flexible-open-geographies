namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class MetricTypeBasic
    {
        public int Identifier { get; set; }
        public string Label { get; set; }
        public int OutputPrecision { get; set; }
        public bool AggregatableByArea { get; set; }
    }
}