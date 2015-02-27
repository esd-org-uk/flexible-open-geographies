using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models.GeoJSON
{
    [Serializable]
    public class MultipleGeometry
    {
        public string type;
        public List<List<List<List<double>>>> coordinates { get; set; }
    }
}