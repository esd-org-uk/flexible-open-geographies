using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Utilities;
using System.Xml.Serialization;

namespace Esd.FlexibleOpenGeographies.Web.Models
{
    [XmlRoot("Type")]
    public class AreaTypeExport
    {
        public string Identifier;
        public string Uri;
        public string Label;
        public string SameAsLink;

        public AreaTypeExport() { }

        public AreaTypeExport(AreaTypeDetails areaType)
        {
            Identifier = areaType.Code;
            Uri = UriCreator.GetAreaTypeUri(areaType.Code);
            Label = areaType.Label;
            SameAsLink = areaType.SameAsLink;
        }
    }
}