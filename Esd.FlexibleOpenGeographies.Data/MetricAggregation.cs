using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("metric_aggregation")]
    public class MetricAggregation
    {
        [Key, Column("metric_type_identifier", Order = 0), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MetricTypeIdentifier { get; set; }
        [Key, Column("type_code", TypeName = "varchar", Order = 1), MaxLength(255), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string TypeCode { get; set; }
        [Column("is_aggregable")]
        public bool IsAggregable { get; set; }
    }
}
