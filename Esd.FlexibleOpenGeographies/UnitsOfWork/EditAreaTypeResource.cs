using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class EditAreaTypeResource : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaTypeResource _resource;

        public EditAreaTypeResource(IContextFactory contextFactory, AreaTypeResource resource)
        {
            _contextFactory = contextFactory;
            _resource = resource;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var resource = context.AreaTypeResources.SingleOrDefault(x => x.AreaTypeResourceId == _resource.AreaTypeResourceId);
                if (resource == null) return;
                resource.Label = _resource.Label;
                resource.Uri = _resource.Url;
                context.SaveChanges();
            }
        }
    }
}
