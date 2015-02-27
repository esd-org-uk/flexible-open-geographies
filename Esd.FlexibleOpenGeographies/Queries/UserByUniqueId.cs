using Esd.FlexibleOpenGeographies.Data;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class UserByUniqueId : IQuerySingle<User>
    {
        private readonly IContextFactory _contextFactory;
        private string _code;

        public UserByUniqueId(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForCode(string code)
        {
            _code = code;
        }

        public User Find()
        {
            using (var context = _contextFactory.Create())
            {
                return context.Users.AsNoTracking().SingleOrDefault(u => u.UserId == _code);
            }
        }
    }
}
