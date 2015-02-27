using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public static class UserMapper
    {
        public static User Map(IHasOwner user)
        {
            return (user.CurrentUser == null || user.CurrentUser.UserId == null)
                ? null
                : new User
                {
                    UserId = user.CurrentUser.UserId,
                    Name = user.CurrentUser.Name,
                    Email = user.CurrentUser.Email,
                    OrganisationId = user.CurrentUser.OrganisationId
                };
        }

        public static UserBasic MapBasic(User user)
        {
            var basic = new UserBasic
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                OrganisationId = user.OrganisationId,
                AccessToken = user.AccessToken
            };
            
            if (user.Organisation != null)
            {
                basic.OrganisationName = user.OrganisationId;
            }

            return basic;
        } 

        public static User MapUser(UserBasic user)
        {
            return new User
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                OrganisationId = user.OrganisationId,
                AccessToken = user.AccessToken
            };
        }
    }
}
