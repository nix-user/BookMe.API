using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace BookMe.Infrastructure.IoC
{
    public static class IoCConfig
    {
        public static void RegisterControllers(HttpConfiguration configuration)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterWebApiControllers(configuration);
            container.RegisterPackages();
            //container.Verify(); this code should be uncommented when converters registration is ready

            configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}