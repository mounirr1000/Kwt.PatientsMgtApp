using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;
using Kwt.PatientsMgtApp.WebUI.Infrastructure;
using Kwt.PatientsMgtApp.WebUI.Utilities;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    
    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "ExceptionHandler")]
    //[CustomAuthorize(Roles = "Admin, Manager, Super Admin, User, Accountant, Auditor, Editor")]
    [CustomAuthorize(Roles = CrudRoles.PatientCrudRolesForAutorizeAttribute)]
    public class ReportController : BaseController
    {
        // GET: Report

        public ActionResult List()
        {
            return View();
        }
        public ActionResult Patient()
        {
            return View();
        }

        [CustomAuthorize(Roles = CrudRoles.PaymentReportCrudRolesForAutorizeAttribute)]
        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult PaymentReference()
        {
            return View();
        }
        public ActionResult PayrollReport()
        {
            return View();
        }
        public ActionResult DepositReport()
        {
            return View();
        }
    }
}