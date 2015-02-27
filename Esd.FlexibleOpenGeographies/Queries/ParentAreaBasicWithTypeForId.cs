using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ParentAreaBasicWithTypeForId : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public ParentAreaBasicWithTypeForId(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            using (var context = _contextFactory.Create())
                return ParentAreas(context);
        }

        private IEnumerable<AreaBasicWithType> ParentAreas(IFogContext context)
        {
            return context.AreaCompositions.AsNoTracking().Include(x => x.Area.AreaType)
                          .Where(x => x.ChildAreaId == _id)
                          .Select(x => new AreaBasicWithType
                          {
                              TypeCode = x.Area.TypeCode,
                              TypeName = x.Area.AreaType.Label,
                              Id = x.AreaId,
                              Code = x.Area.Code,
                              Label = x.Area.Label
                          })
                          .ToList();
        }
    }
}
