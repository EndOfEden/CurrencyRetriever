using System.Web.Http;
using System.Web.Http.Cors;

namespace CurrencyRetriever.WebAPI
{
    public static class WebApiConfig
    {
        public static HttpConfiguration Register(HttpConfiguration config)
        {
            log4net.Config.XmlConfigurator.Configure();

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            return config;
        }
    }
}
