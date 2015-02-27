using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.Services.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace Esd.FlexibleOpenGeographies.Web.Services.Controllers
{
    public class DataController : BaseController
    {
        private readonly IQueryFactory _queryFactory;

        public DataController(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public DataController() {}

        public IHttpActionResult GetValues(string metricType, string period, string area, string areaType, string byType)
        {
            var mainArea = _queryFactory.CreateAreaBasicWithTypeForTypeAndCode(area, areaType).Find();

            if (mainArea == null)
            {
                return NotFound();
            }

            var rows = new List<MetricRow>();
            var groups = new List<ColumnGroup>();
            var sortedMetrics = new Dictionary<string, List<Metric>>();

            string[] metricTypes = metricType.Split(',');
            string[] periods = period.Split(',');

            if (metricTypes.Length != periods.Length)
            {
                return BadRequest("There must be an equal number of metric types and periods.");
            }

            for (int i = 0; i < metricTypes.Length; i++)
            {
                Dictionary<string, bool> foundAreas = new Dictionary<string,bool>();

                var metrics = _queryFactory.CreateMetricDownloadQuery(metricTypes[i], periods[i], mainArea.Id, byType).Fetch();
                var metricTypeDetail = _queryFactory.CreateMetricTypeForCodeQuery(metricTypes[i]).Find();
                var periodDetail = _queryFactory.CreatePeriodByCodeQuery(periods[i]).Find();                

                if (metrics != null)
                {
                    foreach (var m in metrics)
                    {
                        if (sortedMetrics.ContainsKey(m.AreaIdentifier))
                        {
                            sortedMetrics[m.AreaIdentifier].Add(new Metric(m,  metricTypeDetail.OutputPrecision));
                        }
                        else
                        {
                            var results = new List<Metric>();
                            for (int j = 0; j < i; j++)
                            {
                                results.Add(null);
                            }
                            results.Add(new Metric(m, metricTypeDetail.OutputPrecision));
                            sortedMetrics.Add(m.AreaIdentifier, results);
                        }
                        foundAreas.Add(m.AreaIdentifier, false);
                    }

                    foreach (KeyValuePair<string, List<Metric>> kvp in sortedMetrics)
                    {
                        if (!foundAreas.ContainsKey(kvp.Key))
                        {
                            sortedMetrics[kvp.Key].Add(null);
                        }
                    }

                    var group = new ColumnGroup();
                    group.metricType = new Column(metricTypeDetail);
                    group.period = new Column(periodDetail);
                    group.valueType = new Column("raw", "Raw value");

                    groups.Add(group);                 
                }
            }

            if (groups.Count > 0)
            {
                foreach (var kvp in sortedMetrics)
                {
                    rows.Add(new MetricRow(_queryFactory.CreateAreaDetailsByTypeAndCodeQuery(byType, kvp.Key).Find(), kvp.Key, kvp.Value.ToArray()));
                }   

                return Ok(new MetricContainer(rows.ToArray(), groups.ToArray()));
            }

            return NotFound();
        }
    }
}
