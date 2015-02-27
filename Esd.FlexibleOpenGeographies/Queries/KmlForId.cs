using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class KmlForId : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _id;

        public KmlForId(IContextFactory contextFactory, int id)
        {
            _contextFactory = contextFactory;
            _id = id;
        }

        public string Find()
        {
            using (var context = _contextFactory.Create())
            {
                var areaDetails = context.AreaDetails.AsNoTracking().SingleOrDefault(area => area.Id == _id);
                return areaDetails == null ? null : areaDetails.ShapeDocument;
            }
        }
    }
}
