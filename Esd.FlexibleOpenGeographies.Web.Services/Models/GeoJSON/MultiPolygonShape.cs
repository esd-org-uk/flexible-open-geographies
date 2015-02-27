using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models.GeoJSON
{
    [Serializable]
    public class MultiPolygonShape : IShape
    {
        public string type;
        public MultipleGeometry geometry;
        public GeoProperties properties;
    }
}