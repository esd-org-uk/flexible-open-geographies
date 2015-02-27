using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    public class MetricTypesBasicByTerm : IQueryEnumerable<MetricTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private string _term;

        public MetricTypesBasicByTerm(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForCode(string term)
        {
            _term = term;
        }

        public IEnumerable<MetricTypeBasic> Fetch()
        {
            int id = -1;
            string term = string.Empty;            

            if (!string.IsNullOrEmpty(_term))
            {
                int.TryParse(_term, out id);
                term = _term.ToLower();
            }

            using (var context = _contextFactory.Create())
            {
                return context.MetricTypes.AsNoTracking().
                    Where(mt => mt.Identifier == id || mt.Label.ToLower().Contains(term))
                    .OrderBy(type => type.Identifier)
                              .Select(MetricTypeMapper.MapBasic)
                              .ToList();
            }
        }
    }
}
