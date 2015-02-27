using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaTypeDetailsModel
    {
        public string Code { get; set; }
        public string Label { get; set; }
        [DisplayName("Can upload metrics")]
        public string MetricUploadPermissionLevelDescription { get; set; }
        [DisplayName("Alternate labels")]
        public IList<string> AlternateLabels { get; set; }
        [DisplayName("Parent area type(s)")]
        public IList<AreaTypeBasic> ParentTypes { get; set; }
        [DisplayName("Child area type(s)")]
        public IList<AreaTypeBasic> ChildTypes { get; set; }
        [DisplayName("Primary for metrics")]
        public AreaTypeBasic PrimaryChildType { get; set; }
        public string Creator { get; set; }
        public string CreatorDescription { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Editable { get; set; }
        [DisplayName("Same as link"), Url]
        public string SameAsLink { get; set; }
        [DisplayName("URI link"), Url]
        public string URILink { get; set; }
        [DisplayName("Is made up of other types")]
        public bool IsGroup { get; set; }
        [DisplayName("Sub-types")]
        public IList<AreaTypeBasic> GroupMembers { get; set; }
    }
}