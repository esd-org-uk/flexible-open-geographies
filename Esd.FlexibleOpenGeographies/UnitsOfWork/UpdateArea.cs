using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpdateArea : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaFull _area;
        private readonly IUnitOfWork _addGeometry;

        public UpdateArea(IContextFactory contextFactory, AreaFull area, IUnitOfWork addGeometry)
        {
            _contextFactory = contextFactory;
            _area = area;
            _addGeometry = addGeometry;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var areaDetail = context.AreaDetails.SingleOrDefault(area => area.Id == _area.Id);
                if (areaDetail == null) return;
                UpdateBasicFields(areaDetail);
                UpdateGeometryField(areaDetail);
                UpdateAlternateLabels(areaDetail, context);
                context.SaveChanges();
            }
            if (!string.IsNullOrWhiteSpace(_area.GeometryString)) _addGeometry.Execute();
        }

        private void UpdateBasicFields(AreaDetail area)
        {
            area.Label = _area.Label;
            area.Colour = _area.Colour;
            area.SameAsLink = _area.SameAsLink;
            area.UpdateDate = DateTime.UtcNow;
        }

        private void UpdateGeometryField(AreaDetail area)
        {
            if (_area.GeometryString != null) area.ShapeDocument = _area.GeometryString;
        }

        private void UpdateAlternateLabels(AreaDetail area, IFogContext context)
        {
            var existing = area.AlternateLabels.Select(label => label.Label).ToList();
            var remove = existing.Except(_area.AlternateLabels).ToList();
            var add = _area.AlternateLabels.Except(existing).ToList();
            var entities = area.AlternateLabels.Where(label => remove.Contains(label.Label)).ToList();
            foreach (var entity in entities)
            {
                area.AlternateLabels.Remove(entity);
                context.AreaAlternateLabels.Remove(entity);
            }
            foreach (var label in add)
                area.AlternateLabels.Add(new AreaAlternateLabel { AreaId = area.Id, Label = label });
        }
    }
}
