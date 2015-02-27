using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.ModelBuilders
{
    internal static class AreaEditModelBuilder
    {
        public static AreaEditModel Build(AreaDetailsNoGeography area)
        {
            var model = new AreaEditModel
            {
                Id = area.Id,
                Code = area.Code,
                Label = area.Label,
                AlternateLabels = area.AlternateLabels ?? new List<string>(),
                Colour = area.Colour,
                TypeCode = area.TypeCode,
                TypeLabel = area.TypeLabel,
                SameAsLink = area.SameAsLink
            };
            return model;
        }

        public static AreaEditModel WithTypes(this AreaEditModel model, IEnumerable<AreaTypeBasic> types)
        {
            model.AreaTypes = types;
            return model;
        }
    }
}