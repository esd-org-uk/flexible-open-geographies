using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ChildAreaCodesForAreasAndAreaType : IQueryEnumerable<int>
    {
        private readonly IContextFactory _contextFactory;
        private List<int> _ids;
        private string _type;

        public ChildAreaCodesForAreasAndAreaType(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForIds(List<int> ids)
        {
            _ids = ids;
        }

        public void ForType(string type)
        {
            _type = type;
        }

        public IEnumerable<int> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaCompositions.Include(composition => composition.ChildArea.AreaType).AsNoTracking()
                              .Where(composition => _ids.Contains(composition.Area.Id) && composition.ChildArea.AreaType.Code == _type)
                              .Select(area => area.ChildArea.Id)
                              .ToList();
        }
    }
}
