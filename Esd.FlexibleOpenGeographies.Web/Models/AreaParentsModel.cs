using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaParentsModel
    {
        public int AreaId { get; set; }
        public IEnumerable<AreaBasicWithType> Areas { get; set; }
    }
}