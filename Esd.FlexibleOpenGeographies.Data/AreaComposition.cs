using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_composition")]
    public class AreaComposition
    {
        [Key, Required, Column("area_id", Order = 0), ForeignKey("Area")]
        public int AreaId { get; set; }
        [Key, Required, Column("child_area_id", Order = 1), ForeignKey("ChildArea")]
        public int ChildAreaId { get; set; }

        public virtual AreaDetail Area { get; set; }
        public virtual AreaDetail ChildArea { get; set; }
    }
}
