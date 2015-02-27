using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Mappers
{
    public static class OrganisationMapper
    {
        public static Organisation Map(IHasOwner user)
        {
            return user.CurrentUser == null || user.CurrentUser.OrganisationId == null
                ? null
                : new Organisation
                {
                    OrganisationId = user.CurrentUser.OrganisationId,
                    OrganisationName = user.CurrentUser.OrganisationName
                };
        }

        public static OrganisationBasic MapBasic(Organisation org)
        {
            return new OrganisationBasic
            {
                OrganisationId = org.OrganisationId,
                OrganisationName = org.OrganisationName
            };
        }

        public static Organisation MapOrganisation(OrganisationBasic org)
        {
            return new Organisation
            {
                OrganisationId = org.OrganisationId,
                OrganisationName = org.OrganisationName
            };
        }
    }
}
