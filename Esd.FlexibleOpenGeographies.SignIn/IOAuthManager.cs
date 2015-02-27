using DotNetOpenAuth.OAuth;

namespace Esd.FlexibleOpenGeographies.SignIn
{
    public interface IOAuthManager 
    {
        WebConsumer CreateWebConsumer();
    }
}