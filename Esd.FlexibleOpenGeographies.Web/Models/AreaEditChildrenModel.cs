using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaEditChildrenModel : IAreaCodeSelectDropdowns
    {
        public AreaBasicWithType Area { get; set; }
        public IList<AreaTypeBasic> ChildTypes { get; set; }
        
        public string AreaCodeSelectTypeCode { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        public string AreaCodeSelectAreaCode { get; set; }
        public IEnumerable<AreaBasic> Areas { get; set; }
    }
}