using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class MetricUploadPermissionLevelDescriptionById : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public MetricUploadPermissionLevelDescriptionById(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public string Find()
        {
            using (var context = _contextFactory.Create())
                return context.MetricUploadPermissionLevels.AsNoTracking()
                              .Single(x => x.Id == _id)
                              .Description;
        }
    }
}
