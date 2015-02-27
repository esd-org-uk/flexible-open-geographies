using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaTypeAddModel : IHasLabel
    {
        [MaxLength(50)]
        public string Code { get; set; }
        [Required, MaxLength(255)]
        public string Label { get; set; }
        [DisplayName("Alternate labels")]
        public IList<string> AlternateLabels { get; set; }
        [Required, DisplayName("Metric upload permission level")]
        public int MetricUploadPermissionLevelId { get; set; }
        [DisplayName("Same as link"), Url]
        public string SameAsLink { get; set; }
        [Required, DisplayName("Is made up of other types")]
        public bool IsGroup { get; set; }

        [DisplayName("Parent area type(s)")]
        public IList<string> ParentTypes { get; set; }
        [DisplayName("Child area type(s)")]
        public IList<string> ChildTypes { get; set; }
        [DisplayName("Sub-types")]
        public IList<string> GroupMembers { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        public IEnumerable<AreaTypeMetricUploadPermissionLevel> MetricUploadPermissionLevels { get; set; }
    }
}