using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.Models;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using Microsoft.Ajax.Utilities;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPaymentRepository _paymentRepository;

        public HomeController()
        {
            _patientRepository = new PatientRepository();
            _paymentRepository = new PaymentRepository();
        }

        [Authorize]
        public ActionResult Index()
        {
            HomeViewModels model = new HomeViewModels();
            int totalActivePatients = 0;
            int totalPayment = 0;
            int totalTodaysPatient = 0;
            int totalTodaysPayments = 0;
            List<PatientModel> activePatients = new List<PatientModel>();
            List<PaymentModel> paymentList= new List<PaymentModel>();
            var patientslist = _patientRepository.GetPatients();
            var payments = _paymentRepository.GetPayments();
            if (patientslist != null)
            {
                activePatients = patientslist.Where(p => p.IsActive == true).ToList();
                foreach (var pa in activePatients)
                {
                    var pay = payments.Where(p => p.PatientCID==pa.PatientCID).ToList();
                    paymentList.AddRange(pay);
                }
                totalActivePatients = activePatients.Count();
                totalPayment = paymentList.Count();
                totalTodaysPatient = activePatients.Count(pa => pa.CreatedDateFormatted == DateTime.Now.Date.ToString("d"));
                totalTodaysPayments = payments.Count(p => p.PaymentDateFormatted == DateTime.Now.Date.ToString("d"));
            }
            model.TotalActivePatients = totalActivePatients;
            model.TotalPayments = totalPayment;
            model.TodaysPatients = totalTodaysPatient;
            model.TodayDateTime = DateTime.Now;
            model.TotalTodaysPayments = totalTodaysPayments;
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}