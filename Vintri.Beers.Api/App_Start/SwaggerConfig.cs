using System;
using System.Web.Http;
using WebActivatorEx;
using Vintri.Beers.Api;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Swagger Configuration
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Swagger Registration 
        /// </summary>
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                         c.SingleApiVersion("v1", "Vintri.Beers.Api");
                         c.IncludeXmlComments(GetXmlCommentsPath());
                    })
                .EnableSwaggerUi();
        }

        /// <summary>
        /// Get the XML comments path for documentation for swagger API
        /// </summary>
        /// <returns></returns>
        private static string GetXmlCommentsPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"\bin\Vintri.Beers.Api.xml";
        }
    }
}
