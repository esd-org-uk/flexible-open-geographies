using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreasForCalculatedGeometry : IQueryEnumerable<AreaForCalcuatedGeometry>
    {
        private readonly IContextFactory _contextFactory;

        public AreasForCalculatedGeometry(IContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public IEnumerable<AreaForCalcuatedGeometry> Fetch()
        {
            var results = new List<AreaForCalcuatedGeometry>();
            using (var context = _contextFactory.Create())
            {
                var areas = context.AreaDetails.AsNoTracking()
                                   .Where(x => (x.RequiresGeometryCalculation ?? false)
                                               && !(x.GeometryCalculationFailed ?? false)
                                               && string.IsNullOrEmpty(x.ShapeDocument))
                                   .ToList();
                foreach (var area in areas)
                {
                    var primaryType = FindPrimaryType(area);
                    if (primaryType == null) continue;
                    var result = new AreaForCalcuatedGeometry
                    {
                        AreaCode = area.Code,
                        TypeCode = area.TypeCode,
                        ChildAreaCodes = area.AreaCompositions
                                             .Where(x => x.ChildArea.TypeCode == primaryType)
                                             .Select(x => x.ChildArea.Code)
                                             .ToList(),
                        ChildTypeCode = primaryType
                    };
                    if (result.ChildAreaCodes.Any()) results.Add(result);
                }
                return results;
            }
        }

        private string FindPrimaryType(AreaDetail area)
        {
            using (var context = _contextFactory.Create())
            {
                var hierarchy = context.TypeHierarchies.AsNoTracking()
                                       .FirstOrDefault(x => x.TypeCode == area.TypeCode && x.IsPrimary);
                return hierarchy == null ? null : hierarchy.ChildTypeCode;
            }
        }
    }
}
