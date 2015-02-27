using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class AreaTypeEditableDetails
    {
        public string Code { get; set; }
        public string Label { get; set; }
        public int MetricUploadPermissionLevelId { get; set; }
        public string SameAsLink { get; set; }
        public IList<string> AlternateLabels { get; set; }
        public IList<string> ParentTypes { get; set; }
        public IList<string> ChildTypes { get; set; }
        public IList<string> GroupMembers { get; set; }
    }
}
