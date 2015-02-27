using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaTypeEditModelBuilder
    {
        public static AreaTypeEditModel Build(AreaTypeDetails areaType)
        {
            var model = new AreaTypeEditModel
            {
                Code = areaType.Code,
                Label = areaType.Label,
                MetricUploadPermissionLevelId = areaType.MetricUploadPermissionLevelId,
                SameAsLink = areaType.SameAsLink,
                IsGroup = areaType.IsGroup,
                AlternateLabels = areaType.AlternateLabels ?? new List<string>(),
                ChildTypes = areaType.ChildTypes.Select(x => x.Code).ToList(),
                ParentTypes = areaType.ParentTypes.Select(x => x.Code).ToList(),
                GroupMembers = areaType.GroupMembers.Select(x => x.Code).ToList()
            };
            return model;
        }

        public static AreaTypeEditModel WithTypes(this AreaTypeEditModel model, IEnumerable<AreaTypeBasic> types)
        {
            model.AreaTypes = types;
            return model;
        }

        public static AreaTypeEditModel WithMetricUploadPermissionLevels(this AreaTypeEditModel model, IEnumerable<AreaTypeMetricUploadPermissionLevel> permissions)
        {
            model.MetricUploadPermissionLevels = permissions;
            return model;
        }
    }
}