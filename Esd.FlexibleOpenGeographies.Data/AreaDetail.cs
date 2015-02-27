using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Esd.FlexibleOpenGeographies.Data
{
    [Table("area_details")]
    public class AreaDetail
    {
        [Key, Column("id")]
        public int Id { get; set; }
        [Column("code", TypeName = "varchar"), MaxLength(10)]
        public string Code { get; set; }
        [Column("label", TypeName = "varchar"), MaxLength(255)]
        public string Label { get; set; }
        [Column("type_code", TypeName = "varchar"), MaxLength(50), ForeignKey("AreaType")]
        public string TypeCode { get; set; }
        [Column("kml", TypeName = "mediumtext")]
        public string ShapeDocument { get; set; }
        [Column("creator", TypeName = "varchar"), MaxLength(255), ForeignKey("Creator")]
        public string CreatorId { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
        [Column("colour", TypeName = "char"), MaxLength(6)]
        public string Colour { get; set; }
        [Column("requires_geometry_calculation")]
        public bool? RequiresGeometryCalculation { get; set; }
        [Column("geometry_calculation_failed")]
        public bool? GeometryCalculationFailed { get; set; }
        [Column("external_link", TypeName = "text")]
        public string SameAsLink { get; set; }

        public virtual User Creator { get; set; }
        public virtual AreaType AreaType { get; set; }

        public virtual ICollection<AreaResource> Resources { get; set; }
        public virtual ICollection<AreaAlternateLabel> AlternateLabels { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<AreaComposition> AreaCompositions { get; set; }
        [InverseProperty("ChildArea")]
        public virtual ICollection<AreaComposition> ParentAreaCompositions { get; set; }
    }
}
