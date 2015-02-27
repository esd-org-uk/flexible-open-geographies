using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaAddModelBuilder
    {
        public static AreaAddModel EmptyModel { get { return new AreaAddModel(); } }

        public static AreaAddModel WithTypes(this AreaAddModel model, IEnumerable<AreaTypeBasic> types)
        {
            model.AreaTypes = types;
            return model;
        }
    }
}