using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaEditParentsModel
    {
        public AreaBasicWithType Area { get; set; }
        public IList<AreaTypeBasic> ParentTypes { get; set; }
    }
}