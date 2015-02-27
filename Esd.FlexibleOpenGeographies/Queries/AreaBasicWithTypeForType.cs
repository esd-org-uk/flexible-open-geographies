using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaBasicWithTypeForType : IQueryEnumerable<AreaBasicWithType>
    {
        private readonly IContextFactory _contextFactory;
        private string _typeCode;

        public AreaBasicWithTypeForType(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void ForType(string typeCode)
        {
            _typeCode = typeCode;
        }

        public IEnumerable<AreaBasicWithType> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                return context.AreaDetails.AsNoTracking()
                              .Where(a => a.TypeCode == _typeCode)
                              .OrderBy(a => a.Label)
                              .Select(area => new AreaBasicWithType
                              {
                                  Id = area.Id,
                                  Code = area.Code,
                                  Label = area.Label,
                                  TypeCode = area.AreaType.Code,
                                  TypeName = area.AreaType.Label
                              }).ToList();
            }
        }
    }
}
