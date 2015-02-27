using System;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class AreaTypeDetails : AreaTypeBasic
    {
        public IList<string> AlternateLabels { get; set; }
        public IList<AreaTypeBasic> ParentTypes { get; set; }
        public IList<AreaTypeBasic> ChildTypes { get; set; }
        public AreaTypeBasic PrimaryChildType { get; set; }
        public string Creator { get; set; }
        public string CreatorDescription { get; set; }
        public string Organisation { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int MetricUploadPermissionLevelId { get; set; }
        public string SameAsLink { get; set; }
        public bool IsGroup { get; set; }
        public IList<AreaTypeBasic> GroupMembers { get; set; }
    }
}
