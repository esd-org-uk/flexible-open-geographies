using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class AreaNoGeographyOrOwnership : AreaBasic
    {
        public IList<string> AlternateLabels { get; set; }
        public string TypeCode { get; set; }
        public string TypeLabel { get; set; }
        public string Colour { get; set; }
        public string SameAsLink { get; set; }
    }
}
