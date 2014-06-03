using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace HomeMonitorApi
{
    public class HomeMonitorExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage {ReasonPhrase = context.Exception.Message};
            if (context.Exception is NoTemperatureReadingsException)
            {
                context.Response.StatusCode = HttpStatusCode.NoContent;
            }
        }
    }
}