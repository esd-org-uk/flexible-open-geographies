using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class DeleteAreaTypeResource : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _resourceId;

        public DeleteAreaTypeResource(IContextFactory contextFactory, int resourceId)
        {
            _contextFactory = contextFactory;
            _resourceId = resourceId;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var resource = context.AreaTypeResources.SingleOrDefault(x => x.AreaTypeResourceId == _resourceId);
                if (resource == null) return;
                context.AreaTypeResources.Remove(resource);
                context.SaveChanges();
            }
        }
    }
}
