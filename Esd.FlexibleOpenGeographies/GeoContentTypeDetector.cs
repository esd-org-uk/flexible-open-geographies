using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace Esd.FlexibleOpenGeographies
{
    public class GeoContentTypeDetector : IGeoContentTypeDetector
    {
        /// <remarks>
        /// This method returns GeoContentType.GeoJson if it is a JSON string and GeoContentType.Kml if it is an XML string.
        /// There is no checking that it is valid GeoJSON or valid KML
        /// </remarks>
        public GeoContentType DetectFromContent(string content)
        {
            if (IsJson(content)) return GeoContentType.GeoJson;
            if (IsXml(content)) return GeoContentType.Kml;
            throw new InvalidOperationException();
        }

        private static bool IsXml(string content)
        {
            try
            {
                var array = XElement.Parse(content);
                return !array.IsEmpty;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool IsJson(string content)
        {
            try
            {
                var array = JArray.Parse(content);
                return array.HasValues;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
