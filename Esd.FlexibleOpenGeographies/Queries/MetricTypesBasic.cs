using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class MetricTypesBasic : IQueryEnumerable<MetricTypeBasic>
    {
        private readonly IContextFactory _contextFactory;

        public MetricTypesBasic(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<MetricTypeBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.MetricTypes.AsNoTracking()
                              .OrderBy(type => type.Identifier)
                              .Select(MetricTypeMapper.MapBasic)
                              .ToList();
        }
    }
}
