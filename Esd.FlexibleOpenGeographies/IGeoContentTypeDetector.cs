namespace Esd.FlexibleOpenGeographies
{
    public interface IGeoContentTypeDetector
    {
        GeoContentType DetectFromContent(string content);
    }
}
