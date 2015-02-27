using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreasForUser : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private readonly UserBasic _user;

        public AreasForUser(IContextFactory contextFactory, UserBasic user)
        {
            _contextFactory = contextFactory;
            _user = user;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            if (_user == null) return new List<AreaBasicWithType>();
            using (var context = _contextFactory.Create())
            {
                var userIds = _user.OrganisationId != null
                    ? context.Users.AsNoTracking()
                             .Where(x => x.OrganisationId == _user.OrganisationId)
                             .Select(x => x.UserId)
                             .ToList()
                    : new List<string> {_user.UserId};

                return context.AreaDetails.AsNoTracking().Include(x => x.AreaType)
                              .Where(x => userIds.Contains(x.CreatorId))
                              .Select(x => new AreaBasicWithType
                              {
                                  Id = x.Id,
                                  Code = x.Code,
                                  Label = x.Label,
                                  TypeCode = x.TypeCode,
                                  TypeName = x.AreaType.Label
                              })
                              .OrderBy(x => x.Label)
                              .ToList();
            }
        }
    }
}
