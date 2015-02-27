using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaDetailsById : IQuerySingle<AreaDetailsNoGeography>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public AreaDetailsById(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public AreaDetailsNoGeography Find()
        {
            using (var context = _contextFactory.Create())
            {
                var entity = context.AreaDetails.AsNoTracking().SingleOrDefault(area => area.Id == _id);
                return entity == null ? null : AreaMapper.MapDetails(entity);
            }
        }
    }
}
