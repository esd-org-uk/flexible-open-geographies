using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class AddAreaTypeResource : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaTypeResource _resource;

        public AddAreaTypeResource(IContextFactory contextFactory, AreaTypeResource resource)
        {
            _contextFactory = contextFactory;
            _resource = resource;
        }

        public void Execute()
        {
            var resource = new Data.AreaTypeResource
            {
                TypeCode = _resource.TypeCode,
                Label = _resource.Label,
                Uri = _resource.Url
            };
            using (var context = _contextFactory.Create())
            {
                context.AreaTypeResources.Add(resource);
                context.SaveChanges();
            }
        }
    }
}
