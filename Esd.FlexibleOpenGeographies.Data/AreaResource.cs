using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_resource")]
    public class AreaResource
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("area_resource_id")]
        public int AreaResourceId { get; set; }
        [Column("area_id"), ForeignKey("Area")]
        public int AreaId { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }
        [Column("uri", TypeName = "text")]
        public string Uri { get; set; }

        public virtual AreaDetail Area { get; set; }
    }
}
