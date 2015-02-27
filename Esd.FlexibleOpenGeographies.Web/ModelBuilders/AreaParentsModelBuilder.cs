using Esd.FlexibleOpenGeographies.Comparers;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaParentsModelBuilder
    {
        public static AreaParentsModel Build(
            IEnumerable<AreaBasicWithType> parents, 
            IEnumerable<AreaBasicWithType> existing, 
            int areaId)
        {
            var suggestions = parents.Except(existing, new AreaBasicWithTypeIdComparer());
            return new AreaParentsModel { Areas = suggestions, AreaId = areaId };
        }

        public static AreaParentsModel Build(
            IEnumerable<AreaBasicWithType> parents, 
            int areaId)
        {
            return new AreaParentsModel { Areas = parents, AreaId = areaId };
        }
    }
}