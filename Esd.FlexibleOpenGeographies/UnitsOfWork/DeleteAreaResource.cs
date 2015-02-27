using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class DeleteAreaResource : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _resourceId;

        public DeleteAreaResource(IContextFactory contextFactory, int resourceId)
        {
            _contextFactory = contextFactory;
            _resourceId = resourceId;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var resource = context.AreaResources.SingleOrDefault(x => x.AreaResourceId == _resourceId);
                if (resource == null) return;
                context.AreaResources.Remove(resource);
                context.SaveChanges();
            }
        }
    }
}
