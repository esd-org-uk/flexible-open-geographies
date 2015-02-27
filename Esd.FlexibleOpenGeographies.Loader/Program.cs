using Ninject;
using System;

namespace Esd.FlexibleOpenGeographies.Loader
{
    internal class Program
    {
        private static readonly StandardKernel Kernel = new StandardKernel();

        internal static void Main(string[] loaderTypes)
        {
            BindServices();
            var unitOfWorkFactory = Kernel.Get<IUnitOfWorkFactory>();
            var kmlReader = Kernel.Get<IKmlReader>();
            var queryFactory = Kernel.Get<IQueryFactory>();
            foreach (var loaderType in loaderTypes)
                CreateLoader(loaderType, unitOfWorkFactory, kmlReader, queryFactory).Load();
        }

        private static ILoader CreateLoader(
            string loaderType, 
            IUnitOfWorkFactory unitOfWorkFactory, 
            IKmlReader kmlReader, 
            IQueryFactory queryFactory)
        {
            switch (loaderType)
            {
                case "kml":
                    return new KmlLoader(unitOfWorkFactory, kmlReader, queryFactory);
                case "geometry":
                    return new GeometryLoader(unitOfWorkFactory, queryFactory);
                case "aggregate":
                    return new GeographyAggregator(unitOfWorkFactory, queryFactory);
                default:
                    throw new ArgumentException("Unknown loader type", "loaderType");
            }
        }

        private static void BindServices()
        {
            Kernel.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InSingletonScope();
            Kernel.Bind<IContextFactory>().To<ContextFactory>().InSingletonScope();
            Kernel.Bind<IKmlReader>().To<KmlReader>().InSingletonScope();
            Kernel.Bind<IQueryFactory>().To<QueryFactory>().InSingletonScope();
            Kernel.Bind<IFragmentExtractorFactory>().To<FragmentExtractorFactory>().InSingletonScope();
            Kernel.Bind<IGeoContentTypeDetector>().To<GeoContentTypeDetector>().InSingletonScope();
        }
    }
}
