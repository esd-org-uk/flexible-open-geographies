using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaResourcesModel
    {
        public int AreaId { get; set; }
        public IList<AreaResourceModel> Resources { get; set; }
        public bool Editable { get; set; }
    }
}