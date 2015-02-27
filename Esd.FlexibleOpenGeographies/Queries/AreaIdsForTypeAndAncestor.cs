using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaIdsForTypeAndAncestor : IQueryEnumerable<int>
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _ancestorId;
        private readonly string _typeCode;

        public AreaIdsForTypeAndAncestor(IContextFactory contextFactory, int ancestorId, string typeCode)
        {
            _contextFactory = contextFactory;
            _ancestorId = ancestorId;
            _typeCode = typeCode;
        }

        public IEnumerable<int> Fetch()
        {
            var all = new HashSet<int>();
            var types = new AncestorTypesForAreaType(_contextFactory, _typeCode).Fetch().Select(x => x.Code).ToList();
            if (!types.Contains(_typeCode)) types.Add(_typeCode);
            using (var context = _contextFactory.Create())
                return FindChildren(context, _ancestorId, ref all, types);
        }

        private IEnumerable<int> FindChildren(IFogContext context, int areaId, ref HashSet<int> all, ICollection<string> types)
        {
            var results = new HashSet<int>();
            var areas = context.AreaCompositions.AsNoTracking()
                               .Where(x => x.AreaId == areaId && types.Contains(x.ChildArea.TypeCode))
                               .Select(x => new AreaBasicWithType
                               {
                                   Id = x.ChildAreaId,
                                   TypeCode = x.ChildArea.TypeCode
                               })
                               .ToList();
            foreach (var area in areas)
            {
                if (!all.Add(area.Id)) continue;
                if (area.TypeCode == _typeCode)
                    results.Add(area.Id);
                else
                    FindChildren(context, area.Id, ref all, types).ToList().ForEach(x => results.Add(x));
            }
            return results;
        }
    }
}
