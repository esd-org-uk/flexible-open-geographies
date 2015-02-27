using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.UnitsOfWork;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IContextFactory _contextFactory;
        private readonly IFragmentExtractorFactory _fragmentExtractor;
        private readonly IGeoContentTypeDetector _geoContentTypeDetector;

        public UnitOfWorkFactory(
            IContextFactory contextFactory, 
            IFragmentExtractorFactory fragmentExtractor, 
            IGeoContentTypeDetector geoContentTypeDetector)
        {
            _contextFactory = contextFactory;
            _fragmentExtractor = fragmentExtractor;
            _geoContentTypeDetector = geoContentTypeDetector;
        }

        public IUnitOfWork CreateUpdateKmlProcess(int areaId, string kmlString)
        {
            return new UpdateKml(_contextFactory, areaId, kmlString);
        }

        public IUnitOfWork CreateAddAreaProcess(AreaFull area)
        {
            var addGeometry = CreateUpsertGeometryProcess(area.Code, area.GeometryString, area.TypeCode);
            return new AddArea(_contextFactory, area, addGeometry);
        }

        public IUnitOfWork CreateAddMetricProcess(MetricBasic metric)
        {
            return new AddMetric(_contextFactory, metric);
        }

        public IUnitOfWork CreateRemoveMetricProcess(MetricBasic metric)
        {
            return new RemoveMetric(_contextFactory, metric);
        }  

        public IUnitOfWork CreateAddUploadProcess(UploadBasic upload)
        {
            return new AddUpload(_contextFactory, upload);
        }

        public IUnitOfWork CreateRemoveUploadProcess(UploadBasic upload)
        {
            return new RemoveUpload(_contextFactory, upload);
        }   

        public IUnitOfWork CreateAddAreaTypeProcess(AreaTypeWithParentAndAlternateLabels areaType)
        {
            return new AddAreaType(_contextFactory, areaType);
        }     

        public IUnitOfWorkWithResult<string> CreateReserveCodeProcess(string shortCode)
        {
            return new ReserveCode(_contextFactory, shortCode);
        }

        public IUnitOfWorkWithResult<string> CreateReserveTypeCodeProcess()
        {
            return new ReserveTypeCode(_contextFactory);
        }

        public IUnitOfWork CreateUpdateAreaProcess(AreaFull area)
        {
            var addGeometry = CreateUpsertGeometryProcess(area.Code, area.GeometryString, area.TypeCode);
            return new UpdateArea(_contextFactory, area, addGeometry);
        }

        public IUnitOfWork CreateUpdateAreaTypeProcess(AreaTypeEditableDetails areaType)
        {
            return new UpdateAreaType(_contextFactory, areaType);
        }

        public IUnitOfWork CreateUpsertGeometryProcess(string areaCode, string content, string typeCode)
        {
            return new UpsertGeometry(_contextFactory, content, areaCode, typeCode, _geoContentTypeDetector, _fragmentExtractor, this);
        }

        public IUnitOfWork CreateUpsertUser(UserBasic user)
        {
            return new UpsertUser(_contextFactory, user);
        }

        public IUnitOfWork CreateUpsertOrganisation(OrganisationBasic org)
        {
            return new UpsertOrganisation(_contextFactory, org);
        }

        public IUnitOfWork CreateAddAreaResourceProcess(AreaResource areaResource)
        {
            return new AddAreaResource(_contextFactory, areaResource);
        }

        public IUnitOfWork CreateDeleteAreaResourceProcess(int resourceId)
        {
            return new DeleteAreaResource(_contextFactory, resourceId);
        }

        public IUnitOfWork CreateEditAreaResourceProcess(AreaResource resource)
        {
            return new EditAreaResource(_contextFactory, resource);
        }

        public IUnitOfWork CreateAddAreaTypeResourceProcess(AreaTypeResource resource)
        {
            return new AddAreaTypeResource(_contextFactory, resource);
        }

        public IUnitOfWork CreateDeleteAreaTypeResourceProcess(int resourceId)
        {
            return new DeleteAreaTypeResource(_contextFactory, resourceId);
        }

        public IUnitOfWork CreateEditAreaTypeResourceProcess(AreaTypeResource resource)
        {
            return new EditAreaTypeResource(_contextFactory, resource);
        }

        public IUnitOfWork CreateAddAreaCompositionProcess(int parentAreaId, int childAreaId)
        {
            return new AddAreaComposition(_contextFactory, parentAreaId, childAreaId);
        }

        public IUnitOfWork CreateDeleteAreaCompositionProcess(int parentAreaId, int childAreaId)
        {
            return new DeleteAreaComposition(_contextFactory, parentAreaId, childAreaId);
        }

        public IUnitOfWork CreateReplaceChildAreasForAreaAndTypeProcess(int areaId, string areaType, int[] childAreas)
        {
            return new ReplaceChildAreasForAreaAndType(_contextFactory, areaId, areaType, childAreas);
        }

        public IUnitOfWork CreateUpsertCalculatedGeometryProcess(IEnumerable<string> areaCodes, string areaCode, string typeCode, string childTypeCode)
        {
            return new UpsertCalculatedGeometry(_contextFactory, areaCode, typeCode, areaCodes, childTypeCode, this);
        }

        public IUnitOfWork CreateUpdateAreaTypeRelationshipProcess(TypeHierarchyBasic relationship)
        {
            return new UpdateAreaTypeRelationship(_contextFactory, relationship);
        }

        public IUnitOfWork CreateSetGeometryCalculationProcess(string typeCode, string areaCode, bool success)
        {
            return new SetGeometryCalculationResult(_contextFactory, typeCode, areaCode, success);
        }
    }
}
