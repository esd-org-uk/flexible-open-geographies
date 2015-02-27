using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class FilteredAreaBasicForType : IQueryEnumerable<AreaBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly string _filter;

        public FilteredAreaBasicForType(IContextFactory contextFactory, string typeCode, string filter)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _filter = filter;
        }

        public IEnumerable<AreaBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                var type = context.AreaTypes.AsNoTracking().SingleOrDefault(x => x.Code == _typeCode);
                var isGroup = type != null && (type.IsGroup ?? false);
                var types = isGroup
                    ? context.AreaTypeGroupMembers.Where(x => x.TypeCode == _typeCode)
                             .Select(x => x.ChildTypeCode)
                             .ToList()
                    : new List<string> {_typeCode};
                var areas = context.AreaDetails.AsNoTracking().Where(x => types.Contains(x.TypeCode));
                if (!string.IsNullOrWhiteSpace(_filter))
                    areas = areas.Where(x => x.Label.Contains(_filter) || x.Code.Contains(_filter));
                return areas.OrderBy(x => x.Label)
                            .Take(1000)
                            .Select(AreaMapper.MapBasic)
                            .ToList();
            }
        }
    }
}
