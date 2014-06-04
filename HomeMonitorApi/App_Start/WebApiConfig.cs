using System.Web.Http;

namespace HomeMonitorApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new HomeMonitorExceptionFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
