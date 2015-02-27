using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaTypesForUser : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly UserBasic _user;

        public AreaTypesForUser(IContextFactory contextFactory, UserBasic user)
        {
            _contextFactory = contextFactory;
            _user = user;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            if (_user == null) return new List<AreaTypeBasic>();
            using (var context = _contextFactory.Create())
            {
                var userIds = _user.OrganisationId != null
                    ? context.Users.AsNoTracking()
                             .Where(x => x.OrganisationId == _user.OrganisationId)
                             .Select(x => x.UserId)
                             .ToList()
                    : new List<string> { _user.UserId };

                return context.AreaTypes.AsNoTracking()
                              .Where(x => userIds.Contains(x.CreatorId))
                              .Select(x => new AreaTypeBasic
                              {
                                  Code = x.Code,
                                  Label = x.Label
                              })
                              .OrderBy(x => x.Label)
                              .ToList();
            }
        }
    }
}
