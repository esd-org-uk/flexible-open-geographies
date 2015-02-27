using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class SetGeometryCalculationResult : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _typeCode;
        private readonly string _areaCode;
        private readonly bool _success;

        public SetGeometryCalculationResult(IContextFactory contextFactory, string typeCode, string areaCode, bool success)
        {
            _contextFactory = contextFactory;
            _typeCode = typeCode;
            _areaCode = areaCode;
            _success = success;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var area = context.AreaDetails.SingleOrDefault(x => x.TypeCode == _typeCode && x.Code == _areaCode);
                if (area == null) return;
                area.GeometryCalculationFailed = !_success;
                area.RequiresGeometryCalculation = !_success;
                if (_success)
                {
                    var parentAreas = context.AreaCompositions
                                             .Include(x => x.Area)
                                             .Where(x => x.ChildAreaId == area.Id)
                                             .Select(x => x.Area);
                    foreach (var parentArea in parentAreas)
                    {
                        parentArea.RequiresGeometryCalculation = true;
                        parentArea.GeometryCalculationFailed = false;
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
