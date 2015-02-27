using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ParentAreasForArea : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public ParentAreasForArea(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaCompositions.Include(composition => composition.Area.AreaType).AsNoTracking()
                              .Where(composition => composition.ChildAreaId == _id)
                              .Select(x => new AreaBasicWithType
                              {
                                  Id = x.Area.Id,
                                  Code = x.Area.Code,
                                  Label = x.Area.Label,
                                  TypeCode = x.Area.AreaType.Code,
                                  TypeName = x.Area.AreaType.Label
                              })
                              .ToList();
        }
    }
}
