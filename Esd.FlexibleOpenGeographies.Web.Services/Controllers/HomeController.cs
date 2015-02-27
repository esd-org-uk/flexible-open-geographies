using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.Services.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Demo web services";

            return View();
        }
    }
}
