using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaEditChildrenModelBuilder
    {
        public static AreaEditChildrenModel Build(
            IEnumerable<AreaTypeBasic> allTypes,
            IEnumerable<AreaTypeBasic> types,
            AreaBasicWithType area)
        {
            return new AreaEditChildrenModel
            {
                Area = area,
                ChildTypes = types.ToList(),
                Areas = new List<AreaBasic>(),
                AreaTypes = allTypes
            };
        }
    }
}