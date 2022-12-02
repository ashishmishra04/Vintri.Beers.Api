using System.Web.Http;

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Web API Configuration
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register method for Web Api Configuration
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
