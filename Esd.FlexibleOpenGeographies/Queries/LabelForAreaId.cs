using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class LabelForAreaId : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public LabelForAreaId(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public string Find()
        {
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking()
                              .Where(x => x.Id == _id)
                              .Select(x => x.Label + " (" + x.Code + ")")
                              .SingleOrDefault();
        }
    }
}
