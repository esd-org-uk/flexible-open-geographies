using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.ValidationAttributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaAddModel
    {
        [MaxLength(10)]
        public string Code { get; set; }
        [Required, MaxLength(255)]
        public string Label { get; set; }
        [DisplayName("Alternate labels")]
        public IList<string> AlternateLabels { get; set; }
        [Required, DisplayName("Type")]
        public string TypeCode { get; set; }
        [DisplayName("Area type")]
        public string TypeLabel { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        [Uri(ErrorMessage = "Please enter a valid URL"), DisplayName("KML URI")]
        public string KmlUri { get; set; }
        public string Colour { get; set; }
        [DisplayName("Same as link"), Url]
        public string SameAsLink { get; set; }
    }
}