using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ChildAreaIdsForAreaAndAreaType : IQueryEnumerable<int>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;
        private readonly string _areaType;

        public ChildAreaIdsForAreaAndAreaType(IContextFactory contextFactory, int id, string areaType)
        {
            _contextFactory = contextFactory;
            _id = id;
            _areaType = areaType;
        }

        public IEnumerable<int> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaCompositions.Include(x => x.ChildArea.AreaType).AsNoTracking()
                              .Where(x => x.Area.Id == _id && x.ChildArea.AreaType.Code == _areaType)
                              .Select(x => x.ChildArea.Id)
                              .ToList();
        }
    }
}
