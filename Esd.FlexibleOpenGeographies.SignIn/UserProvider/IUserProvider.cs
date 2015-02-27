using Esd.FlexibleOpenGeographies.Dtos;
using System.Security.Principal;

namespace Esd.FlexibleOpenGeographies.SignIn.UserProvider
{
    public interface IUserProvider    
    {
        UserBasic CreateUser(IUnitOfWorkFactory unitOfWorkFactory, string accessToken);
        void SignOut(string returnUrl);
        bool AuthenticationCheck(IPrincipal principal);
    }
}
