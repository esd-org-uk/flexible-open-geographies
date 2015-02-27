using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaTypeSelectModelBuilder
    {
        public static AreaTypeSelectModel EmptyModel { get { return new AreaTypeSelectModel(); } }

        public static AreaTypeSelectModel WithTypes(this AreaTypeSelectModel model, IEnumerable<AreaTypeBasic> types)
        {
            model.AreaTypes = types;
            return model;
        }

        public static AreaTypeSelectModel WithAreas(this AreaTypeSelectModel model, IEnumerable<AreaBasic> areas)
        {
            model.Areas = areas;
            return model;
        }
    }
}