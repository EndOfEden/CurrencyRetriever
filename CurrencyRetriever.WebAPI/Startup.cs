using Microsoft.Owin;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(CurrencyRetriever.WebAPI.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Config\\log4net.xml", Watch = true)]
namespace CurrencyRetriever.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //// token generation
            //app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            //{
            //    AllowInsecureHttp = false,

            //    TokenEndpointPath = new PathString("/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromHours(8),

            //    Provider = new SimpleAuthorizationServerProvider()
            //});

            //// token consumption
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            UnityConfig.RegisterComponents(config);
            SwaggerConfig.Register(config);
            WebApiConfig.Register(config);
            
            app.UseWebApi(config);
        }
    }
}