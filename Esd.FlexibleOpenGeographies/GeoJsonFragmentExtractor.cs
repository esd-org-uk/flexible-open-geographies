using System;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies
{
    public class GeoJsonFragmentExtractor : IFragmentExtractor
    {
        public string Extract(string content)
        {
            //TODO: This assumes a fragment has been passed in. We should extract as we do from KML.
            //However, JSON tools for .NET are significantly worse than XML tools so this is tricky.
            return content;
        }

        public string Extract(IEnumerable<string> content)
        {
            //Haven't implemented as nothing uses this
            throw new NotImplementedException();
        }
    }
}
