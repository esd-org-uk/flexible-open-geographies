using Esd.FlexibleOpenGeographies.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class AreasContainer
    {
        [JsonProperty("area-array")]
        public Area[] Areas { get; set; }
        public int pageNumber;
        public int totalPages;

        public AreasContainer(IEnumerable<AreaBasicWithType> areas)
        {
            Areas = areas.Select(area => new Area(area)).ToArray();
        }

        public AreasContainer(IEnumerable<AreaBasicWithType> areas, int pageNumber, int totalPages) : this(areas)
        {
            this.pageNumber = pageNumber;
            this.totalPages = totalPages;

            Areas = areas.Select(area => new Area(area)).ToArray();
        }
    }
}