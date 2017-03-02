using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace CurrencyRetriever.WebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ICurrencyService.ICurrencyService, CurrencyService.CurrencyService>();
            
            //GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}