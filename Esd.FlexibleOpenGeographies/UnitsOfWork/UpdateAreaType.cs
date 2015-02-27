using Esd.FlexibleOpenGeographies.Data;
using Esd.FlexibleOpenGeographies.Dtos;
using System;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpdateAreaType : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly AreaTypeEditableDetails _areaType;

        public UpdateAreaType(IContextFactory contextFactory, AreaTypeEditableDetails areaType)
        {
            _contextFactory = contextFactory;
            _areaType = areaType;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var areaType = context.AreaTypes.SingleOrDefault(type => type.Code == _areaType.Code);
                if (areaType == null) return;
                UpdateBasicFields(areaType);
                UpdateAlternateLabels(areaType, context);
                UpdateParentAreas(context);
                UpdateComprisingAreas(context);
                UpdateGroupMembers(context);
                context.SaveChanges();
            }
        }

        private void UpdateBasicFields(AreaType areaType)
        {
            areaType.Label = _areaType.Label;
            areaType.MetricUploadPermissionLevelId = _areaType.MetricUploadPermissionLevelId;
            areaType.UpdateDate = DateTime.UtcNow;
            areaType.SameAsLink = _areaType.SameAsLink;
        }

        private void UpdateAlternateLabels(AreaType areaType, IFogContext context)
        {
            var existing = areaType.AlternateLabels.Select(label => label.Label).ToList();
            var remove = existing.Except(_areaType.AlternateLabels).ToList();
            var add = _areaType.AlternateLabels.Except(existing).ToList();
            var entities = areaType.AlternateLabels.Where(label => remove.Contains(label.Label)).ToList();
            foreach (var entity in entities)
            {
                areaType.AlternateLabels.Remove(entity);
                context.AreaTypeAlternateLabels.Remove(entity);
            }
            foreach (var label in add)
                areaType.AlternateLabels.Add(new AreaTypeAlternateLabel { TypeCode = areaType.Code, Label = label });
        }

        private void UpdateParentAreas(IFogContext context)
        {
            var existing = context.TypeHierarchies
                                  .Where(hierarchy => hierarchy.ChildTypeCode == _areaType.Code)
                                  .Select(hierarchy => hierarchy.TypeCode)
                                  .ToList();
            var remove = existing.Except(_areaType.ParentTypes).ToList();
            var add = _areaType.ParentTypes.Except(existing).ToList();
            var entities = context.TypeHierarchies
                                  .Where(hierarchy => hierarchy.ChildTypeCode == _areaType.Code &&
                                                      remove.Contains(hierarchy.TypeCode))
                                  .ToList();
            foreach (var entity in entities)
                context.TypeHierarchies.Remove(entity);
            foreach (var hierarchy in add)
                context.TypeHierarchies.Add(new TypeHierarchy
                {
                    TypeCode = hierarchy,
                    ChildTypeCode = _areaType.Code,
                    IsPrimary = false
                });
        }

        private void UpdateComprisingAreas(IFogContext context)
        {
            var existing = context.TypeHierarchies
                                  .Where(hierarchy => hierarchy.TypeCode == _areaType.Code)
                                  .Select(hierarchy => hierarchy.ChildTypeCode)
                                  .ToList();
            var remove = existing.Except(_areaType.ChildTypes).ToList();
            var add = _areaType.ChildTypes.Except(existing).ToList();
            var entities = context.TypeHierarchies
                                  .Where(x => x.TypeCode == _areaType.Code &&
                                              remove.Contains(x.ChildTypeCode))
                                  .ToList();
            var hasPrimaryChildType = context.TypeHierarchies.AsNoTracking()
                                             .Any(x => x.TypeCode == _areaType.Code &&
                                                       x.IsPrimary &&
                                                       !remove.Contains(x.ChildTypeCode));
            foreach (var entity in entities)
                context.TypeHierarchies.Remove(entity);
            foreach (var hierarchy in add)
            {
                var primary = !hasPrimaryChildType;
                hasPrimaryChildType = true;
                context.TypeHierarchies.Add(new TypeHierarchy
                {
                    TypeCode = _areaType.Code,
                    ChildTypeCode = hierarchy,
                    IsPrimary = primary
                });
            }
        }

        private void UpdateGroupMembers(IFogContext context)
        {
            var existing = context.AreaTypeGroupMembers
                                  .Where(x => x.TypeCode == _areaType.Code)
                                  .Select(x => x.ChildTypeCode)
                                  .ToList();
            var remove = existing.Except(_areaType.GroupMembers).ToList();
            var add = _areaType.GroupMembers.Except(existing).ToList();
            var entities = context.AreaTypeGroupMembers
                                  .Where(x => x.TypeCode == _areaType.Code &&
                                              remove.Contains(x.ChildTypeCode))
                                  .ToList();
            context.AreaTypeGroupMembers.RemoveRange(entities);
            context.AreaTypeGroupMembers.AddRange(add.Select(x => new AreaTypeGroupMember
            {
                TypeCode = _areaType.Code,
                ChildTypeCode = x
            }));
        }
    }
}
