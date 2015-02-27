using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("periods")]
    public class Period
    {
        [Key, Column("identifier", TypeName = "varchar"), MaxLength(255), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Identifier { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }
        [Column("start")]
        public DateTime Start { get; set; }
        [Column("end")]
        public DateTime End { get; set; }
        [Column("type", TypeName = "varchar"), MaxLength(255)]
        public string Type { get; set; }
    }
}
