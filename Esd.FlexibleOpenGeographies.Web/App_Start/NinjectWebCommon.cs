using Esd.FlexibleOpenGeographies.SignIn;
using Esd.FlexibleOpenGeographies.SignIn.UserProvider;
using Esd.FlexibleOpenGeographies.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using System;
using System.Web;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace Esd.FlexibleOpenGeographies.Web
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }
        
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IBindingRoot kernel)
        {
            //Singleton scope
            kernel.Bind<IContextFactory>().To<ContextFactory>().InSingletonScope();
            kernel.Bind<IQueryFactory>().To<QueryFactory>().InSingletonScope();
            kernel.Bind<IUnitOfWorkFactory>().To<UnitOfWorkFactory>().InSingletonScope();
            kernel.Bind<IFragmentExtractorFactory>().To<FragmentExtractorFactory>().InSingletonScope();
            kernel.Bind<IGeoContentTypeDetector>().To<GeoContentTypeDetector>().InSingletonScope();
            kernel.Bind<IOAuthManager>().To<OAuthManager>().InSingletonScope();
            kernel.Bind<IUserProvider>().To<DummyUserProvider>().InSingletonScope();
            kernel.Bind<IOAuthSettingsProvider>().To<DummyOAuthSettingsProvider>().InSingletonScope();
            //Request scope
            kernel.Bind<IKmlReader>().To<KmlReader>().InRequestScope();
        }        
    }
}
