using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpsertOrganisation : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly OrganisationBasic _organisation;

        public UpsertOrganisation(
            IContextFactory contextFactory,
            OrganisationBasic organisation)
        {
            _contextFactory = contextFactory;
            _organisation = organisation;
        }

        public void Execute()
        {
            var organisation = OrganisationMapper.MapOrganisation(_organisation);
            using (var context = _contextFactory.Create())
            {
                var entity = context.Organisations.SingleOrDefault(o => o.OrganisationId == _organisation.OrganisationId);
                
                if (entity != null)
                {
                    if (entity.OrganisationName == _organisation.OrganisationName) return;
                    entity.OrganisationName = _organisation.OrganisationName;
                }
                else
                {
                    context.Organisations.Add(organisation);
                }

                context.SaveChanges();
            }
        }
    }
}
