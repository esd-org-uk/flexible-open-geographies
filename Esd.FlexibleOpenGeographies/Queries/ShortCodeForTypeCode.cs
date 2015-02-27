using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ShortCodeForTypeCode : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private string _typeCode;

        public ShortCodeForTypeCode(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForType(string typeCode)
        {
            _typeCode = typeCode;
        }

        public string Find()
        {
            using (var context = _contextFactory.Create())
                return context.AreaTypes.AsNoTracking()
                              .Single(type => type.Code == _typeCode)
                              .ShortCode;
        }
    }
}
