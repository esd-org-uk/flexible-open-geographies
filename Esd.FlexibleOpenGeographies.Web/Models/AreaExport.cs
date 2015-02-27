using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Utilities;
using System.Xml.Serialization;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    [XmlRoot("Area")]
    public class AreaExport
    {        
        public string Identifier;
        public string Uri;
        public string Label;
        public string SameAsLink;
        public AreaTypeExport Type;

        public AreaExport() { }

        public AreaExport(AreaNoGeographyOrOwnership area, AreaTypeDetails areaType)
        {
            Type = new AreaTypeExport(areaType);
            Uri = UriCreator.GetAreaUri(area.Code, area.TypeCode);
            Identifier = area.Code;
            Label = area.Label;
            SameAsLink = area.SameAsLink;
        }
    }
}