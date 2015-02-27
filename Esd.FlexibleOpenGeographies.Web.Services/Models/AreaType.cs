using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Web.Services.Models
{
    public class AreaType
    {
        public string Identifier { get; set; }
        public string Label { get; set; }

        public AreaType(AreaBasicWithType areaType)
        {
            Identifier = areaType.TypeCode;
            Label = areaType.TypeName;
        }

        public AreaType(AreaTypeDetails areaType)
        {
            Identifier = areaType.Code;
            Label = areaType.Label;
        }

        public AreaType(AreaTypeBasic areaType)
        {
            Identifier = areaType.Code;
            Label = areaType.Label;
        }        
    }
}