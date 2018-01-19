using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Utils
{
    public class HandleAndLogErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var message = string.Format("Exception     : {0}\n" +
                                        "InnerException: {1}",
                filterContext.Exception,
                filterContext.Exception.InnerException);

            Logging.WriteToAppLog(message, filterContext.Exception);
            
            filterContext.ExceptionHandled = true;

            base.OnException(filterContext);

            // Verify if AJAX request
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // Use partial view in case of AJAX request
                var result = new JsonResult();
                filterContext.Result = result;
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml"
                };
            }
        }
    }

    public class HandleWebApiException : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionContext)
        {
            Logging.WriteToAppLog("WebAPI Error", actionContext.Exception);

            actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var resex = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            throw new HttpResponseException(resex);
        }
    }
}