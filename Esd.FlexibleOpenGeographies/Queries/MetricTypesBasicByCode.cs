using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    public class MetricTypesBasicByCode : IQuerySingle<MetricTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private int _code;

        public MetricTypesBasicByCode(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForCode(string code)
        {
            _code = int.Parse(code);
        }

        public MetricTypeBasic Find()
        {
            using (var context = _contextFactory.Create())
            {
                var entity = context.MetricTypes.AsNoTracking().SingleOrDefault(mt => mt.Identifier == _code);
                return entity == null ? null : MetricTypeMapper.MapBasic(entity);
            }
        }
    }
}
