using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Filters;
using log4net;

namespace Vintri.Beers.Api
{
    /// <summary>
    /// Web Api Application Initialization
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// Application Start Function
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new LogExceptionFilterAttribute());
        }

        // Added Method To log any exception throw by the API
        private class LogExceptionFilterAttribute : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext context)
            {
                Log.Error(context.Exception);
            }
        }
    }
}
