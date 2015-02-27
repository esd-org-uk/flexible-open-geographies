using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    public class AreaTypeEditRelationshipsModel
    {
        public string TypeCode { get; set; }
        public IList<TypeHierarchyWithLabels> Relationships { get; set; }
        public string PrimaryChildTypeCode { get; set; }
    }
}