using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class AreaContainer
    {
        public Area Area;

        public AreaContainer(AreaBasicWithType area)
        {
            Area = new Area(area);
        }
    }
}