using System;

namespace Esd.FlexibleOpenGeographies
{
    public class FragmentExtractorFactory : IFragmentExtractorFactory
    {
        public IFragmentExtractor CreateForType(GeoContentType type)
        {
            switch (type)
            {
                case GeoContentType.GeoJson:
                    return new GeoJsonFragmentExtractor();
                case GeoContentType.Kml:
                    return new KmlFragmentExtractor();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
