using System.Collections.Generic;
using System.Linq;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AllAreasWithNoKml : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;

        public AllAreasWithNoKml(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking()
                    .Where(area => area.ShapeDocument == null && area.TypeCode != "OutputArea")
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
