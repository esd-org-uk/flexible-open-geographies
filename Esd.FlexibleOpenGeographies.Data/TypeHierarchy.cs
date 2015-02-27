using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("type_hierarchy")]
    public class TypeHierarchy
    {
        [Required, Key, Column("type_code", Order = 0, TypeName = "varchar"), MaxLength(50), ForeignKey("AreaType")]
        public string TypeCode { get; set; }
        [Required, Key, Column("child_type_code", Order = 1, TypeName = "varchar"), MaxLength(50), ForeignKey("ChildAreaType")]
        public string ChildTypeCode { get; set; }
        [Required, Column("is_primary", TypeName = "bit")]
        public bool IsPrimary { get; set; }
        [Required, Column("covers_whole", TypeName = "bit")]
        public bool CoversWhole { get; set; }

        public virtual AreaType AreaType { get; set; }
        public virtual AreaType ChildAreaType { get; set; }
    }
}
