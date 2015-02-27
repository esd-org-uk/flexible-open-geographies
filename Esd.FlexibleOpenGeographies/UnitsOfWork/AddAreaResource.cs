using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class AddAreaResource : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaResource _areaResource;

        public AddAreaResource(IContextFactory contextFactory, AreaResource areaResource)
        {
            _contextFactory = contextFactory;
            _areaResource = areaResource;
        }

        public void Execute()
        {
            var resource = new Data.AreaResource
            {
                AreaId = _areaResource.AreaId,
                Label = _areaResource.Label,
                Uri = _areaResource.Url
            };
            using (var context = _contextFactory.Create())
            {
                context.AreaResources.Add(resource);
                context.SaveChanges();
            }
        }
    }
}
