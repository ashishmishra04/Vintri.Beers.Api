using System.Web.Http;

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Web Api Application Initialization
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application Start Function
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
