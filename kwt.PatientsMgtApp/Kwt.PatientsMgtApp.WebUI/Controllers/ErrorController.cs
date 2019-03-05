using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.WebUI.Utilities;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult Index(string errorMessage)
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer;
            if (string.IsNullOrEmpty(errorMessage)&& TempData["errorMessage"]!=null)
            {
                errorMessage = (string) TempData["errorMessage"];
            }
            Danger("The Action was not completed successfully due to the following error: "
                    +errorMessage, true);
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
    }
}