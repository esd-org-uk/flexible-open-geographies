using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaTypeSelectModel : IAreaCodeSelectDropdowns
    {
        [Required, DisplayName("Area type")]
        public string TypeCode { get; set; }

        public string AreaCodeSelectTypeCode { get; set; }
        public IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        public string AreaCodeSelectAreaCode { get; set; }
        public IEnumerable<AreaBasic> Areas { get; set; }
    }
}