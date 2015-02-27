using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaTypeAddModelBuilder
    {
        public static AreaTypeAddModel EmptyModel { get { return new AreaTypeAddModel{MetricUploadPermissionLevelId = 3}; } }

        public static AreaTypeAddModel WithTypes(this AreaTypeAddModel model, IEnumerable<AreaTypeBasic> types)
        {
            model.AreaTypes = types;
            return model;
        }

        public static AreaTypeAddModel WithMetricUploadPermissionLevels(this AreaTypeAddModel model, IEnumerable<AreaTypeMetricUploadPermissionLevel> permissions)
        {
            model.MetricUploadPermissionLevels = permissions;
            return model;
        }

        public static AreaTypeAddModel ForGroup(this AreaTypeAddModel model, bool group)
        {
            model.IsGroup = group;
            return model;
        }
    }
}