using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.SignIn.Principal;
using System;
using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{    
    public abstract class BaseController : Controller
    {
        public new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        public UserBasic UserBasic
        {
            get
            {
                return User == null
                    ? new UserBasic
                    {
                        UserId = Guid.Empty.ToString(),
                        Email = "dummy@dummy.com",
                        Name = "Dummy Dummy",
                        OrganisationId = Guid.Empty.ToString(),
                        OrganisationName = "Dummy Organisation"
                    }
                    : User.UserBasic;
            }
        }
    }
}