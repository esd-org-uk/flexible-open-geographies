using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class ResourcesForArea : IQueryEnumerable<AreaResource>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _areaId;

        public ResourcesForArea(IContextFactory contextFactory, int areaId)
        {
            _contextFactory = contextFactory;
            _areaId = areaId;
        }

        public IEnumerable<AreaResource> Fetch()
        {
            using (var context = _contextFactory.Create())
                return context.AreaResources.AsNoTracking()
                              .Where(x => x.AreaId == _areaId)
                              .Select(x => new AreaResource
                              {
                                  AreaResourceId = x.AreaResourceId,
                                  AreaId = x.AreaId,
                                  Label = x.Label,
                                  Url = x.Uri,
                                  
                              })
                              .ToList();
        }
    }
}
