using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Esd.FlexibleOpenGeographies.Web.Services.Controllers
{
    public class AreasController : BaseController
    {
        private readonly IQueryFactory _queryFactory;

        public AreasController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public AreasController()
        {
        }

        public IHttpActionResult GetAreasByType(string areaType)
        {
            return GetAreasByType(areaType, 1, 1000);
        }

        public IHttpActionResult GetAreasByType(string areaType, int page)
        {
            return GetAreasByType(areaType, page, 1000);
        }

        public IHttpActionResult GetAreasByType(string areaType, int page, int offset)
        {
            page--;
            var areas = new List<AreaBasicWithType>(_queryFactory.CreateAreaBasicWithTypeForTypeQuery(areaType).Fetch());
            var pageSize = Convert.ToInt32(Math.Ceiling((decimal)areas.Count / offset));

            return Ok(new AreasContainer(areas.Skip(page * offset).Take(offset), (page + 1), pageSize));
        }

        public IHttpActionResult GetAreasByCoordinates(double lon, double lat)
        {
            return Ok(new AreasContainer(_queryFactory.CreateAreaBasicForCoordinatesQuery(lon, lat).Fetch()));
        }

        public IHttpActionResult GetAreasByBoundedGroup(string area, string areaType, string byType)
        {
            var mainArea = _queryFactory.CreateAreaBasicWithTypeForTypeAndCode(area, areaType).Find();

            if (mainArea == null)
            {
                return NotFound();
            }

            return Ok(new AreasContainer(_queryFactory.CreateAreaBasicWithTypeByBoundingGroup(mainArea.Id, byType).Fetch()));
        }

        public IHttpActionResult GetArea(string code, string type)
        {
            var area = _queryFactory.CreateAreaBasicWithTypeForTypeAndCode(code, type).Find();
            if (area != null)
            {
                return Ok(new AreaContainer(area));
            }
            return NotFound();
        }
    }
}
