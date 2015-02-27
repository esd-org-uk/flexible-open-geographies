using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaSelectModel : IAreaSelectDropdowns
    {
        [Required, DisplayName("Area type")]
        public string TypeCode { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        [DisplayName("Area")]
        public int AreaId { get; set; }
        public IEnumerable<AreaBasic> Areas { get; set; }
    }
}