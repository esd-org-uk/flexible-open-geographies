using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("metric_type")]
    public class MetricType
    {
        [Key, Column("identifier"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Identifier { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }
        [Column("period_start")]
        public DateTime PeriodStart { get; set; }
        [Column("period_end")]
        public DateTime PeriodEnd { get; set; }
        [Column("period_type", TypeName = "varchar"), MaxLength(255)]
        public string PeriodType { get; set; }
        [Column("output_precision")]
        public int OutputPrecision { get; set; }
        [Column("aggregatable_by_area")]
        public bool AggregatableByArea { get; set; }
    }
}
