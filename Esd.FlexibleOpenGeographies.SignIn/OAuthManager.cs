using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.OAuth.ChannelElements;
using Ninject;
using System.Web;

namespace Esd.FlexibleOpenGeographies.SignIn
{
    public class OAuthManager : IOAuthManager
    {
        private readonly IOAuthSettingsProvider _settings;

        [Inject]
        public OAuthManager(IOAuthSettingsProvider settings)
        {
            _settings = settings;
        }

        public WebConsumer CreateWebConsumer()
        {
            var tokenManager = GetTokenManagerKey();
            var serviceProvider = GetServiceDescription();
            return new WebConsumer(serviceProvider, tokenManager);
        }

        private ServiceProviderDescription GetServiceDescription()
        {
            return new ServiceProviderDescription
            {
                AccessTokenEndpoint = new MessageReceivingEndpoint(_settings.ServiceProvider, HttpDeliveryMethods.PostRequest),
                RequestTokenEndpoint = new MessageReceivingEndpoint(_settings.ServiceProvider, HttpDeliveryMethods.PostRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint(_settings.ServiceProvider, HttpDeliveryMethods.PostRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
                ProtocolVersion = ProtocolVersion.V10a
            };
        }

        private IConsumerTokenManager GetTokenManagerKey()
        {
            if (HttpContext.Current.Session["Manager"] == null)
                HttpContext.Current.Session["Manager"] = new InMemoryTokenManager(_settings.ConsumerKey, _settings.SecretKey);
            return (InMemoryTokenManager)HttpContext.Current.Session["Manager"];
        }
    }
}
