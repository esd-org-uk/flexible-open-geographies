using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("metric_upload_permission_level")]
    public class MetricUploadPermissionLevel
    {
        [Key, Column("id")]
        public int Id { get; set; }
        [Column("description", TypeName = "varchar"), MaxLength(255)]
        public string Description { get; set; }
    }
}
