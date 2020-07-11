[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AutoService.WEB.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AutoService.WEB.App_Start.NinjectWebCommon), "Stop")]

namespace AutoService.WEB.App_Start
{
    using AutoService.WEB.Models;
    using AutoService.WEB.Utils;
    using AutoService.WEB.Utils.Interfaces;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.Common.WebHost;
    using System;
    using System.Web;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
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

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IServicesLogic>().To<ServicesLogic>();
            kernel.Bind<IContactInfoLogic>().To<ContactInfoLogic>();
            kernel.Bind<ApplicationDbContext>().ToSelf();
            kernel.Bind<IReviewsLogic>().To<ReviewsLogic>();
            kernel.Bind<IAdminLogic>().To<AdminLogic>();
            kernel.Bind<ISummariesLogic>().To<SummraiesLogic>();
            kernel.Bind<ICarLogic>().To<CarLogic>();
            kernel.Bind<IHomePageLogic>().To<HomePageLogic>();
            kernel.Bind<IVacanciesLogic>().To<VacanciesLogic>();
            kernel.Bind<INewsPageLogic>().To<NewsPageLogic>();
        }
    }
}