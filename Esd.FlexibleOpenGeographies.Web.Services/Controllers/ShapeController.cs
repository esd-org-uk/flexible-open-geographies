using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Services.Models;
using Esd.FlexibleOpenGeographies.Web.Services.Models.GeoJSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Esd.FlexibleOpenGeographies.Web.Services.Controllers
{
    public class ShapeController : BaseController
    {
        private readonly IQueryFactory _queryFactory;

        public ShapeController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public ShapeController()
        {
        }

        public IHttpActionResult GetShape(string code, string type)
        {
            var area = _queryFactory.CreateAreaDetailsByTypeAndCodeQuery(type, code).Find();

            if (area == null)
            {
                return NotFound();
            }

            IShape shape = null;
            string geoJSON = _queryFactory.CreateGeoJsonForAreaQuery(type, code, area.Label).Find();

            try
            {
                shape = (IShape)JsonConvert.DeserializeObject<PolygonShape>(geoJSON);
            }
            catch
            {
                shape = (IShape)JsonConvert.DeserializeObject<MultiPolygonShape>(geoJSON);
            }

            return Ok(shape);           
        }
    }
}
