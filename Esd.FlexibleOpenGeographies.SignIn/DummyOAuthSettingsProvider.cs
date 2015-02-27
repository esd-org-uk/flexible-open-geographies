namespace Esd.FlexibleOpenGeographies.SignIn
{
    public class DummyOAuthSettingsProvider : IOAuthSettingsProvider
    {
        public string ServiceProvider { get { return null; } }
        public string ConsumerKey { get { return null; } }
        public string SecretKey { get { return null; } }
    }
}
