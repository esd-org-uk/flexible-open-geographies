using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class TypeCodesForAreaTypeGroup : IQueryEnumerable<string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;

        public TypeCodesForAreaTypeGroup(IContextFactory contextFactory, string typeCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
        }

        public IEnumerable<string> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                var isGroup = context.AreaTypes.AsNoTracking()
                                     .Where(x => x.Code == _typeCode)
                                     .Select(x => x.IsGroup)
                                     .SingleOrDefault() ?? false;
                return isGroup
                    ? context.AreaTypeGroupMembers.AsNoTracking()
                             .Where(x => x.TypeCode == _typeCode)
                             .Select(x => x.ChildTypeCode)
                             .ToList()
                    : new List<string> {_typeCode};
            }
        }
    }
}
