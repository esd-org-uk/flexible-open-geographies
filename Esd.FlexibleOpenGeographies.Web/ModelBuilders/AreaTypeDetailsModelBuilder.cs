using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using Esd.FlexibleOpenGeographies.Utilities;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaTypeDetailsModelBuilder
    {
        public static AreaTypeDetailsModel Build(AreaTypeDetails areaType, bool editable, string permission)
        {
            return new AreaTypeDetailsModel
            {
                Code = areaType.Code,
                Label = areaType.Label,
                AlternateLabels = areaType.AlternateLabels,
                MetricUploadPermissionLevelDescription = permission,
                ChildTypes = areaType.ChildTypes,
                ParentTypes = areaType.ParentTypes,
                PrimaryChildType = areaType.PrimaryChildType,
                Creator = areaType.Creator,
                CreatorDescription = areaType.CreatorDescription,
                CreateDate = areaType.CreateDate,
                UpdateDate = areaType.UpdateDate,
                Editable = editable,
                SameAsLink = areaType.SameAsLink,
                URILink = UriCreator.GetAreaTypeUri(areaType.Code),
                IsGroup = areaType.IsGroup,
                GroupMembers = areaType.GroupMembers
            };
        }
    }
}