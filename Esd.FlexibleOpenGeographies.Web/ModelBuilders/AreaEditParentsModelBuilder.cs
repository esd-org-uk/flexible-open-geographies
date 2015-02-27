using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaEditParentsModelBuilder
    {
        public static AreaEditParentsModel Build(IEnumerable<AreaTypeBasic> types, AreaBasicWithType area)
        {
            return new AreaEditParentsModel
            {
                Area = area,
                ParentTypes = types.ToList()
            };
        }
    }
}