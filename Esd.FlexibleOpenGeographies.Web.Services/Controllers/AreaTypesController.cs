using Esd.FlexibleOpenGeographies.Web.Services.Models;
using System.Web.Http;

namespace Esd.FlexibleOpenGeographies.Web.Services.Controllers
{
    public class AreaTypesController : ApiController
    {
        private readonly IQueryFactory _queryFactory;

        public AreaTypesController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public AreaTypesController()
        {
        }

        public IHttpActionResult GetAreaTypes()
        {
            var areaTypes = _queryFactory.CreateAreaTypesBasicQuery(true).Fetch();
            if (areaTypes != null)
            {
                return Ok(new AreaTypesContainer(areaTypes));
            }
            return NotFound();
        }

        public IHttpActionResult GetAreaType(string id)
        {
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(id).Find();
            if (areaType != null)
            {
                return Ok(new AreaTypeContainer(areaType));
            }
            return NotFound();
        }
    }
}
