namespace Esd.FlexibleOpenGeographies
{
    public interface IFragmentExtractorFactory
    {
        IFragmentExtractor CreateForType(GeoContentType type);
    }
}
