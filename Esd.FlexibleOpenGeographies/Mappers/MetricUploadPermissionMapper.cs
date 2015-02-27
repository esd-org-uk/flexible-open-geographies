using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public static class MetricUploadPermissionMapper
    {
        public static AreaTypeMetricUploadPermissionLevel Map(MetricUploadPermissionLevel permission)
        {
            return new AreaTypeMetricUploadPermissionLevel
            {
                Id = permission.Id,
                Description = permission.Description
            };
        }
    }
}
