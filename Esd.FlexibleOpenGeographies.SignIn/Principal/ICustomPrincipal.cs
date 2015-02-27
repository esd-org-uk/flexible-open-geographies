using Esd.FlexibleOpenGeographies.Dtos;
using System.Security.Principal;

namespace Esd.FlexibleOpenGeographies.SignIn.Principal
{
    public interface ICustomPrincipal : IPrincipal
    {
        string UserId { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        string OrganisationId { get; set; }
        string OrganisationName { get; set; }
        UserBasic UserBasic { get; }
    }
}
