using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.ActionFilters;
using Esd.FlexibleOpenGeographies.Web.ModelBuilders;
using Esd.FlexibleOpenGeographies.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{
    public class AreaController : BaseController
    {
        private readonly IQueryFactory _queryFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IKmlReader _kmlReader;
        private readonly StringComparer _caseInsensitiveComparer = StringComparer.OrdinalIgnoreCase;

        public AreaController(
            IQueryFactory queryFactory,
            IUnitOfWorkFactory unitOfWorkFactory, 
            IKmlReader kmlReader)
        {
            _queryFactory = queryFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
            _kmlReader = kmlReader;
        }

        [HttpGet, Protected]
        public ActionResult Add()
        {
            var model = PopulateDropDowns(AreaAddModelBuilder.EmptyModel);
            return View(model);
        }

        [HttpPost, Protected]
        public ActionResult Add(AreaAddModel model, HttpPostedFileBase kmlFile)
        {
            if (model.KmlUri != null && kmlFile != null)
                ModelState.AddModelError("KmlUri", "Specify either KML URI or file, not both");
            var kml = ReadKmlString(model.KmlUri, kmlFile);
            ValidateNewAreaCode(model.Code, model.TypeCode);
            if (!ModelState.IsValid) return View(PopulateDropDowns(model));
            var area = new AreaFull
            {
                Code = CalculateAreaCode(model),
                Label = model.Label,
                AlternateLabels = model.AlternateLabels
                                       .Where(label => !string.IsNullOrWhiteSpace(label))
                                       .Distinct(_caseInsensitiveComparer)
                                       .ToList(),
                TypeCode = model.TypeCode,
                Colour = model.Colour,
                GeometryString = kml,
                ComprisingAreaIds = new List<int>(),
                CurrentUser = UserBasic,
                SameAsLink = model.SameAsLink
            };
            _unitOfWorkFactory.CreateAddAreaProcess(area).Execute();
            var id = _queryFactory.CreateAreaBasicWithTypeForTypeAndCode(area.Code, area.TypeCode).Find().Id;
            return RedirectToRoute("EditAreaChildren", new {id});
        }

        [HttpGet]
        public ActionResult Select(string typeCode = null, string areaCode = null, int? areaId = null, string tab = null)
        {
            var model = PopulateDropDowns(AreaSelectModelBuilder.EmptyModel.WithParameters(typeCode, areaCode, areaId, _queryFactory));
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var area = _queryFactory.CreateAreaDetailsByIdQuery(id).Find();
            var editable = _queryFactory.CreateCanBeEditedQuery(area.Creator, area.Organisation, UserBasic).Find();
            var model = AreaDetailsModelBuilder.Build(area, editable);
            return PartialView("_Details", model);
        }

        [HttpGet]
        public ActionResult LinkedAreas(int id)
        {
            var parentAreas = _queryFactory.CreateParentAreasForAreaQuery(id).Fetch().ToList();
            var childAreas = _queryFactory.CreateChildAreasForAreaQuery(id).Fetch().ToList();
            var model = AreaLinkedAreasModelBuilder.Build(id, parentAreas, childAreas);
            return PartialView("_LinkedAreas", model);
        }

        [HttpGet]
        public ActionResult Resources(int id)
        {
            var resources = _queryFactory.CreateResourcesForAreaQuery(id).Fetch().ToList();
            var area = _queryFactory.CreateAreaDetailsByIdQuery(id).Find();
            var editable = _queryFactory.CreateCanBeEditedQuery(area.Creator, area.Organisation, UserBasic).Find();
            var model = AreaResourcesModelBuilder.Build(resources, area, editable);
            return PartialView("_Resources", model);
        }

        [HttpGet, Protected]
        public ActionResult Edit(int id)
        {
            var area = _queryFactory.CreateAreaDetailsByIdQuery(id).Find();
            if (area == null)
                return RedirectToRoute("Index", new { message = string.Format("Area with id {0} does not exist", id) });
            if (!_queryFactory.CreateCanBeEditedQuery(area.Creator, area.Organisation, UserBasic).Find())
                return RedirectToRoute("Index", new { message = "Area cannot be edited" });
            var model = AreaEditModelBuilder.Build(area);
            return View(PopulateDropDowns(model));
        }

        [HttpPost, Protected]
        public ActionResult Edit(AreaEditModel model, HttpPostedFileBase kmlFile)
        {
            if (model.KmlUri != null && kmlFile != null)
                ModelState.AddModelError("KmlUri", "Specify either KML URI or file, not both");
            var kml = ReadKmlString(model.KmlUri, kmlFile);
            if (!ModelState.IsValid) return View(PopulateDropDowns(model));
            var area = new AreaFull
            {
                Id = model.Id,
                Code = model.Code,
                Label = model.Label,
                AlternateLabels = model.AlternateLabels
                                       .Where(label => !string.IsNullOrWhiteSpace(label))
                                       .Distinct(_caseInsensitiveComparer)
                                       .ToList(),
                TypeCode = model.TypeCode,
                Colour = model.Colour,
                GeometryString = kml,
                ComprisingAreaIds = new List<int>(),
                CurrentUser = UserBasic,
                SameAsLink = model.SameAsLink
            };
            _unitOfWorkFactory.CreateUpdateAreaProcess(area).Execute();
            return RedirectToRoute("Index", new { message = "Area updated" });
        }

        [HttpGet]
        public ActionResult AreasForType(string typeCode, string term, bool returnCode = false)
        {
            var areas = _queryFactory.CreateFilteredAreaBasicForTypeQuery(typeCode, term).Fetch();
            return returnCode
                ? Json(areas.Select(area => new
                {
                    value = area.Code,
                    label = string.Format("{0} ({1})", area.Label, area.Code)
                }),
                       JsonRequestBehavior.AllowGet)
                : Json(areas.Select(area => new
                {
                    value = area.Id,
                    label = string.Format("{0} ({1})", area.Label, area.Code)
                }),
                       JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult AreasInBoxForType(string typeCode, double minX, double minY, double maxX, double maxY, string boundingType, string boundingArea)
        {
            var areas = _queryFactory.CreateAreaIdsForTypeAndBoxQuery(typeCode, minX, minY, maxX, maxY).Fetch();
            if (!string.IsNullOrEmpty(boundingType) && !string.IsNullOrEmpty(boundingArea))
                areas = _queryFactory.CreateFilterAreaIdsByAncestorQuery(areas, typeCode, boundingType, boundingArea).Fetch();
            return Json(areas, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AreaGeography(int id, string label = null)
        {
            var area = _queryFactory.CreateAreaDetailsByIdQuery(id).Find();
            label = label ?? area.Label;
            var geoJson = _queryFactory.CreateGeoJsonForAreaQuery(area.TypeCode, area.Code, label).Find();
            return Content(geoJson);
        }

        [HttpGet]
        public ActionResult Colour(int id)
        {
            return Content(_queryFactory.CreateColourForIdQuery(id).Find());
        }

        [HttpGet]
        public ActionResult Label(int id)
        {
            return Content(_queryFactory.CreateLabelForAreaIdQuery(id).Find());
        }

        [HttpGet]
        public ActionResult Id(string areaType, string areaCode)
        {
            return Json(_queryFactory.CreateAreaIdForTypeAndCodeQuery(areaType, areaCode).Find(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadKml(int id)
        {
            var kml = _queryFactory.CreateKmlForIdQuery(id).Find();
            AddDownloadHeader(id, "kml");
            return Content(kml, "application/vnd.google-earth.kml+xml");
        }
        
        [HttpGet]
        public ActionResult DownloadJson(int id)
        {
            var area = _queryFactory.CreateAreaDetailsByIdQuery(id).Find();
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(area.TypeCode).Find();
            AddDownloadHeader(id, "json");
            return Json(new AreaExport(area, areaType), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadXml(int id)
        {
            var area = _queryFactory.CreateAreaDetailsByIdQuery(id).Find();
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(area.TypeCode).Find();

            var areaExport = new AreaExport(area, areaType);
            var xmlSerializer = new XmlSerializer(areaExport.GetType());

            AddDownloadHeader(id, "xml");

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, areaExport);
                return Content(textWriter.ToString(), "application/xml");
            }
        }

        [HttpGet]
        public ActionResult AreaAtPointForType(string typeCode, double x, double y, string boundingType, string boundingArea)
        {
            var area = _queryFactory.CreateAreaBasicForTypeAndCoordinatesQuery(typeCode, x, y).Find();
            if (area != null && !string.IsNullOrEmpty(boundingType) && !string.IsNullOrEmpty(boundingArea))
            {
                var bounds = _queryFactory.CreateAreaBasicForTypeAndCoordinatesQuery(boundingType, x, y).Find();
                if (bounds == null || bounds.Code != boundingArea) area = null;
            }
            return Json(area, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ValidateCode(string code)
        {
            return Content(_queryFactory.CreateValidateAreaCodeQuery(code).Find());
        }

        [HttpGet]
        public ActionResult FilterAreaIdsByAreaType(int[] areaIds, string typeCode)
        {
            var filtered = _queryFactory.CreateFilterAreaIdsByAreaTypeQuery(areaIds, typeCode).Fetch();
            return Json(filtered, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BoundingBox(string typeCode, string areaCode)
        {
            var boundingBox = _queryFactory.CreateBoundingBoxForAreaQuery(typeCode, areaCode).Find();
            return Json(boundingBox, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult BoundingBoxMultiple(int[] areaIds)
        {
            var boundingBox = _queryFactory.CreateBoundingBoxForAreasQuery(areaIds).Find();
            return Json(boundingBox, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, Protected]
        public ActionResult AddResource(AreaResourceModel areaResource)
        {
            if (ModelState.IsValid)
            {
                var resource = new AreaResource
                {
                    AreaId = areaResource.AreaId,
                    Label = areaResource.Label,
                    Url = areaResource.Url
                };
                _unitOfWorkFactory.CreateAddAreaResourceProcess(resource).Execute();
            }
            return Resources(areaResource.AreaId);
        }

        [HttpPost, Protected]
        public ActionResult DeleteResource(int areaId, int resourceId)
        {
            _unitOfWorkFactory.CreateDeleteAreaResourceProcess(resourceId).Execute();
            return Resources(areaId);
        }
        
        [HttpPost, Protected]
        public ActionResult EditResource(AreaResource resource)
        {
            _unitOfWorkFactory.CreateEditAreaResourceProcess(resource).Execute();
            return Resources(resource.AreaId);
        }

        [HttpGet, Protected]
        public ActionResult EditParents(int id)
        {
            var area = _queryFactory.CreateAreaBasicWithTypeForIdQuery(id).Find();
            if (area == null) return RedirectToRoute("Index", new {message = string.Format("Could not find area {0}", id)});
            var types = _queryFactory.CreateParentAreaTypesForTypeQuery(area.TypeCode).Fetch();
            var model = AreaEditParentsModelBuilder.Build(types, area);
            return View("EditParents", model);
        }

        [HttpGet, Protected]
        public ActionResult EditChildren(int id)
        {
            var area = _queryFactory.CreateAreaBasicWithTypeForIdQuery(id).Find();
            if (area == null) return RedirectToRoute("Index", new { message = string.Format("Could not find area {0}", id) });
            var areaTypes = _queryFactory.CreateAreaTypesBasicQuery(false).Fetch();
            var types = _queryFactory.CreateChildTypesForAreaTypeQuery(area.TypeCode).Fetch().ToList();
            var model = AreaEditChildrenModelBuilder.Build(areaTypes, types, area);
            return View("EditChildren", model);
        }

        [HttpPost, Protected]
        public ActionResult SaveChildren(int areaId, string areaType, int[] childAreas)
        {
            _unitOfWorkFactory.CreateReplaceChildAreasForAreaAndTypeProcess(areaId, areaType, childAreas).Execute();
            return null;
        }

        [HttpGet]
        public ActionResult Parents(int id)
        {
            var parents = _queryFactory.CreateParentAreaBasicWithTypeForIdQuery(id).Fetch();
            var model = AreaParentsModelBuilder.Build(parents, id);
            return PartialView("_Parents", model);
        }

        [HttpPost, Protected]
        public ActionResult AddParent(AreaCompositionModel model)
        {
            if (model.ParentAreaId != 0) 
                _unitOfWorkFactory.CreateAddAreaCompositionProcess(model.ParentAreaId, model.ChildAreaId).Execute();
            return Parents(model.ChildAreaId);
        }

        [HttpPost, Protected]
        public ActionResult DeleteParent(AreaCompositionModel model)
        {
            _unitOfWorkFactory.CreateDeleteAreaCompositionProcess(model.ParentAreaId, model.ChildAreaId).Execute();
            return Parents(model.ChildAreaId);
        }

        [HttpPost]
        public ActionResult SuggestParents(int areaId)
        {
            var parents = _queryFactory.CreateSuggestedParentAreaBasicWithTypeForIdQuery(areaId).Fetch();
            var existing = _queryFactory.CreateParentAreaBasicWithTypeForIdQuery(areaId).Fetch();
            var model = AreaParentsModelBuilder.Build(parents, existing, areaId);
            return PartialView("_SuggestParents", model);
        }

        [HttpGet]
        public ActionResult ChildAreaIds(int areaId, string typeCode)
        {
            var data = _queryFactory.CreateChildAreaIdsForAreaAndAreaTypeQuery(areaId, typeCode).Fetch();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AreaInBounds(int areaId, string typeCode, string boundingType, string boundingCode)
        {
            if (!_queryFactory.CreateFilterAreaIdsByAncestorQuery(new[] {areaId}, typeCode, boundingType, boundingCode)
                              .Fetch()
                              .Any())
                areaId = 0;
            return Json(areaId, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AreaByIdOrCode(string idOrCode, string typeCode)
        {
            var data = _queryFactory.CreateAreaByIdOrCodeQuery(idOrCode, typeCode).Find();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        private void AddDownloadHeader(int id, string extension)
        {
            var filename = string.Format("{0}.{1}", id, extension);
            var contentDisposition = new ContentDisposition { FileName = filename };
            Response.AddHeader("Content-Disposition", contentDisposition.ToString());
        }

        private string NextCodeForType(string shortCode)
        {
            var suffix = _unitOfWorkFactory.CreateReserveCodeProcess(shortCode).ExecuteWithResult();
            return string.Format("{0}{1}", shortCode, suffix);
        }

        private AreaAddModel PopulateDropDowns(AreaAddModel model)
        {
            var areaTypes = _queryFactory.CreateAreaTypesBasicQuery(false).Fetch();
            return model.WithTypes(areaTypes);
        }

        private AreaSelectModel PopulateDropDowns(AreaSelectModel model)
        {
            var areaTypes = _queryFactory.CreateNonEmptyAreaTypesBasicQuery(true).Fetch();
            var areas = _queryFactory.CreateAreaBasicForTypeQuery(model.TypeCode, 1000).Fetch();
            return model.WithTypes(areaTypes).WithAreas(areas);
        }

        private AreaEditModel PopulateDropDowns(AreaEditModel model)
        {
            var areaTypes = _queryFactory.CreateAreaTypesBasicQuery(false).Fetch();
            return model.WithTypes(areaTypes);
        }

        private string ReadKmlString(string kmlUri, HttpPostedFileBase kmlFile)
        {
            if (kmlUri == null && kmlFile == null) return null;
            try
            {
                if (kmlUri != null) return _kmlReader.KmlStringForUri(kmlUri);
                using (var reader = new StreamReader(kmlFile.InputStream))
                    return reader.ReadToEnd();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("KmlUri", e);
                return null;
            }
        }

        private string CalculateAreaCode(AreaAddModel model)
        {
            var code = model.Code;
            if (string.IsNullOrEmpty(code))
            {
                var shortCode = _queryFactory.CreateShortCodeForTypeCodeQuery(model.TypeCode).Find();
                code = NextCodeForType(shortCode);
            }
            return code;
        }

        private void ValidateNewAreaCode(string areaCode, string typeCode)
        {
            if (string.IsNullOrEmpty(areaCode)) return;
            var existing = _queryFactory.CreateAreaIdForTypeAndCodeQuery(typeCode, areaCode).Find();
            if (existing > 0)
                ModelState.AddModelError("Code", "There is already an area of this type and code");
        }
    }
}