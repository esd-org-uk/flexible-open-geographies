using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public static class AreaMapper
    {
        public static AreaDetail Map(AreaFull area)
        {
            return new AreaDetail
            {
                Code = area.Code,
                Label = area.Label,
                TypeCode = area.TypeCode,
                Colour = area.Colour,
                CreateDate = DateTime.UtcNow,
                AlternateLabels = area.AlternateLabels
                                      .Select(label => new AreaAlternateLabel {AreaId = area.Id, Label = label})
                                      .ToList(),
                ShapeDocument = area.GeometryString,
                AreaCompositions = area.ComprisingAreaIds
                                       .Select(composition => new AreaComposition {AreaId = area.Id, ChildAreaId = composition})
                                       .ToList(),
                CreatorId = area.CurrentUser == null ? null : area.CurrentUser.UserId,
                RequiresGeometryCalculation = string.IsNullOrEmpty(area.GeometryString) && area.ComprisingAreaIds.Any(),
                SameAsLink = area.SameAsLink
            };
        }

        public static AreaBasic MapBasic(AreaDetail area)
        {
            return new AreaBasic
            {
                Id = area.Id,
                Code = area.Code,
                Label = area.Label
            };
        }

        public static AreaDetailsNoGeography MapDetails(AreaDetail area)
        {              
            var creatorDescription = area.Creator == null ? null : area.Creator.Name;
            if (area.Creator != null && area.Creator.Organisation != null)
                creatorDescription = string.Format("{0} ({1})", creatorDescription, area.Creator.Organisation.OrganisationName);
            return new AreaDetailsNoGeography
            {
                Id = area.Id,
                Code = area.Code,
                Label = area.Label,
                AlternateLabels = area.AlternateLabels.Select(label => label.Label).ToList(),
                TypeCode = area.TypeCode,
                TypeLabel = area.AreaType.Label,
                Colour = area.Colour,
                CreateDate = area.CreateDate,
                UpdateDate = area.UpdateDate,
                Creator = area.CreatorId,
                Organisation = area.Creator == null ? null : area.Creator.OrganisationId,
                CreatorDescription = creatorDescription,
                SameAsLink = area.SameAsLink
            };
        }
    }
}
