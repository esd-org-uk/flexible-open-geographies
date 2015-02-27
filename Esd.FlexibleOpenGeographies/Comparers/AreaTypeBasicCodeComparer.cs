using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies.Comparers
{
    public class AreaTypeBasicCodeComparer : IEqualityComparer<AreaTypeBasic>
    {
        public bool Equals(AreaTypeBasic x, AreaTypeBasic y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Code.Equals(y.Code, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(AreaTypeBasic obj)
        {
            return obj == null || obj.Code == null ? 0 : obj.Code.ToLowerInvariant().GetHashCode();
        }
    }
}
