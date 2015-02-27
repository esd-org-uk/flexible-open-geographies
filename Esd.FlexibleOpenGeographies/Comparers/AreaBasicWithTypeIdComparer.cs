using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Comparers
{
    public class AreaBasicWithTypeIdComparer : IEqualityComparer<AreaBasicWithType>
    {
        public bool Equals(AreaBasicWithType x, AreaBasicWithType y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(AreaBasicWithType obj)
        {
            return obj == null ? 0 : obj.Id.GetHashCode();
        }
    }
}
