using Esd.FlexibleOpenGeographies.Web.Services.Models;
using System.Web.Http;

namespace Esd.FlexibleOpenGeographies.Web.Services.Controllers
{
    public class MetricTypesController : ApiController
    {
        private readonly IQueryFactory _queryFactory;

        public MetricTypesController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public MetricTypesController()
        {
        }

        public IHttpActionResult GetMetricTypes()
        {
            var metricTypes = _queryFactory.CreateMetricTypesBasicQuery().Fetch();
            if (metricTypes != null)
            {
                return Ok(new MetricTypesContainer(metricTypes));
            }
            return NotFound();
        }

        public IHttpActionResult GetMetricType(string id)
        {
            var metricType = _queryFactory.CreateMetricTypeForCodeQuery(id).Find();
            if (metricType != null)
            {
                return Ok(new MetricTypeContainer(metricType));
            }
            return NotFound();
        }
    }
}
