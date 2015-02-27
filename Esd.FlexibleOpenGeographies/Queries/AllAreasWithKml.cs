using System.Collections.Generic;
using System.Linq;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AllAreasWithKml : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;

        public AllAreasWithKml(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking()
                              .Where(area => area.ShapeDocument != null)
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
