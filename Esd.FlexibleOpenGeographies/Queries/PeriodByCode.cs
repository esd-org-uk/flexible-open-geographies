using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class PeriodByCode : IQuerySingle<PeriodBasic>
    {
        private readonly IContextFactory _contextFactory;
        private string _code;

        public PeriodByCode(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForCode(string code)
        {
            _code = code;
        }

        public PeriodBasic Find()
        {
            using (var context = _contextFactory.Create())
            {
                var entity = context.Periods.AsNoTracking().SingleOrDefault(p => p.Identifier == _code);
                return entity == null ? null : PeriodMapper.MapBasic(entity);
            }
        }
    }
}
