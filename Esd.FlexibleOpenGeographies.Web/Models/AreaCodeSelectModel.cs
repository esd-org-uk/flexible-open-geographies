using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaCodeSelectModel : IAreaCodeSelectDropdowns
    {
        public string AreaCodeSelectTypeCode { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        public string AreaCodeSelectAreaCode { get; set; }
        public IEnumerable<AreaBasic> Areas { get; set; }
    }
}