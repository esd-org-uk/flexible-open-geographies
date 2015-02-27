using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("nn_codes")]
    public class ReservedCode
    {
        [Required, Key, Column("type_short_code", Order = 0)]
        public string TypeShortCode { get; set; }
        [Required(AllowEmptyStrings = true), Key, Column("area_code", Order = 1, TypeName = "varchar"), MaxLength(3)]
        public string AreaCodeSuffix { get; set; }
    }
}
