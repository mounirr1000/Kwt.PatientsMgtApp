using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;
using Kwt.PatientsMgtApp.WebUI.Utilities;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    
    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "ExceptionHandler")]
    [CustomAuthorize(Roles = "Admin, Manager, Super Admin, User, Accountant, Auditor, Editor")]
    public class ReportController : BaseController
    {
        // GET: Report

        public ActionResult List()
        {
            return View("Patient");
        }
        public ActionResult Patient()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult PaymentReference()
        {
            return View();
        }
    }
}