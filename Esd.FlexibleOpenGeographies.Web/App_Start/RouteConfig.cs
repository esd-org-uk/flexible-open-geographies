using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Esd.FlexibleOpenGeographies.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("RedirectAreaTypeFormat", "redirect/{type}.{format}", new { controller = "Redirect", action = "Redirect", format = RouteParameter.Optional });
            routes.MapRoute("RedirectAreaType", "redirect/{type}", new { controller = "Redirect", action = "Redirect"});
            routes.MapRoute("RedirectAreaFormat", "redirect/{type}/{code}.{format}", new { controller = "Redirect", action = "Redirect", format = RouteParameter.Optional });
            routes.MapRoute("RedirectArea", "redirect/{type}/{code}", new { controller = "Redirect", action = "Redirect" });            
            routes.MapRoute("Index", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("AddArea", "areas/add", new { controller = "Area", action = "Add" });
            routes.MapRoute("AddAreaResource", "areas/addResource", new { controller = "Area", action = "AddResource" });
            routes.MapRoute("DeleteAreaResource", "areas/deleteResource", new { controller = "Area", action = "DeleteResource" });
            routes.MapRoute("EditAreaResource", "areas/editResource", new { controller = "Area", action = "EditResource" });
            routes.MapRoute("SelectArea", "areas/select", new { controller = "Area", action = "Select" });
            routes.MapRoute("AddAreaParent", "areas/addParent", new { controller = "Area", action = "AddParent" });            
            routes.MapRoute("DeleteAreaParent", "areas/deleteParent", new { controller = "Area", action = "DeleteParent" });            
            routes.MapRoute("EditAreaParents", "areas/{id}/parents", new { controller = "Area", action = "EditParents" });            
            routes.MapRoute("EditAreaChildren", "areas/{id}/children", new { controller = "Area", action = "EditChildren" });            
            routes.MapRoute("SaveAreaChildren", "areas/{id}/children/save", new { controller = "Area", action = "SaveChildren" });            
            routes.MapRoute("AreaParentsPartial", "areaParentsAJAX", new { controller = "Area", action = "Parents" });            
            routes.MapRoute("SuggestAreaParentsPartial", "areaParentsSuggestionAJAX", new { controller = "Area", action = "SuggestParents" });            
            routes.MapRoute("AreaKmlDownload", "areas/{id}/kml", new { controller = "Area", action = "DownloadKml" });
            routes.MapRoute("AreaJsonDownload", "areas/{id}/json", new { controller = "Area", action = "DownloadJson" });
            routes.MapRoute("AreaXmlDownload", "areas/{id}/xml", new { controller = "Area", action = "DownloadXml" });
            routes.MapRoute("EditArea", "areas/{id}", new { controller = "Area", action = "Edit" });            
            routes.MapRoute("MetricsDownload", "metrics/download", new { controller = "Metric", action = "Download" });
            routes.MapRoute("MetricsUpload", "metrics/upload", new { controller = "Metric", action = "Upload" });
            routes.MapRoute("MetricTypesWithData", "metrics/MetricTypesWithData", new { controller = "Metric", action = "MetricTypesWithData" });
            routes.MapRoute("MetricTypesForTerm", "metrics/MetricTypesForTerm", new { controller = "Metric", action = "MetricTypesForTerm" });
            routes.MapRoute("PeriodsForMetricType", "metrics/PeriodsForMetricType", new { controller = "Metric", action = "PeriodsForMetricType" });
            routes.MapRoute("PeriodIdsWithData", "metrics/PeriodIdsWithData", new { controller = "Metric", action = "PeriodIdsWithData" });            
            routes.MapRoute("AreasForTypeAndTerm", "metrics/AreasForTypeAndTerm", new { controller = "Metric", action = "AreasForTypeAndTerm" });
            routes.MapRoute("TypeHierarchyForAreaType", "metrics/TypeHierarchyForAreaType", new { controller = "Metric", action = "TypeHierarchyForAreaType" });                                    
            routes.MapRoute("AreasForType", "areasForTypeAJAX", new { controller = "Area", action = "AreasForType" });
            routes.MapRoute("AreasInBoxForType", "areasInBoxForTypeAJAX", new { controller = "Area", action = "AreasInBoxForType" });
            routes.MapRoute("AreaDetailsPartial", "areasDetailsAJAX", new { controller = "Area", action = "Details" });
            routes.MapRoute("AreaLinkedAreasPartial", "areasLinkedAreasAJAX", new { controller = "Area", action = "LinkedAreas" });
            routes.MapRoute("AreaResourcesPartial", "areasResourcesAJAX", new { controller = "Area", action = "Resources" });
            routes.MapRoute("ChildAreaIds", "childAreaIdsAJAX", new { controller = "Area", action = "ChildAreaIds" });
            routes.MapRoute("AreaTypeResourcesPartial", "areaTypesResourcesAJAX", new { controller = "AreaType", action = "Resources" });
            routes.MapRoute("AncestorTypesForType", "ancestorTypesForTypeAJAX", new { controller = "AreaType", action = "AncestorTypesForType" });
            routes.MapRoute("AddAreaType", "areaTypes/add", new { controller = "AreaType", action = "Add" });
            routes.MapRoute("AddAreaTypeResource", "areaTypes/addResource", new { controller = "AreaType", action = "AddResource" });
            routes.MapRoute("DeleteAreaTypeResource", "areaTypes/deleteResource", new { controller = "AreaType", action = "DeleteResource" });
            routes.MapRoute("EditAreaTypeResource", "areaTypes/editResource", new { controller = "AreaType", action = "EditResource" });
            routes.MapRoute("SelectAreaType", "areaTypes/select", new { controller = "AreaType", action = "Select" });
            routes.MapRoute("EditAreaType", "areaTypes/{code}", new { controller = "AreaType", action = "Edit" });
            routes.MapRoute("AreaTypeJsonFile", "areaTypes/{code}/json", new { controller = "AreaType", action = "DownloadJson" });
            routes.MapRoute("AreaTypeXmlFile", "areaTypes/{code}/xml", new { controller = "AreaType", action = "DownloadXml" });
            routes.MapRoute("EditAreaTypeRelationship", "areaTypes/{code}/relationships", new { controller = "AreaType", action = "EditRelationships" });
            routes.MapRoute("AreaTypeDetailsPartial", "areaTypesDetailsAJAX", new { controller = "AreaType", action = "Details" });
            routes.MapRoute("AreaGeography", "areaGeography", new { controller = "Area", action = "AreaGeography" });
            routes.MapRoute("AreaInBounds", "areaInBounds", new { controller = "Area", action = "AreaInBounds" });
            routes.MapRoute("Colour", "colour", new { controller = "Area", action = "Colour" });
            routes.MapRoute("AreaLabel", "areaLabel", new { controller = "Area", action = "Label" });
            routes.MapRoute("AreaId", "areaId", new { controller = "Area", action = "Id" });
            routes.MapRoute("AreaTypeCodeForLabel", "areaTypeCodeForLabelAJAX", new {controller = "AreaType", action = "CodeForLabel"});
            routes.MapRoute("BoundingBoxForType", "boundingBoxForTypeAJAX", new {controller = "AreaType", action = "BoundingBox"});
            routes.MapRoute("BoundingBoxForArea", "boundingBoxForAreaAJAX", new {controller = "Area", action = "BoundingBox"});
            routes.MapRoute("BoundingBoxForAreas", "boundingBoxForAreasAJAX", new {controller = "Area", action = "BoundingBoxMultiple"});
            routes.MapRoute("AreaAtPointForType", "areaAtPointForTypeAJAX", new { controller = "Area", action = "AreaAtPointForType" });
            routes.MapRoute("AreaTypeCodeError", "areaTypeCodeErrorAJAX", new {controller = "AreaType", action = "ValidateCode"});
            routes.MapRoute("AreaCodeError", "areaCodeErrorAJAX", new {controller = "Area", action = "ValidateCode"});
            routes.MapRoute("AreaIdFilterByType", "areaIdFilterByTypeAJAX", new { controller = "Area", action = "FilterAreaIdsByAreaType" });
            routes.MapRoute("TypeCodesByType", "typeCodesByTypeAJAX", new { controller = "AreaType", action = "TypeCodesByType" });
            routes.MapRoute("AreaByIdOrCode", "areaByIdOrCodeAJAX", new { controller = "Area", action = "AreaByIdOrCode" });
            routes.MapRoute("SignIn", "signIn", new { controller = "OAuth", action = "SignIn" });
            routes.MapRoute("SignOut", "signOut", new { controller = "OAuth", action = "SignOut" });
            routes.MapRoute("Callback", "callback", new { controller = "OAuth", action = "Callback" });
            routes.MapRoute("Query", "data/query", new { controller = "SPARQL", action = "Query" });
            routes.MapRoute("Data", "data/{*catchAll}", new { controller = "SPARQL", action = "Index" });        
        }
    }
}
