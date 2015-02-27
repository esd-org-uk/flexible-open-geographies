using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    public class MetricAggregationByAreaTypeAndMetricType : IQuerySingle<MetricAggregationBasic>
    {
        private readonly IContextFactory _contextFactory;
        private string _typeCode;
        private int _code;

        public MetricAggregationByAreaTypeAndMetricType(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForCode(string code)
        {
            _code = int.Parse(code);
        }

        public void ForType(string typeCode)
        {
            _typeCode = typeCode;
        }

        public MetricAggregationBasic Find()
        {
            using (var context = _contextFactory.Create())
            {
                var entity = context.MetricAggregations.AsNoTracking().SingleOrDefault(ma => ma.MetricTypeIdentifier == _code && ma.TypeCode == _typeCode);
                return entity == null ? null : MetricAggregationMapper.Map(entity);
            }
        }
    }
}
