using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class EditAreaResource :IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaResource _resource;

        public EditAreaResource(IContextFactory contextFactory, AreaResource resource)
        {
            _contextFactory = contextFactory;
            _resource = resource;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var resource = context.AreaResources.SingleOrDefault(x => x.AreaResourceId == _resource.AreaResourceId);
                if (resource == null) return;
                resource.Label = _resource.Label;
                resource.Uri = _resource.Url;
                context.SaveChanges();
            }
        }
    }
}
