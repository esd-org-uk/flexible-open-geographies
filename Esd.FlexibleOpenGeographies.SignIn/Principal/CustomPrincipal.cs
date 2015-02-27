using Esd.FlexibleOpenGeographies.Dtos;
using System.Security.Principal;

namespace Esd.FlexibleOpenGeographies.SignIn.Principal
{
    public class CustomPrincipal : ICustomPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public CustomPrincipal(string email)
        {
            Identity = new GenericIdentity(email);
        }

        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string OrganisationId { get; set; }
        public string OrganisationName { get; set; }

        public UserBasic UserBasic
        {
            get
            {
                return new UserBasic
                {
                    UserId = UserId,
                    Name = Name,
                    Email = Email,
                    OrganisationId = OrganisationId,
                    OrganisationName = OrganisationName
                };
            }
        }
    }
}
