using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Esd.FlexibleOpenGeographies.SignIn.Principal
{
    public class FormsAuthenticationUtility
    {
        public static void ToCookie(UserBasic user)
        {
            if (user == null) return;

            var serializeModel = new CustomPrincipalSerializeModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                OrganisationId = user.OrganisationId,
                OrganisationName = user.OrganisationName
            };

            var userData = new JavaScriptSerializer().Serialize(serializeModel);
            var authTicket = new FormsAuthenticationTicket(
                     1,
                     user.Email,
                     DateTime.Now,
                     DateTime.Now.AddDays(1),
                     false,
                     userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        public static void FromCookie()
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null) return;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null) throw new InvalidOperationException("Could not decrypt authentication cookie");
            var serializeModel = new JavaScriptSerializer().Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);

            var newUser = new CustomPrincipal(authTicket.Name)
            {
                UserId = serializeModel.UserId,
                Name = serializeModel.Name,
                Email = serializeModel.Email,
                OrganisationId = serializeModel.OrganisationId,
                OrganisationName = serializeModel.OrganisationName
            };

            HttpContext.Current.User = newUser;
        }
    }
}
