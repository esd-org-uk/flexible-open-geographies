using System.Linq;
using Esd.FlexibleOpenGeographies.Data;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class DeleteAreaComposition : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly int _parentAreaId;
        private readonly int _childAreaId;

        public DeleteAreaComposition(IContextFactory contextFactory, int parentAreaId, int childAreaId)
        {
            _contextFactory = contextFactory;
            _parentAreaId = parentAreaId;
            _childAreaId = childAreaId;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var composition = context.AreaCompositions.SingleOrDefault(
                    x => x.AreaId == _parentAreaId && x.ChildAreaId == _childAreaId);
                if (composition == null) return;
                context.AreaCompositions.Remove(composition);
                SetParentRecalculate(context);
                context.SaveChanges();
            }
        }

        private void SetParentRecalculate(IFogContext context)
        {
            //Recalculate if we have removed one of the child areas of the primary child type and there is no KML
            var parentArea = context.AreaDetails.SingleOrDefault(x => x.Id == _parentAreaId);
            if (parentArea == null || parentArea.ShapeDocument != null) return;
            var childType = context.AreaDetails.AsNoTracking()
                                   .Where(x => x.Id == _childAreaId)
                                   .Select(x => x.TypeCode)
                                   .SingleOrDefault();
            if (childType == null) return;
            if (context.TypeHierarchies.AsNoTracking()
                       .Any(x => x.TypeCode == parentArea.TypeCode &&
                                 x.ChildTypeCode == childType &&
                                 x.IsPrimary))
                parentArea.RequiresGeometryCalculation = true;
        }
    }
}
