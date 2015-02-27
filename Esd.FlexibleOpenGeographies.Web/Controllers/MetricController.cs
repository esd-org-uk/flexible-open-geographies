using System.Globalization;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.ActionFilters;
using Esd.FlexibleOpenGeographies.Web.Models;
using Esd.FlexibleOpenGeographies.Web.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{
    public class MetricController : BaseController
    {
        private readonly IQueryFactory _queryFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public MetricController(IQueryFactory queryFactory, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _queryFactory = queryFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpGet]
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult MetricTypesForTerm(string term, bool includeMissing)
        {
            var metricTypes = new List<MetricTypeBasic>(_queryFactory.CreateMetricTypeForTermQuery(term).Fetch());
            var results = new List<MetricTypeBasic>();

            if (!includeMissing)
            {
                var metricTypeIds = _queryFactory.CreateMetricTypeIdsWithDataQuery().Fetch();

                foreach (var id in metricTypeIds)
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        continue;
                    }

                    var key = Convert.ToInt32(id);

                    foreach(var metricType in metricTypes)
                    {
                        if (metricType.Identifier == key)
                        {
                            results.Add(metricType);
                            break;
                        }
                    }
                }
            }
            else
            {
                results = metricTypes;
            }

            return Json(results.Select(metricType => new
            {
                value = metricType.Identifier,
                label = metricType.Label + " (" + metricType.Identifier + ")"
            }),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult MetricTypesWithData()
        {
            var metricTypeIds = _queryFactory.CreateMetricTypeIdsWithDataQuery().Fetch();
            return Json(metricTypeIds, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult PeriodIdsWithData()
        {
            var periodIds = _queryFactory.CreatePeriodIdsWithDataQuery().Fetch();
            return Json(periodIds, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult PeriodsForMetricType(string typeCode)
        {
            var periods = _queryFactory.CreatePeriodBasicByMetricTypeQuery(typeCode).Fetch();
            return Json(periods.Select(period => new
            {
                value = period.Identifier,
                text = period.Label
            }),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Download()
        {
            var model = new MetricDownloadModel();
            
            var metricTypes = _queryFactory.CreateMetricTypesBasicQuery().Fetch();            

            var metricTypesProcessed = new List<MetricTypeSelectItem>();

            foreach(var mt in metricTypes)
            {
                metricTypesProcessed.Add(new MetricTypeSelectItem(mt));
            }

            //model.MetricTypes = metricTypesProcessed;
            model.ParentAreaTypes = _queryFactory.CreateAreaTypesBasicQuery(false).Fetch();
            return View(model);
        }

        [HttpPost]
        public ActionResult Download(MetricDownloadModel model)
        {
            var results = new List<MetricCSVDownloadModel>();

            var metricType = _queryFactory.CreateMetricTypeForCodeQuery(model.MetricType).Find();
            var metricAggregation = _queryFactory.CreateMetricAggregationForAreaTypeAndMetricTypeQuery(model.MetricType, model.ByArea).Find();
            var period = _queryFactory.CreatePeriodByCodeQuery(model.Period).Find();
            var mainArea = _queryFactory.CreateAreaDetailsByTypeAndCodeQuery(model.ParentAreaType, model.Area).Find();
            var metrics = _queryFactory.CreateMetricDownloadQuery(model.MetricType, model.Period, mainArea.Id, model.ByArea, model.IncludeMissing).Fetch().ToList();
            
            if (metricAggregation != null && metricAggregation.IsAggregable && metricType.AggregatableByArea)
            {
                var hierarchys = new List<TypeHierarchyBasic>();
                hierarchys = GetValidAreaTypeTree(model.ByArea, hierarchys);
                if (hierarchys.Count > 0)
                {
                    var areas = _queryFactory.CreateChildAreasForAreaAndAreaTypeQuery(mainArea.Id, model.ByArea).Fetch();
                    var areaMapping = areas.ToDictionary(area => area, area => new AggregatedArea());

                    areaMapping = GetAreaCodesForParentArea(metrics, areaMapping, hierarchys, 0);

                    foreach (var kvp in areaMapping)
                    {
                        var aggregatedMetrics = _queryFactory.CreateMetricDownloadQuery(model.MetricType, model.Period, kvp.Value.Ids).Fetch();

                        Double value = 0;
                        MetricBasic lastMetric = null;
                        foreach(var metricBasic in aggregatedMetrics)
                        {
                            Double val;
                            if (Double.TryParse(metricBasic.Value, out val))
                            {
                                value += val;
                            }
                            lastMetric = metricBasic;
                        }
                        
                        if (lastMetric != null)
                        {
                            lastMetric.Value = value.ToString(CultureInfo.InvariantCulture);
                            lastMetric.AreaIdentifier = kvp.Key.Code;
                            lastMetric.AreaTypeIdentifier = kvp.Key.TypeCode;
                            results.Add(new MetricCSVDownloadModel(lastMetric, metricType, period, kvp.Key, 
                                string.Format("Aggregated from {0}", kvp.Value.AreaTypeLabel)));

                            RemoveMetricAggregated(metrics, kvp.Key);
                        }             
                    }

                    if (model.IncludeMissing)
                    {
                        AddMetrics(model, results, metricType, period, metrics);
                    }
                }
            }
            else
            {
                AddMetrics(model, results, metricType, period, metrics);
            }                                               

            var csv = new CsvExport<MetricCSVDownloadModel>(results);

            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=metrics.csv");
            Response.Write(csv.Export());

            return null;
        }
        
        private static void RemoveMetricAggregated(List<MetricBasic> metrics, AreaBasicWithType areaBasicWithType)
        {
            MetricBasic metricMatch = null;

            foreach (MetricBasic metric in metrics)
            {
                if (metric.AreaIdentifier == areaBasicWithType.Code && metric.AreaTypeIdentifier == areaBasicWithType.TypeCode)
                {
                    metricMatch = metric;
                    break;
                }
            }

            if (metricMatch != null)
            {
                metrics.Remove(metricMatch);
            }
        }

        private void AddMetrics(MetricDownloadModel model, List<MetricCSVDownloadModel> results, MetricTypeBasic metricType, PeriodBasic period, List<MetricBasic> metrics)
        {
            foreach (var metric in metrics)
            {
                var area = _queryFactory.CreateAreaBasicWithTypeForTypeAndCode(metric.AreaIdentifier, model.ByArea).Find();
                results.Add(new MetricCSVDownloadModel(metric, metricType, period, area));
            }
        }

        private Dictionary<AreaBasicWithType, AggregatedArea> GetAreaCodesForParentArea(List<MetricBasic> metrics, Dictionary<AreaBasicWithType, AggregatedArea> areaMapping, List<TypeHierarchyBasic> hierarchys, int index)
        {
            var results = new Dictionary<AreaBasicWithType, AggregatedArea>();

            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(hierarchys[index].ChildTypeCode).Find();

            if (areaType == null)
            {
                return areaMapping;
            }

            foreach (var kvp in areaMapping)
            {
                if (kvp.Value.Ids.Count == 0)
                {
                    var codes = new List<int> {kvp.Key.Id};
                    results[kvp.Key] = new AggregatedArea(new List<int>(_queryFactory.CreateChildAreaCodesForAreasAndAreaTypeQuery(codes, areaType.Code).Fetch()), areaType.Label);                    
                }
                else
                {
                    results[kvp.Key] = new AggregatedArea(new List<int>(_queryFactory.CreateChildAreaCodesForAreasAndAreaTypeQuery(kvp.Value.Ids, areaType.Code).Fetch()), areaType.Label);                    
                }
            }

            index++;
            if (index < hierarchys.Count)
            {
                return GetAreaCodesForParentArea(metrics, results, hierarchys, index);
            }

            return results;
        }

        private List<TypeHierarchyBasic> GetValidAreaTypeTree(string code, List<TypeHierarchyBasic> tree)
        {
            var hierarchies = _queryFactory.CreateTypeHierarchiesForAreaTypeQuery(code).Fetch();

            var sorted = from s in hierarchies
                         orderby s.CoversWhole, s.IsPrimary descending
                         select s;

            var h = new List<TypeHierarchyBasic>(sorted);

            if (!h.Any()) return tree;
            var item = h[0];
            if (item.IsPrimary)
            {
                tree.Add(item);
                return tree;
            }

            var treeCopy = new List<TypeHierarchyBasic>(tree.ToArray());

            foreach(var i in h)
            {
                tree.Add(i);
                tree = GetValidAreaTypeTree(i.ChildTypeCode, tree);

                var currentNode = tree[tree.Count - 1];
                if (!currentNode.CoversWhole)
                {
                    tree = treeCopy;
                }
                if (currentNode.IsPrimary)
                {
                    return tree;
                }                    
            }

            return tree;
        }

        [HttpGet]
        [OutputCache(Duration = 10, VaryByParam = "*")]
        public ActionResult AreasForTypeAndTerm(string typeCode, string term)
        {
            IEnumerable<AreaBasic> areas = new List<AreaBasic>();

            if (!string.IsNullOrEmpty(typeCode) || !string.IsNullOrEmpty(term))
            {
                areas = _queryFactory.CreateAreaBasicForTypeAndTermQuery(typeCode, term).Fetch();
            }

            return Json(areas.Select(area => new
            {
                value = area.Code,
                label = string.Format("{0} ({1})", area.Label, area.Code)
            }),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = 10, VaryByParam = "*")]
        public ActionResult TypeHierarchyForAreaType(string typeCode)
        {
            var typeHierarchies = _queryFactory.CreateTypeHierarchiesByAreaCodeQuery(typeCode).Fetch();
            return Json(typeHierarchies.Select(typeHierarchy => new
            {
                value = typeHierarchy.Code,
                text = typeHierarchy.Label
            }),
                        JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Protected]
        public ActionResult Upload(string message)
        {
            return View(new MetricUploadModel(message));
        }

        [HttpPost, Protected]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    var array = ms.GetBuffer();

                    var s = Encoding.UTF8.GetString(array, 0, array.Length);

                    var upload = new UploadBasic { CSV = s, UserId = User.UserId};

                    _unitOfWorkFactory.CreateAddUploadProcess(upload).Execute();
                }
            }

            return RedirectToAction("Upload", "Metric", new
            {
                message = "File uploaded! Once the import is complete you will be notified by email."
            }); 
        }
    }
}