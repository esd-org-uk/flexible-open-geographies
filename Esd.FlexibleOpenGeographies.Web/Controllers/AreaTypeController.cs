using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Web.ActionFilters;
using Esd.FlexibleOpenGeographies.Web.ModelBuilders;
using Esd.FlexibleOpenGeographies.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Esd.FlexibleOpenGeographies.Web.Controllers
{
    public class AreaTypeController : BaseController
    {     
        private readonly IQueryFactory _queryFactory;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly StringComparer _caseInsensitiveComparer = StringComparer.OrdinalIgnoreCase;

        public AreaTypeController(IQueryFactory queryFactory, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _queryFactory = queryFactory;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        [HttpGet, Protected]
        public ActionResult Add(bool group = false)
        {

            var model = PopulateDropDowns(AreaTypeAddModelBuilder.EmptyModel.ForGroup(group));
            return View(model);
        }

        [HttpPost, Protected]
        public ActionResult Add(AreaTypeAddModel model)
        {
            ValidateHierarchy(model);
            ValidateGroupMembership(model);
            if (!ModelState.IsValid) return View(PopulateDropDowns(model));
            var code = string.IsNullOrWhiteSpace(model.Code) ? NextTypeCode() : model.Code;
            var shortCode = code.Length > 7 ? NextTypeCode() : code;
            var areaType = new AreaTypeWithParentAndAlternateLabels
            {
                Code = code,
                Label = model.Label,
                AlternateLabels = model.AlternateLabels
                                       .Where(label => !string.IsNullOrWhiteSpace(label))
                                       .Distinct(_caseInsensitiveComparer)
                                       .ToList(),
                ParentTypeCodes = model.ParentTypes ?? new List<string>(),
                ChildTypeCodes = model.ChildTypes ?? new List<string>(),
                PrimaryTypeCode = model.ChildTypes != null && model.ChildTypes.Any()
                    ? model.ChildTypes.First()
                    : null,
                MetricUploadPermissionLevelId = model.MetricUploadPermissionLevelId,
                ShortCode = shortCode,
                CurrentUser = UserBasic,
                SameAsLink = model.SameAsLink,
                IsGroup = model.IsGroup,
                GroupMemberCodes = model.GroupMembers ?? new List<string>()
            };
            _unitOfWorkFactory.CreateAddAreaTypeProcess(areaType).Execute();
            return areaType.ChildTypeCodes.Any() || areaType.ParentTypeCodes.Any()
                ? RedirectToRoute("EditAreaTypeRelationship", new { code })
                : RedirectToRoute("Index", new { message = "Area type added" });
        }

        [HttpGet, Protected]
        public ActionResult Edit(string code)
        {
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(code).Find();
            if (areaType == null)
                return RedirectToRoute("Index", new { message = string.Format("Area type with code {0} does not exist", code) });
            if (!_queryFactory.CreateCanBeEditedQuery(areaType.Creator, areaType.Organisation, UserBasic).Find())
                return RedirectToRoute("Index", new { message = "Area type cannot be edited" });
            var model = AreaTypeEditModelBuilder.Build(areaType);
            return View(PopulateDropDowns(model));
        }

        [HttpPost, Protected]
        public ActionResult Edit(AreaTypeEditModel model)
        {
            ValidateHierarchy(model);
            if (!ModelState.IsValid) return View(PopulateDropDowns(model));
            var areaType = new AreaTypeEditableDetails
            {
                Code = model.Code,
                Label = model.Label,
                AlternateLabels = model.AlternateLabels
                                       .Where(label => !string.IsNullOrWhiteSpace(label))
                                       .Distinct(_caseInsensitiveComparer)
                                       .ToList(),
                MetricUploadPermissionLevelId = model.MetricUploadPermissionLevelId,
                ChildTypes = model.ChildTypes ?? new List<string>(),
                ParentTypes = model.ParentTypes ?? new List<string>(),
                GroupMembers = model.GroupMembers ?? new List<string>(),
                SameAsLink = model.SameAsLink
            };
            _unitOfWorkFactory.CreateUpdateAreaTypeProcess(areaType).Execute();
            return areaType.ChildTypes.Any() || areaType.ParentTypes.Any()
                ? RedirectToRoute("EditAreaTypeRelationship", new {code = model.Code})
                : RedirectToRoute("Index", new {message = "Area type updated"});
        }

        [HttpGet, Protected]
        public ActionResult EditRelationships(string code)
        {
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(code).Find();
            if (areaType == null)
                return RedirectToRoute("Index", new { message = string.Format("Area type with code {0} does not exist", code) });
            if (!_queryFactory.CreateCanBeEditedQuery(areaType.Creator, areaType.Organisation, UserBasic).Find())
                return RedirectToRoute("Index", new { message = "Area type cannot be edited" });
            var model = new AreaTypeEditRelationshipsModel
            {
                TypeCode = code,
                Relationships = _queryFactory.CreateTypeHierarchiesWithLabelsForTypeQuery(code).Fetch().ToList()
            };
            return View(model);
        }

        [HttpPost, Protected]
        public ActionResult EditRelationships(AreaTypeEditRelationshipsModel model)
        {
            //Model binding an array to radio buttons doesn't work smoothly - need to set IsPrimary
            foreach (var relationship in model.Relationships)
            {
                if (relationship.TypeCode == model.TypeCode)
                    relationship.IsPrimary = model.PrimaryChildTypeCode == relationship.ChildTypeCode;
                _unitOfWorkFactory.CreateUpdateAreaTypeRelationshipProcess(relationship).Execute();
            }
            return RedirectToRoute("Index", new { message = "Area type updated" });
        }

        [HttpGet]
        public ActionResult Select(string typeCode = null, string tab = null)
        {
            var model = PopulateDropDowns(AreaTypeSelectModelBuilder.EmptyModel);
            model.TypeCode = typeCode;
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(string code)
        {
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(code).Find();
            var permission = _queryFactory.CreateMetricUploadPermissionLevelDescriptionByIdQuery(areaType.MetricUploadPermissionLevelId).Find();
            var editable = _queryFactory.CreateCanBeEditedQuery(areaType.Creator, areaType.Organisation, UserBasic).Find();
            var model = AreaTypeDetailsModelBuilder.Build(areaType, editable, permission);
            return PartialView("_Details", model);
        }

        [HttpGet]
        public ActionResult DownloadJson(string code)
        {
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(code).Find();            
            AddDownloadHeader(code, "json");
            return Json(new AreaTypeExport(areaType), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DownloadXml(string code)
        {
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(code).Find();

            var areaExport = new AreaTypeExport(areaType);
            var xmlSerializer = new XmlSerializer(areaExport.GetType());
            AddDownloadHeader(code, "xml");

            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, areaExport);
                return Content(textWriter.ToString(), "application/xml");
            }
        }

        [HttpGet]
        public ActionResult CodeForLabel(string label)
        {
            return Content(_queryFactory.CreateAreaTypeCodeForLabelQuery(label).Find());
        }

        [HttpGet]
        public ActionResult ValidateCode(string code)
        {
            return Content(_queryFactory.CreateValidateTypeCodeQuery(code).Find());
        }

        [HttpGet]
        public ActionResult BoundingBox(string code)
        {
            var boundingBox = _queryFactory.CreateBoundingBoxForTypeQuery(code).Find();
            return Json(boundingBox, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Resources(string code)
        {
            var resources = _queryFactory.CreateResourcesForAreaTypeQuery(code).Fetch().ToList();
            var areaType = _queryFactory.CreateAreaTypeDetailsByCodeQuery(code).Find();
            var editable = _queryFactory.CreateCanBeEditedQuery(areaType.Creator, areaType.Organisation, UserBasic).Find();
            var model = AreaTypeResourcesModelBuilder.Build(resources, areaType, editable);
            return PartialView("_Resources", model);
        }

        [HttpPost, Protected]
        public ActionResult AddResource(AreaTypeResourceModel resource)
        {
            if (ModelState.IsValid)
            {
                var dto = new AreaTypeResource
                {
                    TypeCode = resource.TypeCode,
                    Label = resource.Label,
                    Url = resource.Url
                };
                _unitOfWorkFactory.CreateAddAreaTypeResourceProcess(dto).Execute();
            }
            return Resources(resource.TypeCode);
        }

        [HttpPost, Protected]
        public ActionResult DeleteResource(string typeCode, int resourceId)
        {
            _unitOfWorkFactory.CreateDeleteAreaTypeResourceProcess(resourceId).Execute();
            return Resources(typeCode);
        }

        [HttpPost, Protected]
        public ActionResult EditResource(AreaTypeResource resource)
        {
            _unitOfWorkFactory.CreateEditAreaTypeResourceProcess(resource).Execute();
            return Resources(resource.TypeCode);
        }

        [HttpGet]
        public ActionResult AncestorTypesForType(string typeCode)
        {
            var ancestors = _queryFactory.CreateAncestorTypesForAreaTypeQuery(typeCode).Fetch().OrderBy(x => x.Label);
            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TypeCodesByType(string typeCode)
        {
            var members = _queryFactory.CreateTypeCodesForAreaTypeGroupQuery(typeCode).Fetch();
            return Json(members, JsonRequestBehavior.AllowGet);
        }

        private void AddDownloadHeader(string code, string extension)
        {
            var filename = string.Format("{0}.{1}", code, extension);
            var contentDisposition = new ContentDisposition { FileName = filename };
            Response.AddHeader("Content-Disposition", contentDisposition.ToString());
        }

        private string NextTypeCode()
        {
            return _unitOfWorkFactory.CreateReserveTypeCodeProcess().ExecuteWithResult();
        }

        private AreaTypeAddModel PopulateDropDowns(AreaTypeAddModel model)
        {
            var areaTypes = _queryFactory.CreateAreaTypesBasicQuery(false).Fetch();
            var permissions = _queryFactory.CreateAllMetricUploadPermissionLevelsQuery().Fetch();
            return model.WithTypes(areaTypes).WithMetricUploadPermissionLevels(permissions);
        }

        private AreaTypeSelectModel PopulateDropDowns(AreaTypeSelectModel model)
        {
            var areaTypes = _queryFactory.CreateAreaTypesBasicQuery(true).Fetch();
            var areas = _queryFactory.CreateAreaBasicForTypeQuery(model.TypeCode, 1000).Fetch(); 
            return model.WithTypes(areaTypes).WithAreas(areas);
        }

        private AreaTypeEditModel PopulateDropDowns(AreaTypeEditModel model)
        {
            var areaTypes = _queryFactory.CreateAreaTypesBasicQuery(false).Fetch();
            var permissions = _queryFactory.CreateAllMetricUploadPermissionLevelsQuery().Fetch();
            return model.WithTypes(areaTypes).WithMetricUploadPermissionLevels(permissions);
        }

        private void ValidateHierarchy(IList<string> parentTypes, IList<string> childTypes)
        {
            var parents = parentTypes ?? new List<string>();
            var children = childTypes ?? new List<string>();
            if (parents.Intersect(children).Any())
                ModelState.AddModelError("ChildTypes", "Area types cannot consist of area types they are contained by");
        }

        private void ValidateHierarchy(AreaTypeEditModel model)
        {
            ValidateHierarchy(model.ParentTypes, model.ChildTypes);
        }

        private void ValidateHierarchy(AreaTypeAddModel model)
        {
            ValidateHierarchy(model.ParentTypes, model.ChildTypes);
        }

        private void ValidateGroupMembership(AreaTypeAddModel model)
        {
            if (model.IsGroup && (model.GroupMembers == null || !model.GroupMembers.Any()))
                ModelState.AddModelError("GroupMembers", "At least one group member must be selected");
        }
    }
}