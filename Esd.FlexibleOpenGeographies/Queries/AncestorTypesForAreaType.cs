using Esd.FlexibleOpenGeographies.Comparers;
using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AncestorTypesForAreaType : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly AreaTypeBasicCodeComparer _comparer = new AreaTypeBasicCodeComparer();

        public AncestorTypesForAreaType(IContextFactory contextFactory, string typeCode)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
                return FindParents(context, _typeCode);
        }

        private IEnumerable<AreaTypeBasic> FindParents(IFogContext context, string typeCode)
        {
            var results = new List<AreaTypeBasic>();
            var areaTypes = context.TypeHierarchies.AsNoTracking()
                                   .Where(x => x.ChildTypeCode == typeCode)
                                   .Select(x => new AreaTypeBasic
                                   {
                                       Code = x.TypeCode,
                                       Label = x.AreaType.Label
                                   })
                                   .ToList();
            foreach (var areaType in areaTypes)
            {
                results.Add(areaType);
                results.AddRange(FindParents(context, areaType.Code)
                    .Distinct(_comparer)
                    .Where(x => !results.Contains(x, _comparer)));
            }
            return results.Distinct(_comparer);
        }
    }
}
