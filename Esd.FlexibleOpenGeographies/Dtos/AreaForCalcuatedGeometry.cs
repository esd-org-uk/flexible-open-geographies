using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Dtos
{
    public class AreaForCalcuatedGeometry
    {
        public string AreaCode { get; set; }
        public string TypeCode { get; set; }
        public IEnumerable<string> ChildAreaCodes { get; set; }
        public string ChildTypeCode { get; set; }
    }
}
