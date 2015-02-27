using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaTypeResourceModel
    {
        public int AreaTypeResourceId { get; set; }
        [Required]
        public string TypeCode { get; set; }
        [Required]
        public string Label { get; set; }
        [Url, Required]
        public string Url { get; set; }
    }
}