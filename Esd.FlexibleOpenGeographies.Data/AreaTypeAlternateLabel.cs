using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_type_alternate_label")]
    public class AreaTypeAlternateLabel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("area_type_alternate_label_id")]
        public int AreaTypeAlternateLabelId { get; set; }
        [Column("type_code", TypeName = "varchar"), MaxLength(50), ForeignKey("AreaType")]
        public string TypeCode { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }

        public virtual AreaType AreaType { get; set; }
    }
}
