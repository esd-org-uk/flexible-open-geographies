using Esd.FlexibleOpenGeographies.SignIn.UserProvider;
using Esd.FlexibleOpenGeographies.Web.Controllers;
using Ninject;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.ActionFilters
{
    public abstract class AuthenticateBaseAttribute : ActionFilterAttribute    
    {
        [Inject]
        public IUserProvider Provider { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.Url != null && ConfigurationManager.AppSettings["HasSignIn"] == "true")
            {
                if (Provider.AuthenticationCheck(GetController(context).User))
                {
                    return;
                }
            }
            base.OnActionExecuting(context);
        }

        protected BaseController GetController(ActionExecutingContext context)
        {
            if (!(context.Controller is BaseController))
            {
                throw new InvalidOperationException("The attribute should only be used on a BaseController");
            }
            return context.Controller as BaseController;            
        }
    }
}