using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using Esd.FlexibleOpenGeographies.Utilities;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaDetailsModelBuilder
    {
        public static AreaDetailsModel Build(AreaDetailsNoGeography area, bool editable)
        {
            return new AreaDetailsModel
            {
                Id = area.Id,
                AreaType = new AreaTypeBasic{Code = area.TypeCode, Label = area.TypeLabel},
                Code = area.Code,
                Label = area.Label,
                AlternateLabels = area.AlternateLabels,
                Colour = area.Colour,
                CreateDate = area.CreateDate,
                Creator = area.Creator,
                CreatorDescription = area.CreatorDescription,
                UpdateDate = area.UpdateDate,
                Editable = editable,
                SameAsLink = area.SameAsLink,
                URILink = UriCreator.GetAreaUri(area.Code, area.TypeCode)
            };
        }
    }
}