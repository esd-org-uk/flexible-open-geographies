using Esd.FlexibleOpenGeographies.Web.Models;
using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IQueryFactory _queryFactory;

        public HomeController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        [HttpGet]
        public ActionResult Index(string message = null)
        {
            var user = UserBasic;
            var areas = _queryFactory.CreateAreasForUserQuery(user).Fetch();
            var types = _queryFactory.CreateAreaTypesForUserQuery(user).Fetch();
            var model = new HomeModel
            {
                MessageText = message, 
                Areas = areas,
                AreaTypes = types
            };
            return View(model);
        }
    }
}