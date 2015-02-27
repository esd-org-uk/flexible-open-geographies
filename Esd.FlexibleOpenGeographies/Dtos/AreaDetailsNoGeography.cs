using System;

namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class AreaDetailsNoGeography : AreaNoGeographyOrOwnership
    {
        public string Creator { get; set; }
        public string CreatorDescription { get; set; }
        public string Organisation { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
