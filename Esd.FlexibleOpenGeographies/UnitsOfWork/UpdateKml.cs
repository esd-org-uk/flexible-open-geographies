using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpdateKml : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _areaId;
        private readonly string _kmlString;

        public UpdateKml(IContextFactory contextFactory, int areaId, string kmlString)
        {
            _contextFactory = contextFactory;
            _areaId = areaId;
            _kmlString = kmlString;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var areaDetail = context.AreaDetails.SingleOrDefault(area => area.Id == _areaId);
                if (areaDetail == null) return;
                areaDetail.ShapeDocument = _kmlString;
                context.SaveChanges();
            }
        }
    }
}