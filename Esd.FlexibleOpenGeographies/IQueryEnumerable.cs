using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies
{
    public interface IQueryEnumerable<T>
    {
        IEnumerable<T> Fetch();
    }
}
