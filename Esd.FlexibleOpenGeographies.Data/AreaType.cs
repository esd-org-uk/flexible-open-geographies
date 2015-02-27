using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_type")]
    public class AreaType
    {
        [Key, Column("code", TypeName = "varchar"), MaxLength(50)]
        public string Code { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }
        [Column("creator", TypeName = "varchar"), MaxLength(255), ForeignKey("Creator")]
        public string CreatorId { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
        [Column("metric_upload_permission_level_id")]
        public int MetricUploadPermissionLevelId { get; set; }
        [Column("short_code", TypeName = "varchar"), MaxLength(7)]
        public string ShortCode { get; set; }
        [Column("external_link", TypeName = "text")]
        public string SameAsLink { get; set; }
        [Column("is_group")]
        public bool? IsGroup { get; set; }

        public virtual User Creator { get; set; }

        public virtual ICollection<AreaTypeResource> Resources { get; set; }
        public virtual ICollection<AreaDetail> Areas { get; set; }
        public virtual ICollection<AreaTypeAlternateLabel> AlternateLabels { get; set; }
    }
}
