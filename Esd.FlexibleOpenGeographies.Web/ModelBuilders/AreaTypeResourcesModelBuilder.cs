using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaTypeResourcesModelBuilder
    {
        public static AreaTypeResourcesModel Build(IEnumerable<AreaTypeResource> resources, AreaTypeDetails areaType, bool editable)
        {
            return new AreaTypeResourcesModel
            {
                TypeCode = areaType.Code,
                Editable = editable,
                Resources = resources.Select(x => new AreaTypeResourceModel
                {
                    AreaTypeResourceId = x.AreaTypeResourceId,
                    TypeCode = x.TypeCode,
                    Label = x.Label,
                    Url = x.Url
                }).ToList()
            };
        }
    }
}