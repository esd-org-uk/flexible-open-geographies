using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class AreaFull : AreaNoGeographyOrOwnership, IHasOwner
    {
        public string GeometryString { get; set; }
        public IList<int> ComprisingAreaIds { get; set; }
        public UserBasic CurrentUser { get; set; }
    }
}
