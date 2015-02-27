using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ChildAreasForArea : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public ChildAreasForArea(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaCompositions.Include(composition => composition.ChildArea.AreaType).AsNoTracking()
                              .Where(composition => composition.AreaId == _id)
                              .Select(composition => composition.ChildArea)
                              //Can't get this working with mapper
                              .Select(area => new AreaBasicWithType
                              {
                                  Id = area.Id,
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              })
                              .ToList();
        }
    }
}
