using Esd.FlexibleOpenGeographies.Dtos;
using System.Data.Entity;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.Queries
{
    internal class AreaTypeDetailsByCode : IQuerySingle<AreaTypeDetails>
    {
        private readonly IContextFactory _contextFactory;
        private readonly string _code;

        public AreaTypeDetailsByCode(IContextFactory contextFactory, string code)
        {
            _contextFactory = contextFactory;
            _code = code;
        }

        public AreaTypeDetails Find()
        {
            using (var context = _contextFactory.Create())
            {
                var areaType = context.AreaTypes.Include(x => x.AlternateLabels).Include(x => x.Creator).AsNoTracking()
                                      .SingleOrDefault(x => x.Code == _code);
                if (areaType == null) return null;
                var children = context.TypeHierarchies.Include(x => x.ChildAreaType).AsNoTracking()
                                     .Where(x => x.TypeCode == _code)
                                     .Select(x => new AreaTypeBasic { Code = x.ChildAreaType.Code, Label = x.ChildAreaType.Label })
                                     .ToList();
                var parents = context.TypeHierarchies.Include(x => x.AreaType).AsNoTracking()
                                     .Where(x => x.ChildTypeCode == _code)
                                     .Select(x => new AreaTypeBasic { Code = x.AreaType.Code, Label = x.AreaType.Label })
                                     .ToList();
                var groupMembers = context.AreaTypeGroupMembers.Include(x => x.ChildAreaType).AsNoTracking()
                                          .Where(x => x.TypeCode == _code)
                                          .Select(x => new AreaTypeBasic { Code = x.ChildAreaType.Code, Label = x.ChildAreaType.Label })
                                          .ToList();
                var typeHierarchy = context.TypeHierarchies.AsNoTracking()
                                           .FirstOrDefault(x => x.TypeCode == _code && x.IsPrimary);
                var primaryChildType = typeHierarchy == null ? null : new AreaTypeBasic
                {
                    Code = typeHierarchy.ChildTypeCode,
                    Label = typeHierarchy.ChildAreaType.Label
                };
                var creatorDescription = areaType.Creator == null ? null : areaType.Creator.Name;
                if (areaType.Creator != null && areaType.Creator.Organisation != null)
                    creatorDescription = string.Format("{0} ({1})", creatorDescription, areaType.Creator.Organisation.OrganisationName);
                return new AreaTypeDetails
                {
                    Code = _code,
                    Label = areaType.Label,
                    MetricUploadPermissionLevelId = areaType.MetricUploadPermissionLevelId,
                    AlternateLabels = areaType.AlternateLabels.Select(label => label.Label).ToList(),
                    ChildTypes = children,
                    PrimaryChildType = primaryChildType,
                    ParentTypes = parents,
                    Creator = areaType.CreatorId,
                    CreatorDescription = creatorDescription,
                    Organisation = areaType.Creator == null ? null : areaType.Creator.OrganisationId,
                    CreateDate = areaType.CreateDate,
                    UpdateDate = areaType.UpdateDate,
                    SameAsLink = areaType.SameAsLink,
                    IsGroup = areaType.IsGroup ?? false,
                    GroupMembers = groupMembers
                };
            }
        }
    }
}
