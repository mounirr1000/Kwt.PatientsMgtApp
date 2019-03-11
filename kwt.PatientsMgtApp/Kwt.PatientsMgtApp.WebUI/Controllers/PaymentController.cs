﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;
using Kwt.PatientsMgtApp.WebUI.Models;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using PagedList;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "PatientMgtException")]
    public class PaymentController : BaseController
    {
        private const int PageSize = 2;
        // GET: Patient
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IPayRateRepository _payRateRepository;
        // private readonly IPaymentDeductionRepository _paymentDeductionRepository;
        public PaymentController()
        {
            _paymentRepository = new PaymentRepository();
            _beneficiaryRepository = new BeneficiaryRepository();
            _payRateRepository = new PayRateRepository();
            //   _paymentDeductionRepository = new PaymentDeductionRepository();

        }
        // GET: Companion
        public ActionResult List(string searchpaymentText, string currentFilter, int? page, bool? clearSearch)
        {
            int pageNumber = (page ?? 1);
            //ViewBag.isBeneficiary = isBeneficiary ?? false;
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateInSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            //ViewBag.CidSortParm = String.IsNullOrEmpty(sortOrder) ? "Cid" : "";
            //ViewBag.BeneficiarySortParm = String.IsNullOrEmpty(sortOrder) ? "Beneficiary" : "";

            var payments = _paymentRepository.GetPayments();

            if (payments != null)
            {
                payments = payments.OrderByDescending(c => c.CreatedDate).ToList();
                //switch (sortOrder)
                //{
                //    case "name_desc":
                //        payments = payments.OrderBy(c => c.CompanionFName).ThenBy(c => c.CompanionLName).ToList();
                //        break;
                //    case "cid":
                //        payments = payments.OrderBy(c => c.PatientCID).ToList();
                //        break;
                //    case "date_desc":
                //        payments = payments.OrderBy(c => c.PaymentDate).ToList();
                //        break;
                //    //case "Beneficiary":
                //    //    payments = payments.OrderBy(c => c.IsBeneficiary).ToList();
                //    //    break;
                //    default: // created date ascending 
                //        payments = payments.OrderByDescending(c => c.CreatedDate).ToList();
                //        break;
                //}
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
                    DateTime date = new DateTime();
                    if (!String.IsNullOrEmpty(searchpaymentText))
                    {
                        var term = searchpaymentText.ToLower().Trim();
                        result = payments?
                            .Where(p =>
                                p.PatientCID.ToLower().Trim().Contains(term)
                            ).ToList();
                        if (DateTime.TryParse(searchpaymentText, out date))
                        {
                            result = payments?
                            .Where(p =>
                                p.CreatedDate.Date == date.Date
                            ).ToList();
                        }
                    }

                    if (result?.Count > 0)
                    {
                        Success(string.Format("We have <b>{0}</b> returned results from the searched criteria", result.Count),
                            true);
                        //return View(result.ToPagedList(pageNumber, PageSize));
                        return View(result);
                    }
                    if (!String.IsNullOrEmpty(searchpaymentText))
                    {
                        if (result?.Count == 0)
                        {
                            Information(
                                 string.Format(
                                     "There is no Payment in our records with the selected search criteria <b>{0}</b>",
                                     searchpaymentText), true);
                        }
                    }
                }
            }

            //return View(payments.ToPagedList(pageNumber, PageSize));
            return View(payments);
        }
        //new February 28, 2019
        public ActionResult GetTodaysPayments()
        {

            return RedirectToAction("List");
        }
        //
        [ExceptionHandler]
        public ActionResult Details(int paymentId)
        {

            var payment = _paymentRepository.GetPaymentById(paymentId);


            if (payment != null)
            {

                //new
                //     payment.PaymentDeductionModel = _paymentDeductionRepository.GetPaymentDeductionByPaymentId(paymentId);

                //
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
                        "There is No patient in our records with this CID <b>{0}</b> Or This patient is Not Active! Please Enter a Valid CID",
                        patientCid), true);
            }
            if (!String.IsNullOrEmpty(patientCid)
                && payment.PatientCID != null
                && !payment.IsActive)
            {

                Information(
                    String.Format(
                        "The patient with CID <b>{0}</b> is Not Active, You cannot add a payment to this patient",
                        patientCid), true);
            }

            return View(payment);
        }
        [ExceptionHandler]
        [HttpPost]
        public ActionResult Create(PaymentModel payment)
        {
            ValidatePayment(payment);
            if (ModelState.IsValid)
            {
                payment.CreatedBy = User.Identity.Name;
                _paymentRepository.AddPayment(payment);
                Success(string.Format("Payment for Patient with Civil ID <b>{0}</b> was successfully added.", payment.PatientCID), true);
                return RedirectToAction("List");
            }

            else
            {
                Danger(string.Format("Please correct the error list before proceeding"), true);
                //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
                //companion.Banks = _patientManagmentRepository.GetBanks();
                payment = _paymentRepository.GetPaymentObject(payment.PatientCID);
                return View(payment);
            }

        }

        [ExceptionHandler]
        public ActionResult Edit(int paymentId)
        {
            PaymentViewModel paymentView = new PaymentViewModel();
            var payment = _paymentRepository.GetPaymentById(paymentId);
            payment.PayRates = _payRateRepository.GetPayRatesList();
            paymentView.Payment = payment;

            //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
            //companion.Banks = _patientManagmentRepository.GetBanks();
            return View(paymentView);
        }
        [HttpPost]
        [ExceptionHandler]
        public ActionResult Edit(PaymentViewModel pay)
        {
            // ValidatePayment(payment, true);
            //foreach (ModelState modelState in ViewData.ModelState.Values)
            //{
            //    var test = false;
            //    if (modelState.Errors.Count > 0)
            //    {
            //         test = true;
            //        foreach (ModelError error in modelState.Errors)
            //        {
            //            var message = error.ErrorMessage;
            //        }
            //    }
            //}
            
            if (ModelState.IsValid)
            {
                pay.Payment.ModifiedBy = User.Identity.Name;
                _paymentRepository.UpdatePayment(pay.Payment);
                Success(string.Format("Payment for patient with  CId <b>{0}</b> was successfully updated.", pay.Payment.PatientCID), true);
                return RedirectToAction("Details", "Payment", new { paymentId = pay.Payment.Id });
            }
            else
            {
                pay.Payment = _paymentRepository.GetPaymentById(pay.Payment.Id);
                pay.Payment.PayRates = _payRateRepository.GetPayRatesList();
                Information(string.Format("Payment for patient with  CId  <b>{0}</b> Was Not updated.", pay.Payment.PatientCID), true);
                //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
                //companion.Banks = _patientManagmentRepository.GetBanks();
                return View(pay.Payment);
            }
        }

        [ExceptionHandler]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(int? paymentId)
        {
            PaymentModel payment = null;
            if (paymentId != null)
            {
                var id = paymentId ?? 0;
                payment = _paymentRepository.GetPaymentById(id);
            }
            if (payment != null)
            {
                var deleted = _paymentRepository.DeletePayment(payment);
                if (deleted > 0)
                {
                    // display delete message success and redirect to patient list
                    Success(
                        string.Format("Payment with Id <b>{0}</b> was Successfully Deleted.", payment.Id),
                        true);
                }
            }
            else
            {
                if (paymentId != null)
                    Information(string.Format("No Payment with Id <b>{0}</b> In our record.", paymentId), true);
            }
            return RedirectToAction("List");
        }
        private void ValidatePayment(PaymentModel payment, bool isEdit = false)
        {
            //         --When payrateid = 1, for the same payment period, means both patient and compnaion get paid
            //--When payrateid = 2, for the same payment period, means  patient get paid and not the compnaion
            //  --When payrateid = 3, for the same payment period, means the companion get paid and not the patient
            ValidatePaymentDates(payment);
            var historyPaments = _paymentRepository.GetPaymentsByPatientCid(payment.PatientCID);
            CheckIfPaymentAlreadyMade(payment, historyPaments);
            CheckIfDuplicatePayment(payment, historyPaments, isEdit);

            //check for same dates with different total and different payrates
            //in this case payment should not be blocked, 'cause patient can be paid sparatly than companion but in the same period
            CheckIfNoDuplicatePayment(payment, historyPaments, isEdit);
        }

        private void ValidatePaymentDates(PaymentModel payment)
        {
            if (ModelState.IsValidField("PaymentStartDate") && ModelState.IsValidField("PaymentEndDate"))
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
            //
            if (ModelState.IsValidField("PatientPayRate") && ModelState.IsValidField("CompanionPayRate"))
            {
                if (payment.PatientPayRate == payment.CompanionPayRate && payment.PatientPayRate == 0)
                {
                    ModelState.AddModelError("PatientPayRate", "Patient Pay Rate and Companion Pay Rate cannot both be zero");
                    ModelState.AddModelError("CompanionPayRate", "Companion Pay Rate and Patient Pay Rate cannot both be zero");
                }
            }
        }

        private void CheckIfPaymentAlreadyMade(PaymentModel payment, List<PaymentModel> historyPaments)
        {

            var paymentMadeForBoth = historyPaments.Any(p => p.PaymentStartDate == payment.PaymentStartDate &&
                                     p.PaymentEndDate == payment.PaymentEndDate
                                  && p.PayRateID == 1);//when payrateid==1, means payment was made for both
            if (paymentMadeForBoth)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient and companion already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient and companion already has a payment made with this end date");
            }
            var paymentMadeForPatient = historyPaments.Any(p => p.PaymentStartDate == payment.PaymentStartDate &&
                                    p.PaymentEndDate == payment.PaymentEndDate
                                 && p.PayRateID == 2 && payment.PatientPayRate == 75);//When payrateid = 2, for the same payment period, means  patient get paid and not the compnaion
            if (paymentMadeForPatient)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with the same rate and with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with the same rate and with this end date");
            }
            var paymentMadeForCompanion = historyPaments.Any(p => p.PaymentStartDate == payment.PaymentStartDate &&
                                    p.PaymentEndDate == payment.PaymentEndDate
                                 && p.PayRateID == 3 && payment.PatientPayRate == 25);//When payrateid = 3, for the same payment period, means the companion get paid and not the patient
            if (paymentMadeForCompanion)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Companion already has a payment made with the same rate and with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Companion already has a payment made with the same rate and with this end date");
            }
        }

        private void CheckIfDuplicatePayment(PaymentModel payment, List<PaymentModel> historyPaments, bool isEdit)
        {
            var duplicatPayment = historyPaments
                                .Any(p => p.PaymentStartDate == payment.PaymentStartDate &&
                                    p.PaymentEndDate == payment.PaymentEndDate &&
                                    p.PatientPayRate == payment.PatientPayRate &&
                                    p.CompanionPayRate == payment.CompanionPayRate
                                    //check amount due if the same aswell
                                    && p.TotalDue == payment.TotalDue);
            if (!isEdit && duplicatPayment)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with this end date");
                //total due should not be the same for the same dates
                if (ModelState.IsValidField("TotalDue"))

                    ModelState.AddModelError("TotalDue", "Patient already has a payment with the same amount made with these start and end dates");
            }
            var duplicatCompanionPayment = historyPaments
                                .Any(p => p.PaymentStartDate == payment.PaymentStartDate &&
                                    p.PaymentEndDate == payment.PaymentEndDate &&
                                    p.CompanionPayRate == payment.CompanionPayRate
                                    //check amount due if the same aswell
                                    && p.TotalDue == payment.TotalDue);
            if (!isEdit && duplicatCompanionPayment)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with this end date");
                //total due should not be the same for the same dates
                if (ModelState.IsValidField("TotalDue"))

                    ModelState.AddModelError("TotalDue", "Patient already has a payment with the same amount made with these start and end dates");
            }
            var duplicatPatientPayment = historyPaments
                                .Any(p => p.PaymentStartDate == payment.PaymentStartDate &&
                                    p.PaymentEndDate == payment.PaymentEndDate &&
                                    p.PatientPayRate == payment.PatientPayRate
                                    //check amount due if the same aswell
                                    && p.TotalDue == payment.TotalDue);
            if (!isEdit && duplicatPatientPayment)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with this end date");
                //total due should not be the same for the same dates
                if (ModelState.IsValidField("TotalDue"))

                    ModelState.AddModelError("TotalDue", "Patient already has a payment with the same amount made with these start and end dates");
            }
        }

        private void CheckIfNoDuplicatePayment(PaymentModel payment, List<PaymentModel> historyPaments, bool isEdit)
        {
            var isNotduplicatPayment = historyPaments
                               .Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                       && p.PaymentEndDate == payment.PaymentEndDate)
                                       && p.TotalDue != payment.TotalDue);
            if (isNotduplicatPayment)//same dates, different total
            {
                //check if the payrate for each payee is different than our history records
                var samePatientPayRates = historyPaments
                               .Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                       && p.PaymentEndDate == payment.PaymentEndDate
                                       && p.PatientPayRate == payment.PatientPayRate));
                if (!isEdit && samePatientPayRates)
                {
                    ModelState.AddModelError("PatientPayRate", "Our records indicates that there is payment conflict with this Patient Pay Rate");
                }
                var sameCompanionPayRates = historyPaments
                               .Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                       && p.PaymentEndDate == payment.PaymentEndDate
                                       && p.CompanionPayRate == payment.CompanionPayRate));
                if (!isEdit && samePatientPayRates && payment.CompanionPayRate != null)
                {
                    ModelState.AddModelError("CompanionPayRate", "Our records indicates that there is payment conflict with this Companion Pay Rate");
                }

            }
            if (!isNotduplicatPayment)
            {
                var stratDateConflictPayment = historyPaments
                                   .Any(p => (p.PaymentStartDate <= payment.PaymentStartDate
                                           && payment.PaymentStartDate <= p.PaymentEndDate)
                                           || (payment.PaymentEndDate <= p.PaymentEndDate
                                           && payment.PaymentStartDate >= p.PaymentStartDate));
                if (!isEdit && stratDateConflictPayment)
                {
                    ModelState.AddModelError("PaymentStartDate", "Our records indicates that there is payment conflict with this payment start date");
                }
                var endDateConflictPayment = historyPaments
                                   .Any(p => (payment.PaymentEndDate <= p.PaymentEndDate
                                           && payment.PaymentStartDate >= p.PaymentStartDate) ||
                                           (payment.PaymentEndDate <= p.PaymentEndDate
                                           && payment.PaymentStartDate < p.PaymentStartDate));

                if (!isEdit && endDateConflictPayment)
                {
                    ModelState.AddModelError("PaymentEndDate", "Our records indicates that there is payment conflict with this payment end date");
                }
            }
        }

    }
}