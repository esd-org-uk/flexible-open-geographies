using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaTypeResourcesModel
    {
        public string TypeCode { get; set; }
        public IList<AreaTypeResourceModel> Resources { get; set; }
        public bool Editable { get; set; }
    }
}