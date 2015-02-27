using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ColourForId : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public ColourForId(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public string Find()
        {
            using (var context = _contextFactory.Create())
            {
                var entity = context.AreaDetails.AsNoTracking().SingleOrDefault(x => x.Id == _id);
                return entity == null ? null : entity.Colour;
            }
        }
    }
}
