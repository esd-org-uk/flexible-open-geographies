using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AggregatedArea
    {
        public List<int> Ids { get; set; }
        public string AreaTypeLabel { get; set; }

        public AggregatedArea()
        {
            Ids = new List<int>();
        }

        public AggregatedArea(List<int> ids, string areaTypeCode)
        {
            Ids = ids;
            AreaTypeLabel = areaTypeCode;
        }
    }
}