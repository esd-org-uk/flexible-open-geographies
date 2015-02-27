using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaTypeCodeForLabel : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _label;

        public AreaTypeCodeForLabel(IContextFactory contextFactory, string label)
        {
            _contextFactory = contextFactory;
            _label = label;
        }

        public string Find()
        {
            using (var context = _contextFactory.Create())
            {
                var areaType = context.AreaTypes.AsNoTracking().SingleOrDefault(x => x.Label == _label);
                return areaType == null ? null : areaType.Code;
            }
        }
    }
}
