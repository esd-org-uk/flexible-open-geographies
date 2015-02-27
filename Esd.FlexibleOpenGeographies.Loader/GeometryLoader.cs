using System.Collections.Generic;
using System.Linq;
using Esd.FlexibleOpenGeographies.Dtos;

namespace Esd.FlexibleOpenGeographies.Loader
{
    internal class GeometryLoader : ILoader
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IQueryFactory _queryFactory;

        public GeometryLoader(IUnitOfWorkFactory unitOfWorkFactory, IQueryFactory queryFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _queryFactory = queryFactory;
        }

        public void Load()
        {
            var areasWithKml = _queryFactory.CreateAllAreasWithKmlQuery().Fetch().ToList();
            var areasWithGeometry = _queryFactory.CreateAllAreasWithGeometryQuery().Fetch().ToList();
            var areasRequiringLoad = areasWithKml.Except(areasWithGeometry, new MatchingCodeAndType());
            foreach (var area in areasRequiringLoad)
            {
                var kml = _queryFactory.CreateKmlForTypeAndCodeQuery(area.TypeCode, area.Code).Find();
                if (kml != null) _unitOfWorkFactory.CreateUpsertGeometryProcess(area.Code, kml, area.TypeCode).Execute();
            }
        }

        private class MatchingCodeAndType : IEqualityComparer<AreaBasicWithType>
        {
            public bool Equals(AreaBasicWithType x, AreaBasicWithType y)
            {
                return x.Code == y.Code && x.TypeCode == y.TypeCode;
            }

            public int GetHashCode(AreaBasicWithType obj)
            {
                return obj.Code.GetHashCode() ^ obj.TypeCode.GetHashCode();
            }
        }
    }
}
