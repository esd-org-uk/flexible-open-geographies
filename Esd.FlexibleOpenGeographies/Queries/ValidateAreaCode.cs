using System.Linq;
using System.Web;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ValidateAreaCode : IQuerySingle<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _code;

        public ValidateAreaCode(IContextFactory contextFactory, string code)
        {
            _contextFactory = contextFactory;
            _code = code;
        }

        public string Find()
        {
            if (string.IsNullOrWhiteSpace(_code)) return null;
            if (_code != HttpUtility.HtmlEncode(_code) 
                || _code != HttpUtility.HtmlDecode(_code)
                || _code != HttpUtility.UrlEncode(_code)
                || _code != HttpUtility.UrlDecode(_code))
                return "Code cannot contain special characters";
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking().Any(x => x.Code == _code)
                    ? "Code already exists"
                    : null;
        }
    }
}
