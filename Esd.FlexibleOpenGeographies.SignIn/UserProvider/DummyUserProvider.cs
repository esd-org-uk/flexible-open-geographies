using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Security.Principal;

namespace Esd.FlexibleOpenGeographies.SignIn.UserProvider
{
    public class DummyUserProvider : IUserProvider
    {
        public UserBasic CreateUser(IUnitOfWorkFactory unitOfWorkFactory, string accessToken)
        {
            return new UserBasic
            {
                UserId = Guid.Empty.ToString(),
                Email = "dummy@dummy.com", 
                Name = "Dummy Dummy", 
                OrganisationId = Guid.Empty.ToString(), 
                OrganisationName = "Dummy Organisation"
            };
        }

        public void SignOut(string returnUrl) { }

        public bool AuthenticationCheck(IPrincipal principal)
        {
            return false;
        }
    }
}
