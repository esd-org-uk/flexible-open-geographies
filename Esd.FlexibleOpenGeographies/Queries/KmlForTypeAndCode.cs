using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class KmlForTypeAndCode : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly string _code;

        public KmlForTypeAndCode(IContextFactory contextFactory, string typeCode, string code)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _code = code;
        }

        public string Find()
        {
            using (var context = _contextFactory.Create())
            {
                var areaDetails = context.AreaDetails.AsNoTracking().SingleOrDefault(area => area.TypeCode == _typeCode && area.Code == _code);
                return areaDetails == null ? null : areaDetails.ShapeDocument;
            }
        }
    }
}
