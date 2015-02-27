using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaIdForTypeAndCode : IQuerySingle<int>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _areaType;
        private readonly string _areaCode;

        public AreaIdForTypeAndCode(IContextFactory contextFactory, string areaType, string areaCode)
        {
            _contextFactory = contextFactory;
            _areaType = areaType;
            _areaCode = areaCode;
        }

        public int Find()
        {
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking()
                              .Where(x => x.TypeCode == _areaType && x.Code == _areaCode)
                              .Select(x => x.Id)
                              .SingleOrDefault();
        }
    }
}
