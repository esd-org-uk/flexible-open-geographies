using System.Linq;

namespace Esd.FlexibleOpenGeographies.Loader
{
    internal class KmlLoader : ILoader
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IKmlReader _kmlReader;
        private readonly IQueryFactory _queryFactory;

        public KmlLoader(IUnitOfWorkFactory unitOfWorkFactory, IKmlReader kmlReader, IQueryFactory queryFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _kmlReader = kmlReader;
            _queryFactory = queryFactory;
        }

        public void Load()
        {
            var areasWithMissingKml = _queryFactory.CreateAllAreasWithNoKmlQuery().Fetch();
            //OutputArea currently doesn't have KML in web services
            foreach (var area in areasWithMissingKml.Where(x => x.TypeCode != "OutputArea"))
            {
                var kmlString = _kmlReader.KmlStringForCode(area.Code);
                if (kmlString != null) _unitOfWorkFactory.CreateUpdateKmlProcess(area.Id, kmlString).Execute();
            }
        }
    }
}
