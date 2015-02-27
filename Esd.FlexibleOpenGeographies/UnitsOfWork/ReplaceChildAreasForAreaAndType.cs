using Esd.FlexibleOpenGeographies.Data;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class ReplaceChildAreasForAreaAndType : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _areaId;
        private readonly string _areaType;
        private readonly int[] _childAreas;

        public ReplaceChildAreasForAreaAndType(IContextFactory contextFactory, int areaId, string areaType, int[] childAreas)
        {
            _contextFactory = contextFactory;
            _areaId = areaId;
            _areaType = areaType;
            _childAreas = childAreas ?? new int[]{};
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var toRemove = context.AreaCompositions
                                      .Where(x => x.AreaId == _areaId
                                                  && x.ChildArea.TypeCode == _areaType
                                                  && !_childAreas.Contains(x.ChildAreaId));
                if (toRemove.Any()) context.AreaCompositions.RemoveRange(toRemove);
// ReSharper disable once AccessToDisposedClosure
                var newRecords = _childAreas.Where(x => context.AreaCompositions.AsNoTracking()
                                                               .SingleOrDefault(x1 => x1.AreaId == _areaId &&
                                                                                      x1.ChildAreaId == x) == null)
                                            .Select(x => new AreaComposition
                                            {
                                                AreaId = _areaId,
                                                ChildAreaId = x
                                            });
                context.AreaCompositions.AddRange(newRecords);
                context.SaveChanges();
            }
        }
    }
}
