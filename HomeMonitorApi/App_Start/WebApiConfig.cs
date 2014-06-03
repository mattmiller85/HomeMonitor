using System;
using System.Collections.Generic;
using System.Linq;
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

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new {action = "latest", count = RouteParameter.Optional}
            //    );

            config.Routes.MapHttpRoute(
                name: "DefaultTemperatureApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = "latest" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultTemperatureHistoryApi",
                routeTemplate: "api/{controller}/{action}/{count}",
                defaults: new { action = "latest", count = 1 }
            );
        }
    }
}
