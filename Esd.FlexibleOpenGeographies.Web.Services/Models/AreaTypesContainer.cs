using Esd.FlexibleOpenGeographies.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class AreaTypesContainer
    {
        [JsonProperty("areaType-array")]
        public AreaType[] AreaTypes {get;set;}

        public AreaTypesContainer(IEnumerable<AreaTypeBasic> areaTypes)
        {
            AreaTypes = areaTypes.Select(areaType => new AreaType(areaType)).ToArray();
        }
    }
}