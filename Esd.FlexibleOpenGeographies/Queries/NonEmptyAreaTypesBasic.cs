using Esd.FlexibleOpenGeographies.Comparers;
using Esd.FlexibleOpenGeographies.Dtos;
using Esd.FlexibleOpenGeographies.Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class NonEmptyAreaTypesBasic : IQueryEnumerable<AreaTypeBasic>
    {
        private readonly IContextFactory _contextFactory;
        private readonly bool _includeGroups;
        private readonly AreaTypeBasicCodeComparer _comparer = new AreaTypeBasicCodeComparer();

        public NonEmptyAreaTypesBasic(IContextFactory contextFactory, bool includeGroups)
        {
            _contextFactory = contextFactory;
            _includeGroups = includeGroups;
        }

        public IEnumerable<AreaTypeBasic> Fetch()
        {
            using (var context = _contextFactory.Create())
            {
                var areaTypes = context.AreaTypes.AsNoTracking()
                                       .Where(x => x.Areas.Any())
                                       .Select(AreaTypeMapper.MapBasic)
                                       .ToList();
                if (_includeGroups)
                {
                    var typeCodes = areaTypes.Select(x => x.Code).ToList();
                    areaTypes.AddRange(context.AreaTypeGroupMembers.AsNoTracking()
                        .Where(x => typeCodes.Contains(x.ChildTypeCode))
                        .Select(x => new AreaTypeBasic
                        {
                            Code = x.TypeCode,
                            Label = x.AreaType.Label
                        })
                        .ToList());
                }

                return areaTypes.Distinct(_comparer).OrderBy(x => x.Label);
            }
        }
    }
}
