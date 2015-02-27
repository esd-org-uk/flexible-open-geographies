namespace Esd.FlexibleOpenGeographies.SignIn
{
    public interface IOAuthSettingsProvider
    {
        string ServiceProvider { get; }
        string ConsumerKey { get; }
        string SecretKey { get; }
    }
}
