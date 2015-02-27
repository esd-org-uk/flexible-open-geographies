using System.Linq;

namespace Esd.FlexibleOpenGeographies.Loader
{
    internal class GeographyAggregator : ILoader
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IQueryFactory _queryFactory;

        public GeographyAggregator(IUnitOfWorkFactory unitOfWorkFactory, IQueryFactory queryFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _queryFactory = queryFactory;
        }

        public void Load()
        {
            while (true)
            {
                var areas = _queryFactory.CreateAreasForCalculatedGeometryQuery().Fetch().ToList();
                if (!areas.Any()) break;
                foreach (var area in areas)
                    _unitOfWorkFactory.CreateUpsertCalculatedGeometryProcess(
                        area.ChildAreaCodes, area.AreaCode, area.TypeCode, area.ChildTypeCode).Execute();
            }
        }
    }
}
