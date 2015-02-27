using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.ActionFilters
{
    public class ProtectedAttribute : AuthenticateBaseAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Url != null && ConfigurationManager.AppSettings["HasSignIn"] == "true")
            {
                var controller = GetController(context);
                if (controller != null && controller.User == null)
                {
                    var routeHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                    context.HttpContext.Response.Redirect(string.Format("{0}?returnUrl={1}", routeHelper.RouteUrl("SignIn"), 
                        HttpUtility.UrlEncode(context.HttpContext.Request.Url.AbsoluteUri)));
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}