namespace Esd.FlexibleOpenGeographies
{
    public interface IKmlReader
    {
        string KmlStringForCode(string code);
        string KmlStringForUri(string uri);
    }
}
