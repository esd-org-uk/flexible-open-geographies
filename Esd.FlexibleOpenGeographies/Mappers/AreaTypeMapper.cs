using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public static class AreaTypeMapper
    {
        public static AreaTypeBasic MapBasic(AreaType areaType)
        {
            return new AreaTypeBasic
            {
                Code = areaType.Code,
                Label = areaType.Label
            };
        }

        public static AreaType Map(AreaTypeWithParentAndAlternateLabels areaType)
        {
            return new AreaType
            {
                Code = areaType.Code,
                ShortCode = areaType.ShortCode,
                Label = areaType.Label,
                CreateDate = DateTime.UtcNow,
                MetricUploadPermissionLevelId = areaType.MetricUploadPermissionLevelId,
                AlternateLabels = areaType.AlternateLabels.Select(label => 
                    new AreaTypeAlternateLabel{TypeCode = areaType.Code, Label = label}).ToList(),
                CreatorId = areaType.CurrentUser == null ? null : areaType.CurrentUser.UserId,
                SameAsLink = areaType.SameAsLink,
                IsGroup = areaType.IsGroup
            };
        }

        public static IList<TypeHierarchy> MapTypeHierarchies(AreaTypeWithParentAndAlternateLabels areaType)
        {
            return areaType.ParentTypeCodes
                           .Select(parentTypeCode =>
                                       new TypeHierarchy
                                       {
                                           TypeCode = parentTypeCode,
                                           ChildTypeCode = areaType.Code
                                       })
                           .Concat(areaType.ChildTypeCodes
                                           .Select(childTypeCode =>
                                                       new TypeHierarchy
                                                       {
                                                           TypeCode = areaType.Code,
                                                           ChildTypeCode = childTypeCode,
                                                           IsPrimary = childTypeCode == areaType.PrimaryTypeCode
                                                       }))
                           .ToList();
        }

        public static IList<AreaTypeGroupMember> MapGroupMembers(AreaTypeWithParentAndAlternateLabels areaType)
        {
            return areaType.GroupMemberCodes
                           .Select(x => new AreaTypeGroupMember
                           {
                               TypeCode = areaType.Code,
                               ChildTypeCode = x
                           })
                           .ToList();
        }
    }
}
