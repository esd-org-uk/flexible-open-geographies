using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class BoundingBoxForAreas : IQuerySingle<BoundingBox>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IEnumerable<int> _areaIds;

        public BoundingBoxForAreas(IContextFactory contextFactory, IEnumerable<int> areaIds)
        {
            _contextFactory = contextFactory;
            _areaIds = areaIds;
        }

        public BoundingBox Find()
        {
            if (_areaIds == null || !_areaIds.Any()) return null;
            var bounds = _areaIds.Select(x => new AreaBasicWithTypeForId(_contextFactory, x).Find())
                                 .Where(x => x != null)
                                 .Select(x => new BoundingBoxForArea(_contextFactory, x.TypeCode, x.Code).Find())
                                 .Where(x => x != null)
                                 .ToList();
            return new BoundingBox
            {
                MaximumX = bounds.Max(x => x.MaximumX),
                MaximumY = bounds.Max(x => x.MaximumY),
                MinimumX = bounds.Min(x => x.MinimumX),
                MinimumY = bounds.Min(x => x.MinimumY)
            };
        }
    }
}
