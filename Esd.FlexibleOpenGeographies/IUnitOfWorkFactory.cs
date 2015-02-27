using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;

namespace Esd.FlexibleOpenGeographies
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUpdateKmlProcess(int areaId, string kmlString);
        IUnitOfWork CreateAddAreaProcess(AreaFull area);
        IUnitOfWork CreateAddAreaTypeProcess(AreaTypeWithParentAndAlternateLabels areaType);
        IUnitOfWork CreateAddMetricProcess(MetricBasic metric);
        IUnitOfWork CreateRemoveMetricProcess(MetricBasic metric);
        IUnitOfWork CreateRemoveUploadProcess(UploadBasic upload);
        IUnitOfWork CreateAddUploadProcess(UploadBasic upload);
        IUnitOfWorkWithResult<string> CreateReserveCodeProcess(string shortCode);
        IUnitOfWorkWithResult<string> CreateReserveTypeCodeProcess();
        IUnitOfWork CreateUpdateAreaProcess(AreaFull area);
        IUnitOfWork CreateUpdateAreaTypeProcess(AreaTypeEditableDetails areaType);
        IUnitOfWork CreateUpsertGeometryProcess(string areaCode, string content, string typeCode);
        IUnitOfWork CreateUpsertCalculatedGeometryProcess(IEnumerable<string> areaCodes, string areaCode, string typeCode, string childTypeCode);
        IUnitOfWork CreateUpdateAreaTypeRelationshipProcess(TypeHierarchyBasic relationship);
        IUnitOfWork CreateSetGeometryCalculationProcess(string typeCode, string areaCode, bool success);
        IUnitOfWork CreateUpsertUser(UserBasic user);
        IUnitOfWork CreateUpsertOrganisation(OrganisationBasic org);
        IUnitOfWork CreateAddAreaResourceProcess(AreaResource areaResource);
        IUnitOfWork CreateDeleteAreaResourceProcess(int resourceId);
        IUnitOfWork CreateEditAreaResourceProcess(AreaResource resource);
        IUnitOfWork CreateAddAreaTypeResourceProcess(AreaTypeResource resource);
        IUnitOfWork CreateDeleteAreaTypeResourceProcess(int resourceId);
        IUnitOfWork CreateEditAreaTypeResourceProcess(AreaTypeResource resource);
        IUnitOfWork CreateAddAreaCompositionProcess(int parentAreaId, int childAreaId);
        IUnitOfWork CreateDeleteAreaCompositionProcess(int parentAreaId, int childAreaId);
        IUnitOfWork CreateReplaceChildAreasForAreaAndTypeProcess(int areaId, string areaType, int[] childAreas);
    }
}
