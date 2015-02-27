using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaBasicForType : IQueryEnumerable<AreaBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly int _limit;

        public AreaBasicForType(IContextFactory contextFactory, string typeCode, int limit)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _limit = limit;
        }

        public IEnumerable<AreaBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaDetails.AsNoTracking()
                              .Where(area => area.TypeCode == _typeCode)
                              .OrderBy(area => area.Label)
                              .Take(_limit)
                              .Select(AreaMapper.MapBasic)
                              .ToList();
        }
    }
}
