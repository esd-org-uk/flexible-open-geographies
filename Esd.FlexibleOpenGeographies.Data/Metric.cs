using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("metric")]
    public class Metric
    {
        [Key, Column("metric_type_identifier", TypeName = "varchar", Order = 0), MaxLength(255)]
        public string MetricTypeIdentifier { get; set; }
        [Key, Column("period_identifier", TypeName = "varchar", Order = 1), MaxLength(255)]
        public string PeriodIdentifier { get; set; }
        [Key, Column("area_identifier", TypeName = "varchar", Order = 2), MaxLength(255)]
        public string AreaIdentifier { get; set; }
        [Key, Column("type_code", TypeName = "varchar", Order = 3), MaxLength(255)]
        public string AreaTypeIdentifier { get; set; }
        [Column("value", TypeName = "varchar"), MaxLength(255)]
        public string Value { get; set; }
    }
}
