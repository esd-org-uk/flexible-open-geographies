namespace Esd.FlexibleOpenGeographies.Utilities
{
    public class UriCreator
    {
        private const string RootURL = "http://fog.id.esd.org.uk/";

        public static string GetAreaUri(string areaCode, string areaType)
        {
            return string.Concat(GetAreaTypeUri(areaType), "/", areaCode);
        }

        public static string GetAreaTypeUri(string areaType)
        {
            return string.Concat(RootURL, areaType);
        }
    }
}
