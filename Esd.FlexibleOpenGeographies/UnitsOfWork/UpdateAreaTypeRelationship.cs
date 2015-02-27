using Esd.FlexibleOpenGeographies.Dtos;
using System.Linq;

namespace Esd.FlexibleOpenGeographies.UnitsOfWork
{
    internal class UpdateAreaTypeRelationship : IUnitOfWork
    {
        private readonly IContextFactory _contextFactory;
        private readonly TypeHierarchyBasic _relationship;

        public UpdateAreaTypeRelationship(IContextFactory contextFactory, TypeHierarchyBasic relationship)
        {
            _contextFactory = contextFactory;
            _relationship = relationship;
        }

        public void Execute()
        {
            using (var context = _contextFactory.Create())
            {
                var entity = context.TypeHierarchies
                                    .SingleOrDefault(x => x.TypeCode == _relationship.TypeCode
                                                          && x.ChildTypeCode == _relationship.ChildTypeCode);
                if (entity == null
                    || (entity.CoversWhole == _relationship.CoversWhole
                        && entity.IsPrimary == _relationship.IsPrimary))
                    return;
                entity.CoversWhole = _relationship.CoversWhole;
                entity.IsPrimary = _relationship.IsPrimary;
                context.SaveChanges();
            }
        }
    }
}
