using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ResourcesForAreaType : IQueryEnumerable<AreaTypeResource>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;

        public ResourcesForAreaType(IContextFactory contextFactory, string typeCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
        }

        public IEnumerable<AreaTypeResource> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaTypeResources.AsNoTracking()
                              .Where(x => x.TypeCode == _typeCode)
                              .Select(x => new AreaTypeResource
                              {
                                  AreaTypeResourceId = x.AreaTypeResourceId,
                                  TypeCode = x.TypeCode,
                                  Label = x.Label,
                                  Url = x.Uri,

                              })
                              .ToList();
        }
    }
}
