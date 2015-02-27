using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class AddAreaType : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaTypeWithParentAndAlternateLabels _areaType;

        public AddAreaType(IContextFactory contextFactory, AreaTypeWithParentAndAlternateLabels areaType)
        {
            _contextFactory = contextFactory;
            _areaType = areaType;
        }

        public void Execute()
        {
            var newAreaType = AreaTypeMapper.Map(_areaType);
            var hierarchies = AreaTypeMapper.MapTypeHierarchies(_areaType);
            var groupMembers = AreaTypeMapper.MapGroupMembers(_areaType);
            var organisation = OrganisationMapper.Map(_areaType);
            var user = UserMapper.Map(_areaType);
            using (var context = _contextFactory.Create())
            {
                if (organisation != null && !context.Organisations.Any(x => x.OrganisationId == organisation.OrganisationId))
                    context.Organisations.Add(organisation);
                if (user != null && context.Users.SingleOrDefault(x => x.UserId == user.UserId) == null)
                    context.Users.Add(user);
                context.AreaTypes.Add(newAreaType);
                context.TypeHierarchies.AddRange(hierarchies);
                context.AreaTypeGroupMembers.AddRange(groupMembers);
                context.SaveChanges();
            }
        }
    }
}
