using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.DataAccess.SQL;

namespace Kwt.PatientsMgtApp.WebUI.CustomFilter
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        // for any exception that is thrown by the app, use this to log it and direct the user to a friendly web message
        //protected override void OnException(ExceptionContext filterContext)

        readonly IExceptionLoggerRepository _exceptionLogger = new ExceptionLoggerRepository();
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                ExceptionLoggerObject logger = new ExceptionLoggerObject()
                {
                    ExceptionMessage = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                };

                _exceptionLogger.AddExceptionLogger(logger);

                //filterContext.ExceptionHandled = true;
            }
        }
    }
}