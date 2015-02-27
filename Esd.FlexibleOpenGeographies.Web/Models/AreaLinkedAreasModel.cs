using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaLinkedAreasModel
    {
        public int AreaId { get; set; }
        public IList<AreaBasicWithType> ParentAreas { get; set; }
        public IList<AreaBasicWithType> ChildAreas { get; set; }
    }
}