using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public interface IAreaCodeSelectDropdowns
    {
        string AreaCodeSelectTypeCode { get; set; }
        IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        string AreaCodeSelectAreaCode { get; set; }
        IEnumerable<AreaBasic> Areas { get; set; }
    }
}
