using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaLinkedAreasModelBuilder
    {
        public static AreaLinkedAreasModel Build(int areaId, IList<AreaBasicWithType> parentAreas, IList<AreaBasicWithType> childAreas)
        {
            return new AreaLinkedAreasModel
            {
                AreaId = areaId,
                ParentAreas = parentAreas,
                ChildAreas = childAreas
            };
        }
    }
}