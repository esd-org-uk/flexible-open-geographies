using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Queries;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies
{
    public class QueryFactory : IQueryFactory
    {
        private readonly IContextFactory _contextFactory;

        public QueryFactory(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IQueryEnumerable<AreaBasicWithType> CreateAllAreasWithNoKmlQuery()
        {
            return new AllAreasWithNoKml(_contextFactory);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateAllAreasWithKmlQuery()
        {
            return new AllAreasWithKml(_contextFactory);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateAllAreasWithGeometryQuery()
        {
            return new AllAreasWithGeometry(_contextFactory);
        }

        public IQueryEnumerable<AreaTypeBasic> CreateAreaTypesBasicQuery(bool includeGroups)
        {
            return new AreaTypesBasic(_contextFactory, includeGroups);
        }

        public IQueryEnumerable<AreaTypeBasic> CreateAreaTypesByTypesQuery(List<TypeHierarchyBasic> hierarchies)
        {
            var query = new AreaTypesByTypes(_contextFactory);
            query.ForHierarchies(hierarchies);
            return query;
        }        

        public IQuerySingle<UploadBasic> CreateUploadBasicSingleQuery()
        {
            return new UploadBasicSingle(_contextFactory);
        }

        public IQueryEnumerable<MetricTypeBasic> CreateMetricTypesBasicQuery()
        {
            return new MetricTypesBasic(_contextFactory);
        }

        public IQueryEnumerable<PeriodBasic> CreatePeriodBasicByMetricTypeQuery(string identifier)
        {
            var query = new PeriodByMetricType(_contextFactory);
            query.ForCode(identifier);

            return query;
        }

        public IQueryEnumerable<MetricBasic> CreateMetricDownloadQuery(string metricTypeCode, string periodCode, int areaId)
        {
            return CreateMetricDownloadQuery(metricTypeCode, periodCode, areaId, string.Empty);
        }

        public IQueryEnumerable<MetricBasic> CreateMetricDownloadQuery(string metricTypeCode, string periodCode, int areaId, bool includeMissingValues)
        {
            return CreateMetricDownloadQuery(metricTypeCode, periodCode, areaId, string.Empty, includeMissingValues);
        }

        public IQueryEnumerable<MetricBasic> CreateMetricDownloadQuery(string metricTypeCode, string periodCode, int areaId, string areaTypeCode)
        {
            return CreateMetricDownloadQuery(metricTypeCode, periodCode, areaId, areaTypeCode, false);
        }

        public IQueryEnumerable<MetricBasic> CreateMetricDownloadQuery(string metricTypeCode, string periodCode, int areaId, string areaTypeCode, bool includeMissingValues)
        {
            var query = new MetricDownload(_contextFactory);
            query.ForMetricTypeCode(metricTypeCode);
            query.ForPeriodCode(periodCode);
            query.ForAreaId(areaId);
            query.ForAreaTypeCode(areaTypeCode);
            query.ForIncludeMissingValues(includeMissingValues);

            return query;
        }

        public IQueryEnumerable<int> CreateChildAreaIdsForAreaAndAreaTypeQuery(int id, string areaType)
        {
            return new ChildAreaIdsForAreaAndAreaType(_contextFactory, id, areaType);
        }

        public IQueryEnumerable<MetricBasic> CreateMetricDownloadQuery(string metricTypeCode, string periodCode, List<int> areaIds)
        {
            var query = new MetricDownloadWithArea(_contextFactory);
            query.ForMetricTypeCode(metricTypeCode);
            query.ForPeriodCode(periodCode);
            query.ForAreas(areaIds);

            return query;
        }         
       
        public IQuerySingle<MetricTypeBasic> CreateMetricTypeForCodeQuery(string identifier)
        {
            var query = new MetricTypesBasicByCode(_contextFactory);
            query.ForCode(identifier);
            return query;
        }

        public IQueryEnumerable<int> CreateChildAreaCodesForAreasAndAreaTypeQuery(List<int> ids, string areaType)
        {
            var query = new ChildAreaCodesForAreasAndAreaType(_contextFactory);
            query.ForIds(ids);
            query.ForType(areaType);
            return query;
        }

        public IQueryEnumerable<AreaBasicWithType> CreateChildAreasForAreaAndAreaTypeQuery(int id, string areaType)
        {
            var query = new ChildAreasForAreaAndAreaType(_contextFactory);
            query.ForAreaId(id);
            query.ForTypeCode(areaType);
            return query;
        }                

        public IQuerySingle<MetricAggregationBasic> CreateMetricAggregationForAreaTypeAndMetricTypeQuery(string identifier, string areaTypeCode)
        {
            var query = new MetricAggregationByAreaTypeAndMetricType(_contextFactory);
            query.ForCode(identifier);
            query.ForType(areaTypeCode);
            return query;
        }

        public IQueryEnumerable<MetricTypeBasic> CreateMetricTypeForTermQuery(string term)
        {
            var query = new MetricTypesBasicByTerm(_contextFactory);
            query.ForCode(term);
            return query;
        }             

        public IQueryEnumerable<string> CreateMetricTypeIdsWithDataQuery()
        {
            return new MetricTypeIdsWithData(_contextFactory);
        }

        public IQueryEnumerable<AreaTypeBasic> CreateNonEmptyAreaTypesBasicQuery(bool includeGroups)
        {
            return new NonEmptyAreaTypesBasic(_contextFactory, includeGroups);
        }

        public IQueryEnumerable<AreaBasic> CreateAreaBasicForTypeQuery(string typeCode, int limit)
        {
            return new AreaBasicForType(_contextFactory, typeCode, limit);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateParentAreaBasicWithTypeForIdQuery(int id)
        {
            return new ParentAreaBasicWithTypeForId(_contextFactory, id);
        }

        public IQueryEnumerable<AreaTypeBasic> CreateParentAreaTypesForTypeQuery(string typeCode)
        {
            return new ParentAreaTypesForType(_contextFactory, typeCode);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateAreaBasicWithTypeForTypeQuery(string typeCode)
        {
            var query = new AreaBasicWithTypeForType(_contextFactory);
            query.ForType(typeCode);
            return query;
        }        

        public IQueryEnumerable<AreaBasic> CreateAreaBasicForTypeAndTermQuery(string typeCode, string term)
        {
            var query = new AreaBasicForTypeAndTerm(_contextFactory);
            query.ForType(typeCode);
            query.ForTerm(term);
            return query;
        }        

        public IQuerySingle<AreaBasicWithType> CreateAreaBasicWithTypeForIdQuery(int id)
        {
            return new AreaBasicWithTypeForId(_contextFactory, id);
        }

        public IQuerySingle<User> CreateUserByUniqueIdQuery(string id)
        {
            var query = new UserByUniqueId(_contextFactory);
            query.ForCode(id);
            return query;
        }        

        public IQueryEnumerable<AreaTypeBasic> CreateTypeHierarchiesByAreaCodeQuery(string code)
        {
            var query = new HierarchicalAreaTypesByAreaCode(_contextFactory);
            query.ForCode(code);
            return query;
        }

        public IQuerySingle<string> CreateGeoJsonForAreaQuery(string typeCode, string code, string name)
        {
            return new GeoJsonForArea(_contextFactory, typeCode, code, name);
        }

        public IQueryEnumerable<TypeHierarchyBasic> CreateTypeHierarchiesForAreaTypeQuery(string code)
        {
            var query = new TypeHierarchiesForAreaType(_contextFactory);
            query.ForTypeCode(code);
            return query;
        }

        public IQueryEnumerable<TypeHierarchyWithLabels> CreateTypeHierarchiesWithLabelsForTypeQuery(string typeCode)
        {
            return new TypeHierarchiesWithLabelsForType(_contextFactory, typeCode);
        }

        public IQueryEnumerable<AreaForCalcuatedGeometry> CreateAreasForCalculatedGeometryQuery()
        {
            return new AreasForCalculatedGeometry(_contextFactory);
        }

        public IQuerySingle<AreaBasic> CreateAreaBasicForTypeAndCoordinatesQuery(string typeCode, double x, double y)
        {
            return new AreaBasicForTypeAndCoordinates(_contextFactory, typeCode, x, y);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateAreaBasicForCoordinatesQuery(double x, double y)
        {
            return new AreaBasicForCoordinates(_contextFactory, x, y);
        }

        public IQuerySingle<string> CreateValidateTypeCodeQuery(string code)
        {
            return new ValidateTypeCode(_contextFactory, code);
        }

        public IQuerySingle<string> CreateValidateAreaCodeQuery(string code)
        {
            return new ValidateAreaCode(_contextFactory, code);
        }

        public IQuerySingle<BoundingBox> CreateBoundingBoxForTypeQuery(string code)
        {
            return new BoundingBoxForType(_contextFactory, code);
        }

        public IQuerySingle<BoundingBox> CreateBoundingBoxForAreaQuery(string typeCode, string areaCode)
        {
            return new BoundingBoxForArea(_contextFactory, typeCode, areaCode);
        }

        public IQuerySingle<BoundingBox> CreateBoundingBoxForAreasQuery(IEnumerable<int> areaIds)
        {
            return new BoundingBoxForAreas(_contextFactory, areaIds);
        }

        public IQueryEnumerable<int> CreateAreaIdsForTypeAndBoxQuery(string typeCode, double minX, double minY, double maxX, double maxY)
        {
            return new AreaIdsForTypeAndBox(_contextFactory, typeCode, minX, minY, maxX, maxY);
        }

        public IQueryEnumerable<int> CreateFilterAreaIdsByAreaTypeQuery(IEnumerable<int> areaIds, string typeCode)
        {
            return new FilterAreaIdsByAreaType(_contextFactory, areaIds, typeCode);
        }

        public IQuerySingle<string> CreateLabelForAreaIdQuery(int id)
        {
            return new LabelForAreaId(_contextFactory, id);
        }

        public IQueryEnumerable<Dtos.AreaResource> CreateResourcesForAreaQuery(int id)
        {
            return new ResourcesForArea(_contextFactory, id);
        }

        public IQueryEnumerable<Dtos.AreaTypeResource> CreateResourcesForAreaTypeQuery(string typeCode)
        {
            return new ResourcesForAreaType(_contextFactory, typeCode);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateSuggestedParentAreaBasicWithTypeForIdQuery(int areaId)
        {
            return new SuggestedParentAreaBasicWithTypeForId(_contextFactory, areaId);
        }

        public IQuerySingle<int> CreateAreaIdForTypeAndCodeQuery(string areaType, string areaCode)
        {
            return new AreaIdForTypeAndCode(_contextFactory, areaType, areaCode);
        }

        public IQueryEnumerable<int> CreateAreaIdsForTypeAndAncestorQuery(int ancestorId, string typeCode)
        {
            return new AreaIdsForTypeAndAncestor(_contextFactory, ancestorId, typeCode);
        }

        public IQueryEnumerable<int> CreateFilterAreaIdsByAncestorQuery(IEnumerable<int> areas, string typeCode, string boundingType, string boundingArea)
        {
            return new FilterAreaIdsByAncestor(_contextFactory, areas, typeCode, boundingType, boundingArea);
        }

        public IQueryEnumerable<AreaTypeBasic> CreateAncestorTypesForAreaTypeQuery(string typeCode)
        {
            return new AncestorTypesForAreaType(_contextFactory, typeCode);
        }

        public IQueryEnumerable<string> CreateTypeCodesForAreaTypeGroupQuery(string typeCode)
        {
            return new TypeCodesForAreaTypeGroup(_contextFactory, typeCode);
        }

        public IQuerySingle<AreaBasic> CreateAreaByIdOrCodeQuery(string idOrCode, string typeCode)
        {
            return new AreaByIdOrCode(_contextFactory, idOrCode, typeCode);
        }

        public IQuerySingle<AreaDetailsNoGeography> CreateAreaDetailsByIdQuery(int id)
        {
            return new AreaDetailsById(_contextFactory, id);
        }

        public IQuerySingle<AreaDetailsNoGeography> CreateAreaDetailsByTypeAndCodeQuery(string typeCode, string code)
        {
            return new AreaDetailsByTypeAndCode(_contextFactory, typeCode, code);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateAreaBasicWithTypeByBoundingGroup(int areaId, string areaType)
        {
            var query = new AreaDetailsByBoundingGroup(_contextFactory);
            query.ForAreaId(areaId);
            query.ForAreaTypeCode(areaType);
            return query;
        }

        public IQuerySingle<AreaBasicWithType> CreateAreaBasicWithTypeForTypeAndCode(string areaCode, string areaType)
        {
            return new AreaBasicWithTypeForTypeAndCode(_contextFactory, areaType, areaCode);
        }        

        public IQuerySingle<PeriodBasic> CreatePeriodByCodeQuery(string code)
        {
            var query = new PeriodByCode(_contextFactory);
            query.ForCode(code);
            return query;
        }

        public IQueryEnumerable<string> CreatePeriodIdsWithDataQuery()
        {
            return new PeriodIdsWithData(_contextFactory);
        }

        public IQuerySingle<string> CreateKmlForIdQuery(int id)
        {
            return new KmlForId(_contextFactory, id);
        }

        public IQuerySingle<string> CreateKmlForTypeAndCodeQuery(string typeCode, string code)
        {
            return new KmlForTypeAndCode(_contextFactory, typeCode, code);
        }

        public IQueryEnumerable<AreaTypeBasic> CreateChildTypesForAreaTypeQuery(string areaTypeCode)
        {
            return new ChildTypesForAreaType(_contextFactory, areaTypeCode);
        }

        public IQueryEnumerable<AreaBasic> CreateFilteredAreaBasicForTypeQuery(string typeCode, string filter)
        {
            return new FilteredAreaBasicForType(_contextFactory, typeCode, filter);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateParentAreasForAreaQuery(int id)
        {
            return new ParentAreasForArea(_contextFactory, id);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateChildAreasForAreaQuery(int id)
        {
            return new ChildAreasForArea(_contextFactory, id);
        }

        public IQuerySingle<AreaTypeDetails> CreateAreaTypeDetailsByCodeQuery(string code)
        {
            return new AreaTypeDetailsByCode(_contextFactory, code);
        }

        public IQuerySingle<string> CreateShortCodeForTypeCodeQuery(string code)
        {
            var query = new ShortCodeForTypeCode(_contextFactory);
            query.ForType(code);
            return query;
        }

        public IQuerySingle<bool> CreateCanBeEditedQuery(string creator, string organisation,
            UserBasic currentUser)
        {
            return new CanBeEdited(creator, organisation, currentUser);
        }

        public IQuerySingle<string> CreateColourForIdQuery(int id)
        {
            return new ColourForId(_contextFactory, id);
        }

        public IQueryEnumerable<AreaBasicWithType> CreateAreasForUserQuery(UserBasic user)
        {
            return new AreasForUser(_contextFactory, user);
        }

        public IQueryEnumerable<AreaTypeBasic> CreateAreaTypesForUserQuery(UserBasic user)
        {
            return new AreaTypesForUser(_contextFactory, user);
        }

        public IQuerySingle<string> CreateAreaTypeCodeForLabelQuery(string label)
        {
            return new AreaTypeCodeForLabel(_contextFactory, label);
        }

        public IQueryEnumerable<AreaTypeMetricUploadPermissionLevel> CreateAllMetricUploadPermissionLevelsQuery()
        {
            return new AllMetricUploadPermissionLevels(_contextFactory);
        }

        public IQuerySingle<string> CreateMetricUploadPermissionLevelDescriptionByIdQuery(int id)
        {
            return new MetricUploadPermissionLevelDescriptionById(_contextFactory, id);
        }
    }
}
