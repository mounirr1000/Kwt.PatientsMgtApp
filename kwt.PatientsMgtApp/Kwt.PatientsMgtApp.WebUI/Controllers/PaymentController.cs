using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;

using Kwt.PatientsMgtApp.WebUI.Utilities;
using PagedList;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{

    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "PatientMgtException")]
    public class PaymentController : BaseController
    {
        private const int PageSize = 5;
        // GET: Patient
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        //private readonly IPayRepository _patientManagmentRepository;

        public PaymentController()
        {
            _paymentRepository = new PaymentRepository();
            _beneficiaryRepository = new BeneficiaryRepository();
            //_patientManagmentRepository = new PatientManagmentRepository();

        }
        // GET: Companion
        public ActionResult List(string searchpaymentText, string currentFilter, bool? isBeneficiary, string sortOrder, int? page, bool? clearSearch)
        {
            int pageNumber = (page ?? 1);
            ViewBag.isBeneficiary = isBeneficiary ?? false;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateInSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.CidSortParm = String.IsNullOrEmpty(sortOrder) ? "Cid" : "";
            ViewBag.BeneficiarySortParm = String.IsNullOrEmpty(sortOrder) ? "Beneficiary" : "";

            var payments = _paymentRepository.GetPayments();
            if (payments != null)
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        payments = payments.OrderBy(c => c.CompanionFName).ThenBy(c => c.CompanionLName).ToList();
                        break;
                    case "cid":
                        payments = payments.OrderBy(c => c.PatientCID).ToList();
                        break;
                    case "date_desc":
                        payments = payments.OrderBy(c => c.PaymentDate).ToList();
                        break;
                    //case "Beneficiary":
                    //    payments = payments.OrderBy(c => c.IsBeneficiary).ToList();
                    //    break;
                    default: // created date ascending 
                        payments = payments.OrderByDescending(c => c.CreatedDate).ToList();
                        break;
                }
                if (clearSearch != true)
                {
                    if (searchpaymentText != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchpaymentText = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchpaymentText;
                    var result = new List<PaymentModel>();
                    if (!String.IsNullOrEmpty(searchpaymentText))
                    {
                        var term = searchpaymentText.ToLower();
                        result = payments?
                            .Where(p =>
                                p.PatientCID.ToLower().Contains(term)
                                || p.CompanionFName.ToLower().Contains(term)
                                || p.CompanionLName.ToLower().Contains(term)
                                || p.PatientFName.ToLower().Contains(term)
                                || p.PatientLName.ToLower().Contains(term)
                            ).ToList();
                    }
                    if (result?.Count > 0)
                    {
                        Success(string.Format("We have <b>{0}</b> returned results from the searched criteria", result.Count),
                            true);
                        return View(result.ToPagedList(pageNumber, PageSize));
                        //return View(result);
                    }
                    if (!String.IsNullOrEmpty(searchpaymentText))
                    {
                        if (result?.Count == 0)
                        {
                            Information(
                                 string.Format(
                                     "There is no patient in our records with the selected search criteria <b>{0}</b>&nbsp<b>{1}</b>",
                                     searchpaymentText), true);
                        }
                    }
                }
            }

            return View(payments.ToPagedList(pageNumber, PageSize));
            //return View(patients);
        }

        [ExceptionHandler]
        public ActionResult Details(int paymentId)
        {

            var payment = _paymentRepository.GetPaymentById(paymentId);

            if (payment != null)
            {


                return View(payment);
            }
            else
            {
                Information(string.Format("Payment with Id <b>{0}</b> Does Not exist in our records.", paymentId), true);
                return View("List");
            }
        }

        //[ExceptionHandler]
        //[HttpGet]
        //public ActionResult AddPayment()
        //{
        //    PaymentModel payment = new PaymentModel();

        //    return View(payment);
        //}
        [ExceptionHandler]
        [HttpGet]
        public ActionResult Create(string patientCid)
        {
            PaymentModel payment = new PaymentModel();
            if (patientCid != null)
                payment = _paymentRepository.GetPaymentObject(patientCid);
            if (!String.IsNullOrEmpty(patientCid) &&
                (payment.PatientCID == null || payment.BeneficiaryCID == null))
            {
                
                Information(
                    String.Format(
                        "There is No patient in our records with this CID <b>{0}</b> ! Please Enter a Valid CID",
                        patientCid), true);
            }
           
            return View(payment);
        }
        [ExceptionHandler]
        [HttpPost]
        public ActionResult Create(PaymentModel payment)
        {
            ValidateCompanion(payment);
            if (ModelState.IsValid)
            {
                _paymentRepository.AddPayment(payment);
                Success(string.Format("Payment for patient with civil id <b>{0}</b> was successfully added.", payment.PatientCID), true);
                return RedirectToAction("List");
            }

            else
            {
                Danger(string.Format("Please correct the error list before proceeding"), true);
                //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
                //companion.Banks = _patientManagmentRepository.GetBanks();
                return View(payment);
            }

        }

        [ExceptionHandler]
        public ActionResult Edit(int paymentId)
        {
            var payment = _paymentRepository.GetPaymentById(paymentId);
            //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
            //companion.Banks = _patientManagmentRepository.GetBanks();
            return View(payment);
        }
        [HttpPost]
        [ExceptionHandler]
        public ActionResult Edit(PaymentModel payment)
        {
            ValidateCompanion(payment);
            if (ModelState.IsValid)
            {
                _paymentRepository.UpdatePayment(payment);
                Success(string.Format("Payment for patient with  CId <b>{0}</b> was successfully updated.", payment.PatientCID), true);
                return RedirectToAction("Details", "Payment", new { paymentId = payment.Id });
            }
            else
            {
                Information(string.Format("Payment for patient with  CId  <b>{0}</b> Was Not updated.", payment.PatientCID), true);
                //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
                //companion.Banks = _patientManagmentRepository.GetBanks();
                return View(payment);
            }
        }

        private void ValidateCompanion(PaymentModel payment)
        {
            if (ModelState.IsValidField("PaymentStartDate") &&
                ModelState.IsValidField("PaymentEndDate"))
            {
                //Todo: check for current date as well
                if (DateTime.Parse(payment.PaymentStartDate?.ToString()) >
                    DateTime.Parse(payment.PaymentEndDate?.ToString()))
                {
                    ModelState.AddModelError("PaymentStartDate", "Payment Start Date should not be after payment end Date");
                    ModelState.AddModelError("PaymentEndDate", "Payment End Date should not be before payment end Date");
                    return;
                }

            }
            var historyPaments = _paymentRepository.GetPaymentsByPatientCid(payment.PatientCID);
            var duplicatPayment = historyPaments
                                .Any(p => p.PaymentStartDate == payment.PaymentStartDate &&
                                    p.PaymentEndDate == payment.PaymentEndDate &&
                                    p.PatientPayRate == payment.PatientPayRate &&
                                    p.CompanionPayRate == payment.CompanionPayRate);

            if (duplicatPayment)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with this end date");
            }
            var stratDateConflictPayment = historyPaments
                               .Any(p => (p.PaymentStartDate <= payment.PaymentStartDate
                                       && payment.PaymentStartDate <= p.PaymentEndDate)
                                       || (payment.PaymentEndDate <= p.PaymentEndDate
                                       && payment.PaymentStartDate >= p.PaymentStartDate));
            if (stratDateConflictPayment)
            {
                ModelState.AddModelError("PaymentStartDate", "Our records indicates that there is payment conflict with this payment start date");
            }
            var endDateConflictPayment = historyPaments
                               .Any(p => (payment.PaymentEndDate <= p.PaymentEndDate
                                       && payment.PaymentStartDate >= p.PaymentStartDate)||
                                       (payment.PaymentEndDate <= p.PaymentEndDate
                                       && payment.PaymentStartDate < p.PaymentStartDate));
            if (endDateConflictPayment)
            {
                ModelState.AddModelError("PaymentEndDate", "Our records indicates that there is payment conflict with this payment end date");
            }
            //var companionType = _companionManagmentRepository.GetCompanionTypes()
            //                             .Where(c => c.Id == (int)Enums.CompanionType.Primary)
            //                             .Select(ct => ct.CompanionType).FirstOrDefault();
            // get the patient Associated with this companion
            //if (payment.PatientCID != null)
            //{
            //    var patient = _patientRepository.GetPatient(payment.PatientCID);
            //    if (patient == null && ModelState.IsValidField("PatientCID"))
            //    {
            //        ModelState.AddModelError("PatientCID", payment.PatientCID + " is an incorrect Patient Cid, There is not patient in our record with this Cid");
            //    }
            //    if (patient != null && ModelState.IsValidField("PatientCID"))
            //    {
            //        //if (patient.IsBeneficiary && payment.IsBeneficiary)
            //        //    ModelState.AddModelError("IsBeneficiary", "The patient with " + companion.PatientCID + " CID associated with this companion is already Beneficiary, You can't have the companion as beneficiary");
            //        //if (!patient.IsBeneficiary && !companion.IsBeneficiary)
            //        //    if (ModelState.IsValidField("CompanionType") && companion.CompanionType == companionType)
            //        //        ModelState.AddModelError("IsBeneficiary", "The patient with " + companion.PatientCID +
            //        //                    " CID associated with this companion is Not Beneficiary, So you need to set this companion as Beneficiary");
            //    }
            //    if (patient != null)
            //    {
            //        if (ModelState.IsValidField("CompanionType") && payment.CompanionType == companionType)
            //        {
            //            var existingCompanions = _companionRepository.GetCompanions().Where(c => c.PatientCID == companion.PatientCID && c.IsActive);

            //            foreach (var comp in existingCompanions)
            //            {
            //                if (comp.CompanionCID != companion.CompanionCID)
            //                {
            //                    if (comp.CompanionType == companionType)
            //                    {
            //                        ModelState.AddModelError("CompanionType", "There is already a companion with the patient declared as Primary, You can't have this companion as primary");
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        // see if there is a c primary companion with this patient, we can't have two companion as primary
            //    }
            //}
            //if (ModelState.IsValidField("IsBeneficiary")
            //   && companion.IsBeneficiary == true
            //   && companion.IBan == null)
            //{
            //    ModelState.AddModelError("IBan", "Since the companion is Beneficiary, you need to enter the Bank Account field");
            //}
            //if (ModelState.IsValidField("IsBeneficiary")
            //    && companion.IsBeneficiary == true
            //    && companion.BankName == null)
            //{
            //    ModelState.AddModelError("BankName", "Since the companion is Beneficiary, you need to enter the Bank Name field");
            //}
            //if (ModelState.IsValidField("IsBeneficiary")
            //    && companion.IsBeneficiary == false
            //    && companion.BankName != null)
            //{
            //    ModelState.AddModelError("BankName", "The companion is Not Beneficiary, so no need to enter the Bank Name field");
            //}
            //if (ModelState.IsValidField("IsBeneficiary")
            //    && companion.IsBeneficiary == false
            //    && companion.IBan != null)
            //{
            //    ModelState.AddModelError("IBan", "The companion is Not Beneficiary, so no need to enter the Bank Account field");
            //}
            //if (ModelState.IsValidField("isActive")
            //    && companion.IsActive == false
            //    && companion.DateOut == null)
            //{
            //    ModelState.AddModelError("DateOut", "The companion is not active, so you need to enter the Date out field");
            //}
            //if (ModelState.IsValidField("DateOut")
            //    && companion.DateOut != null
            //    && companion.IsActive)
            //{
            //    ModelState.AddModelError("IsActive", "Date out is set, so the companion Active status should be No");
            //}
            //// if the companion is not Primary and the user select it to be beneficiary
            //if (ModelState.IsValidField("CompanionType"))
            //{

            //    if (companion.IsBeneficiary && companion.CompanionType != companionType)
            //        ModelState.AddModelError("CompanionType", companion.CompanionType +
            //            " Can't be Beneficiary, Only " + companionType + " companion who can");
            //}
        }
    }
}