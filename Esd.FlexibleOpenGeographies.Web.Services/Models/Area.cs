using Esd.FlexibleOpenGeographies.Dtos;
using System.Configuration;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class Area
    {
        public string Identifier { get; set; }
        public string Label { get; set; }
        public string Uri { get; set; }
        public AreaType AreaType { get; set; }
        private static readonly string WebRootUrl;

        static Area()
        {
            WebRootUrl = ConfigurationManager.AppSettings["WebSiteRootUrl"];
        }

        public Area() { }

        public Area(AreaBasicWithType area)
        {
            Identifier = area.Code;
            Label = area.Label;
            if (string.IsNullOrEmpty(area.TypeCode)) return;
            AreaType = new AreaType(area);
            Uri = string.Format("{0}/areas/select#area={1},areaType={2}", WebRootUrl, area.Code, area.TypeCode);
        }

        public Area(AreaDetailsNoGeography area)
        {
            Identifier = area.Code;
            Label = area.Label;
        }
    }
}