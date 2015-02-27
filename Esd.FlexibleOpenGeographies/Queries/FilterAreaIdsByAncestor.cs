using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class FilterAreaIdsByAncestor : IQueryEnumerable<int>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IEnumerable<int> _areas;
        private readonly string _typeCode;
        private readonly string _boundingType;
        private readonly string _boundingArea;

        public FilterAreaIdsByAncestor(IContextFactory contextFactory, IEnumerable<int> areas, string typeCode, string boundingType, string boundingArea)
        {
            _contextFactory = contextFactory;
            _areas = areas;
            _typeCode = typeCode;
            _boundingType = boundingType;
            _boundingArea = boundingArea;
        }

        public IEnumerable<int> Fetch()
        {
            var areaId = new AreaIdForTypeAndCode(_contextFactory, _boundingType, _boundingArea).Find();
            if (areaId == 0) return _areas; //No ancestor to filter on
            var validAreas = new AreaIdsForTypeAndAncestor(_contextFactory, areaId, _typeCode).Fetch();
            return _areas.Intersect(validAreas);
        }
    }
}
