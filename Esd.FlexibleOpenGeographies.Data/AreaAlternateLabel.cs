using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_alternate_label")]
    public class AreaAlternateLabel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("area_alternate_label_id")]
        public int AreaAlternateLabelId { get; set; }
        [Column("area_id"), ForeignKey("Area")]
        public int AreaId { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }

        public virtual AreaDetail Area { get; set; }
    }
}
