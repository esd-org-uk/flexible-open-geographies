using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("user")]
    public class User
    {
        [Key, Column("user_id", TypeName = "varchar"), MaxLength(255)]
        public string UserId { get; set; }
        [Column("name", TypeName = "varchar"), MaxLength(255)]
        public string Name { get; set; }
        [Column("email", TypeName = "varchar"), MaxLength(255)]
        public string Email { get; set; }

        [Column("organisation_id", TypeName = "varchar"), MaxLength(255), ForeignKey("Organisation")]
        public string OrganisationId { get; set; }

        [Column("access_token", TypeName = "varchar"), MaxLength(255)]
        public string AccessToken { get; set; }

        public virtual Organisation Organisation { get; set; }
    }
}
