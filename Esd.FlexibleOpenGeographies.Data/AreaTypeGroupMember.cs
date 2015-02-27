using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_type_group_member")]
    public class AreaTypeGroupMember
    {
        [Required, Key, Column("parent_code", Order = 0, TypeName = "varchar"), MaxLength(50), ForeignKey("AreaType")]
        public string TypeCode { get; set; }
        [Required, Key, Column("child_code", Order = 1, TypeName = "varchar"), MaxLength(50), ForeignKey("ChildAreaType")]
        public string ChildTypeCode { get; set; }

        public virtual AreaType AreaType { get; set; }
        public virtual AreaType ChildAreaType { get; set; }
    }
}
