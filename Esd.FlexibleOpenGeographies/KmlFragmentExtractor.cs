using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Esd.FlexibleOpenGeographies
{
    public class KmlFragmentExtractor : IFragmentExtractor
    {
        private static readonly XNamespace KmlNamespace = "http://www.opengis.net/kml/2.2";

        public string Extract(string content)
        {
            var kmlDocument = XElement.Parse(content);
            var geometries = ExtractGeometries(kmlDocument);
            return ElementsAsString(geometries);
        }

        public string Extract(IEnumerable<string> content)
        {
            var geometries = new List<XElement>();
            foreach (var kml in content)
            {
                try
                {
                    var kmlDocument = XElement.Parse(kml);
                    geometries.AddRange(ExtractGeometries(kmlDocument));
                }
// ReSharper disable once EmptyGeneralCatchClause
                catch {}
            }
            return ElementsAsString(geometries);
        }

        private static string ElementsAsString(ICollection<XElement> geometries)
        {
            return geometries.Any() ? Combine(geometries) : null;
        }

        private static IList<XElement> ExtractGeometries(XContainer element)
        {
            var geometryElements = new List<XElement>();
            geometryElements.AddRange(element.Descendants(KmlNamespace + "Point"));
            geometryElements.AddRange(element.Descendants(KmlNamespace + "LineString"));
            geometryElements.AddRange(element.Descendants(KmlNamespace + "Polygon"));
            return geometryElements;
        }

        private static string Combine(ICollection<XElement> geometries)
        {
            var element = geometries.Count > 1 ? new XElement("MultiGeometry", geometries) : new XElement(geometries.Single());
            return element.ToString();
        }
    }
}
