using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaResourceModel
    {
        public int AreaResourceId { get; set; }
        [Required]
        public int AreaId { get; set; }
        [Required]
        public string Label { get; set; }
        [Url, Required]
        public string Url { get; set; }
    }
}