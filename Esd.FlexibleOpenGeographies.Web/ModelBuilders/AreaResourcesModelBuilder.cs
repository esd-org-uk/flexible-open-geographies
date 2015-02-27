using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaResourcesModelBuilder
    {
        public static AreaResourcesModel Build(IEnumerable<AreaResource> resources, AreaDetailsNoGeography area, bool editable)
        {
            return new AreaResourcesModel
            {
                AreaId = area.Id,
                Editable = editable,
                Resources = resources.Select(x => new AreaResourceModel
                {
                    AreaResourceId = x.AreaResourceId,
                    AreaId = x.AreaId,
                    Label = x.Label,
                    Url = x.Url
                }).ToList()
            };
        }
    }
}