using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public interface IAreaSelectDropdowns
    {
        string TypeCode { get; set; }
        IEnumerable<AreaTypeBasic> AreaTypes { get; set; }
        int AreaId { get; set; }
        IEnumerable<AreaBasic> Areas { get; set; }
    }
}
