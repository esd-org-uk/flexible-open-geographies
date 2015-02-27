using System;
namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class MetricBasic : IComparable<MetricBasic>
    {
        public string MetricTypeIdentifier { get; set; }
        public string PeriodIdentifier { get; set; }
        public string AreaIdentifier { get; set; }
        public string AreaTypeIdentifier { get; set; }
        public string Value { get; set; }

        public int CompareTo(MetricBasic other)
        {
            if (other == null)
            {
                return 1;
            }

            int compare = this.MetricTypeIdentifier.CompareTo(other.MetricTypeIdentifier);

            if (compare == 0)
            {
                compare = this.PeriodIdentifier.CompareTo(other.PeriodIdentifier);

                if (compare == 0)
                {
                    compare = this.AreaIdentifier.CompareTo(other.AreaIdentifier);
                }
            }

            return compare;
        }
    }
}
