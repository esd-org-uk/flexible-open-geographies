using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("upload")]
    public class Upload
    {
        [Key, Column("id")]
        public int Id { get; set; }
        [Column("csv", TypeName = "varchar")]
        public string CSV { get; set; }
        [Column("user_id", TypeName = "varchar"), MaxLength(512)]
        public string UserId { get; set; }
    }
}
