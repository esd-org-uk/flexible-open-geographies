using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpsertUser : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly UserBasic _user;

        public UpsertUser(IContextFactory contextFactory, UserBasic user)
        {
            _contextFactory = contextFactory;
            _user = user;
        }

        public void Execute()
        {
            var user = UserMapper.MapUser(_user);
            using (var context = _contextFactory.Create())
            {
                var entity = context.Users.SingleOrDefault(u => u.UserId == _user.UserId);
                if (entity != null)
                {
                    if (ValuesAreEqual(entity, user)) return;
                    entity.AccessToken = user.AccessToken;
                    entity.Email = user.Email;
                    entity.Name = user.Name;
                    entity.OrganisationId = user.OrganisationId;
                }
                else
                {
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }

        private static bool ValuesAreEqual(User u1, User u2)
        {
            return u1.AccessToken == u2.AccessToken
                && u1.Email == u2.Email
                && u1.Name == u2.Name
                && u1.OrganisationId == u2.OrganisationId;
        }
    }
}
