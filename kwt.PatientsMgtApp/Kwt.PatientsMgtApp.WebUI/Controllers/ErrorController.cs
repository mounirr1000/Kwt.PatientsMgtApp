using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using Microsoft.Ajax.Utilities;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        readonly IExceptionLoggerRepository _exceptionLogger = new ExceptionLoggerRepository();
        public ActionResult Index(string errorMessage)
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer;
          var exception=  _exceptionLogger.GetLatestExceptionLogger();
            errorMessage = exception?.ExceptionMessage ?? errorMessage;

            if (string.IsNullOrEmpty(errorMessage)&& TempData["errorMessage"]!=null)
            {
                errorMessage = (string) TempData["errorMessage"];
            }
            Danger(@"The Action was not completed successfully due to the following error(s): ", true);
            if(!errorMessage.IsNullOrWhiteSpace())
            Warning(errorMessage, true);
            if (url != null)
            {
                TempData["errorMessage"] = null;
                return Redirect(url.PathAndQuery);
            }
            TempData["errorMessage"] = null;
            return RedirectToAction("Index", "Home");
        }
        protected override void OnException(ExceptionContext filterContext)
        {

            filterContext.Result = RedirectToAction("Index", "Error", new { errorMessage = filterContext.Exception.Message });
            TempData["errorMessage"] = filterContext.Exception.Message;
            filterContext.ExceptionHandled = false;
        }


        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Status404()
        {
            
            return View();
        }

        public ActionResult Status405()
        {

            return View();
        }
        public ActionResult GeneralError()
        {

            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}