using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class AreaTypeContainer
    {
        public AreaType AreaType;

        public AreaTypeContainer(AreaTypeDetails areaType)
        {
            AreaType = new AreaType(areaType);
        }
    }
}