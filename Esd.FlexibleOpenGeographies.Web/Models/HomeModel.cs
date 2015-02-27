using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class HomeModel
    {
        public string MessageText { get; set; }
        public IEnumerable<AreaBasicWithType> Areas { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
    }
}