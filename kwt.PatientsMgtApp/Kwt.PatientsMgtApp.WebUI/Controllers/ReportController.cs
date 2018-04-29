using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.WebUI.Utilities;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
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

        public ActionResult Payment()
        {
            return View();
        }
    }
}