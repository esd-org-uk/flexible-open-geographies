using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaBasicWithTypeForId : IQuerySingle<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public AreaBasicWithTypeForId(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public AreaBasicWithType Find()
        {
            using (var context = _contextFactory.Create())
            {
                return context.AreaDetails.AsNoTracking()
                              .Where(a => a.Id == _id)
                              .Select(area => new AreaBasicWithType
                              {
                                  Id = area.Id,
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              }).SingleOrDefault();
            }
        }
    }
}
