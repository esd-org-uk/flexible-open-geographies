using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class AreaTypeWithParentAndAlternateLabels : AreaTypeBasic, IHasOwner
    {
        public int MetricUploadPermissionLevelId { get; set; }
        public string ShortCode { get; set; }
        public string PrimaryTypeCode { get; set; }
        public UserBasic CurrentUser { get; set; }
        public string SameAsLink { get; set; }
        public bool IsGroup { get; set; }
        public IList<string> AlternateLabels { get; set; }
        public IList<string> ParentTypeCodes { get; set; }
        public IList<string> ChildTypeCodes { get; set; }
        public IList<string> GroupMemberCodes { get; set; }
    }
}
