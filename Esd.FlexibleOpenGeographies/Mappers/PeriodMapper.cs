using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public class PeriodMapper
    {
        public static PeriodBasic MapBasic(Period period)
        {
            return new PeriodBasic
            {
                Identifier = period.Identifier,
                Label = period.Label
            };
        }
    }
}
