using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{
    public class RedirectController : BaseController
    {
        private readonly IQueryFactory _queryFactory;

        public RedirectController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        [HttpGet]
        public ActionResult Redirect(string code, string type, string format)
        {
            if (string.IsNullOrEmpty(type)) return null;
            if (!string.IsNullOrEmpty(code))
            {
                var area = _queryFactory.CreateAreaBasicWithTypeForTypeAndCode(code, type).Find();
                if (area == null) return null;

                switch (format)
                {
                    case "kml": return RedirectToAction("DownloadKml", "Area", new { ID = area.Id });
                    case "json": return RedirectToAction("DownloadJson", "Area", new { ID = area.Id });
                    case "xml": return RedirectToAction("DownloadXml", "Area", new { ID = area.Id });
                    default: return RedirectToAction("Select", "Area", new {areaId = area.Id});
                }
            }

            switch (format)
            {
                case "json": return RedirectToAction("DownloadJson", "AreaType", new { code = type });
                case "xml": return RedirectToAction("DownloadXml", "AreaType", new { code = type });
                default: return RedirectToAction("Select", "Area", new { typeCode = type });
            }
        }
    }
}