using Esd.FlexibleOpenGeographies.SignIn;
using Esd.FlexibleOpenGeographies.SignIn.Principal;
using Esd.FlexibleOpenGeographies.SignIn.UserProvider;
using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{
    public class OAuthController : Controller
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IUserProvider _userProvider;
        private readonly IOAuthManager _oAuthManager;

        [Inject]
        public OAuthController(
            IUnitOfWorkFactory unitOfWorkFactory, 
            IUserProvider userProvider, 
            IOAuthManager oAuthManager)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _userProvider = userProvider;
            _oAuthManager = oAuthManager;
        }

        [HttpGet]
        public void SignIn(string returnUrl)
        {
            var consumer = _oAuthManager.CreateWebConsumer();

            var callbackUrl = Url.Action("Callback", null, new { returnUrl }, "http");
            if (string.IsNullOrWhiteSpace(callbackUrl))
                throw new InvalidOperationException("No Callback route defined");
            var callbackUri = new Uri(callbackUrl);

            try
            {
                consumer.Channel.Send(consumer.PrepareRequestUserAuthorization(callbackUri, null, null));
            }
            catch (Exception)
            {
                if (returnUrl == null)
                    returnUrl = "/";
                Response.Redirect(returnUrl);
            }
        }

        [HttpGet]
        public ActionResult Callback(string returnUrl)
        {
            var consumer = _oAuthManager.CreateWebConsumer();

            var accessTokenResponse = consumer.ProcessUserAuthorization();
            if (accessTokenResponse == null)
                return Redirect(returnUrl);

            if (_userProvider != null)
            {
                FormsAuthenticationUtility.ToCookie(_userProvider.CreateUser(_unitOfWorkFactory, accessTokenResponse.AccessToken));
            }

            return Redirect(returnUrl);
        }

        [HttpGet]
        public ActionResult SignOut(string returnUrl)
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            _userProvider.SignOut(returnUrl);
            return Redirect("/");
        }
    }
}