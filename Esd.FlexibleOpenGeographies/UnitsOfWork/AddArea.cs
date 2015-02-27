using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class AddArea : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaFull _area;
        private readonly IUnitOfWork _addGeometry;

        public AddArea(IContextFactory contextFactory, AreaFull area, IUnitOfWork addGeometry)
        {
            _contextFactory = contextFactory;
            _area = area;
            _addGeometry = addGeometry;
        }

        public void Execute()
        {
            var newArea = AreaMapper.Map(_area);
            var organisation = OrganisationMapper.Map(_area);
            var user = UserMapper.Map(_area);
            using (var context = _contextFactory.Create())
            {
                if (organisation != null && !context.Organisations.Any(x => x.OrganisationId == organisation.OrganisationId))
                    context.Organisations.Add(organisation);
                if (user != null && !context.Users.Any(x => x.UserId == user.UserId))
                    context.Users.Add(user);
                context.AreaDetails.Add(newArea);
                context.SaveChanges();
            }
            if (!string.IsNullOrWhiteSpace(_area.GeometryString)) _addGeometry.Execute();
        }
    }
}
