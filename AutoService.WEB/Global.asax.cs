using AutoService.BLL.Infrastructure;
using AutoService.WEB.Utils;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AutoService.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // внедрение зависимостей
            NinjectModule registrations = new NinjectRegistrations();
            NinjectModule bllRegistrations = new ServiceModule("DefaultConnection");
            var kernel = new StandardKernel(registrations,bllRegistrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}