using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies
{
    public interface IFragmentExtractor
    {
        string Extract(string content);
        string Extract(IEnumerable<string> content);
    }
}
