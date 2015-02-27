using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_type_resource")]
    public class AreaTypeResource
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("area_type_resource_id")]
        public int AreaTypeResourceId { get; set; }
        [Column("type_code", TypeName = "varchar"), MaxLength(50), ForeignKey("AreaType")]
        public string TypeCode { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }
        [Column("uri", TypeName = "text")]
        public string Uri { get; set; }

        public virtual AreaType AreaType { get; set; }
    }
}
