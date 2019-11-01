using System;
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
using Kwt.PatientsMgtApp.WebUI.Infrastructure;
using Kwt.PatientsMgtApp.WebUI.Models;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using PagedList;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    //[CustomAuthorize(Roles = "Admin, Manager, Super Admin, User, Accountant, Auditor")]
    [CustomAuthorize(Roles = CrudRoles.PaymentCrudRolesForAutorizeAttribute)]
    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "ExceptionHandler")]
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
                                p.PatientCID.ToLower().Trim().Contains(term)|| p.PaymentID.ToString().Contains(term)
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

            //for auditor roles, show nonapproved payment
            if (User.IsInAnyRoles2(CrudRoles.PaymentApprovalRoles))
            {
                payments = payments?.Where(p => p.IsApproved != true).ToList();// get only nonApproved payment
            }
            return View(payments);
        }
        //new February 28, 2019
        public ActionResult GetTodaysPayments()
        {

            return RedirectToAction("List");
        }
        //
        //new June 29, 2019
        [ExceptionHandler]
        public JsonResult GetNextPayment(int? numberOfDays = null)
        {
            var payments = _paymentRepository.GetNextPatientPayment(numberOfDays);//-80 2019-08-09 
            //var paymentEndDate = payments.Select(p => p.PaymentEndDate).SingleOrDefault(); //2019-08-09
            //var patientCidList = payments.Select(p => p.PatientCID).ToList();// cid 40 patients
            ////total 40   =>39
            //var pays = _paymentRepository.GetPayments();  // total payments

            //var payMatches = from pay in pays
            //              where patientCidList.Contains(pay.PatientCID)
            //              select pay;

            //var pa = from pay in payMatches
            //         where pay.PaymentEndDate> paymentEndDate
            //         select pay;


            // pays.Where(p=>p.PatientCID )
            // l
            //pays.Select(p=>p.PatientCID).ToList().().

            // MaxJsonLength = Int32.MaxValue
            return Json(payments, JsonRequestBehavior.AllowGet);
        }

        [ExceptionHandler]
        [CustomAuthorize(Roles = CrudRoles.PaymentApprovalRolesForAutorizeAttribute)]
        public JsonResult RefreshPaymentListAfterApproval(int? paymentId)
        {
            PaymentModel payment = null;
            if (paymentId != null)
            {
                var id = paymentId ?? 0;
                payment = _paymentRepository.GetPaymentById(id);
                if (payment != null)
                {
                    payment.IsApproved = true;
                    //Edit(payment);
                    _paymentRepository.UpdateApprovedPayment(payment);
                }
            }
            var payments = _paymentRepository.GetPayments();
            var unApprovedPayment = payments.Where(p => p.IsApproved != true).OrderByDescending(p => p.CreatedDate).ToList();
            var jsonResult = new JsonResult();// Json(payments.Where(p => p.IsApproved == false), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = unApprovedPayment;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
             return jsonResult;
        }
        [ExceptionHandler]
        public JsonResult ToggleApprovedPayment(int? toggleType)
        {
            var payments = _paymentRepository.GetPayments();
            
            List<PaymentModel> unApprovedPayment = new List<PaymentModel>();
            if (toggleType != null)
            {
                if (toggleType == 2) // not approved
                {
                    unApprovedPayment = payments.Where(p => p.IsApproved != true).OrderByDescending(p=>p.CreatedDate).ToList();// get null and false isapproved
                }
                else // just approved Payment
                {
                    unApprovedPayment = payments.Where(p => p.IsApproved == true).OrderByDescending(p => p.CreatedDate).ToList();
                }
                    
            }
             
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = unApprovedPayment;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        [ExceptionHandler]
        public JsonResult GetApprovedPayment(int? id)
        {
            var payments = _paymentRepository.GetPayments();
            if (id != null)
            {
                if (id == 2) // not approved
                {
                    payments = payments.Where(p => p.IsApproved != true).OrderByDescending(p => p.CreatedDate).ToList();// get null and false isapproved
                }
                else // just approved Payment
                {
                    payments = payments.Where(p => p.IsApproved == true).OrderByDescending(p => p.CreatedDate).ToList();
                }

            }
            
           // payments = payments.Where(p => p.IsApproved == true).OrderByDescending(p => p.CreatedDate).ToList();
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payments;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
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
                return RedirectToAction("List");// View("List");
            }
        }


        [ExceptionHandler]
        [HttpGet]
        [CustomAuthorize(Roles = CrudRoles.PaymentCreateRolesForAutorizeAttribute)]
        public ActionResult Create(string patientCid)
        {
            PaymentModel payment = new PaymentModel();
            payment = patientCid != null ? _paymentRepository.GetPaymentObject(patientCid) : _paymentRepository.GetPaymentObject();
            if (!String.IsNullOrEmpty(patientCid) &&
                (payment.PatientCID == null))
            {

                Information(
                    String.Format(
                        "There is No patient in our records with this CID <b>{0}</b> Please Enter a Valid CID",
                        patientCid), true);
            }
            else if (!String.IsNullOrEmpty(patientCid) &&
                 (payment.BeneficiaryCID == null))
            {

                Information(
                    String.Format(
                        "There is No Beneficiary with this patient <b>{0}</b> You can't make payment to this patient",
                        patientCid), true);
            }
            else if (!String.IsNullOrEmpty(patientCid)
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
        [CustomAuthorize(Roles = CrudRoles.PaymentCreateRolesForAutorizeAttribute)]
        public ActionResult Create(PaymentModel payment)
        {
            // allow the supper admin to make any payment without validating the payment
            if (!User.IsInAnyRoles2(CrudRoles.AdminCrudRoles))
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
                var paymentObj = _paymentRepository.GetPaymentObject(payment.PatientCID);
                paymentObj.PaymentEndDate = payment.PaymentEndDate;
                paymentObj.PaymentStartDate = payment.PaymentStartDate;
                return View(paymentObj);
            }

        }

        [ExceptionHandler]
        [CustomAuthorize(Roles = CrudRoles.PaymentUpdateRolesForAutorizeAttribute)]
        public ActionResult Edit(int paymentId)
        {
            // PaymentViewModel paymentView = new PaymentViewModel();
            var payment = _paymentRepository.GetPaymentById(paymentId);
            if (payment==null)
            {
                Warning(string.Format("There is no payment id with {0}",paymentId), true);
                return RedirectToAction("List");
            }
            if (payment.IsVoid == true)
            {
                return RedirectToAction("Details", "Payment", new { paymentId = paymentId });
            }
            payment.PayRates = _payRateRepository.GetPayRatesList();
            // paymentView.Payment = payment;

            //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
            //companion.Banks = _patientManagmentRepository.GetBanks();
            return View(payment);
        }
        [HttpPost]
        [ExceptionHandler]
        [CustomAuthorize(Roles = CrudRoles.PaymentUpdateRolesForAutorizeAttribute)]
        public ActionResult Edit(PaymentModel pay)
        {
            
            if (!User.IsInRole(Roles.SuperAdmin))
                ValidatePayment(pay, true);

            if (ModelState.IsValid)
            {
                pay.ModifiedBy = User.Identity.Name;
                pay.PaymentDeductionObject.ModifiedBy = User.Identity.Name;
                _paymentRepository.UpdatePayment(pay);
                Success(string.Format("Payment for patient with  CId <b>{0}</b> was successfully updated.", pay.PatientCID), true);
                return RedirectToAction("Details", "Payment", new { paymentId = pay.Id });
            }
            pay = _paymentRepository.GetPaymentById(pay.Id);
            pay.PayRates = _payRateRepository.GetPayRatesList();
            Information(string.Format("Payment for patient with  CId  <b>{0}</b> Was Not updated.", pay.PatientCID), true);
            //companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
            //companion.Banks = _patientManagmentRepository.GetBanks();
            return View(pay);
        }

        [ExceptionHandler]
        //[Authorize(Roles = "Super Admin, Admin, Manager")]
        [CustomAuthorize(Roles = CrudRoles.PaymentDeleteRolesForAutorizeAttribute)]
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

        [ExceptionHandler]
        //[Authorize(Roles = "Super Admin, Admin, Manager")]
        [CustomAuthorize(Roles = CrudRoles.PaymentDeleteRolesForAutorizeAttribute)]
        public ActionResult VoidPayment(int? paymentId)
        {
            PaymentModel payment = null;
            if (paymentId != null)
            {
                var id = paymentId ?? 0;
                payment = _paymentRepository.GetPaymentById(id);
            }
            if (payment != null)
            {
                var upDatedPayment = _paymentRepository.VoidPayment(payment);
                if (upDatedPayment.IsVoid == true)
                {
                    // display delete message success and redirect to patient list
                    Success(
                        string.Format("Payment with Id <b>{0}</b> was Successfully Voided.", payment.Id),
                        true);
                }
                else
                {
                    Warning(
                        string.Format("Payment with Id <b>{0}</b> was not Voided.", payment.Id),
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
            //--When payrateid = 1, for the same payment period, means both patient and compnaion get paid
            //--When payrateid = 2, for the same payment period, means  patient get paid and not the compnaion
            //--When payrateid = 3, for the same payment period, means the companion get paid and not the patient
            var historyPayments = _paymentRepository.GetPaymentsByPatientCid(payment.PatientCID)?.Where(p=>p.IsVoid!=true).ToList();
            if (!ValidatePaymentDates(payment, historyPayments, isEdit)) return;
            //new 07-16-2019
            // when payment is a correction or adjustment means we can have duplicate payment dates
            if (payment.PaymentTypeId == (int)Enums.PaymentType.Correction ||
                payment.PaymentTypeId == (int)Enums.PaymentType.Adjustment)
            {
                return;
            }
            // end new

            if (historyPayments.Count > 0)
            {

                if (!CheckIfPaymentAlreadyMade(payment, historyPayments, isEdit)) return;
                if (!CheckIfDuplicatePayment(payment, historyPayments, isEdit)) return;
                //check for same dates with different total and different payrates
                //in this case payment should not be blocked, 'cause patient can be paid sparatly than companion but in the same period
                CheckIfNoDuplicatePayment(payment, historyPayments, isEdit);
            }

        }

        private bool ValidatePaymentDates(PaymentModel payment, List<PaymentModel> paymentHistory, bool isEdit)
        {
            bool isValid = true;
            if (ModelState.IsValidField("PaymentStartDate") && ModelState.IsValidField("PaymentEndDate"))
            {
                //Todo: check for current date as well
                if (DateTime.Parse(payment.PaymentStartDate?.ToString()) >
                    DateTime.Parse(payment.PaymentEndDate?.ToString()))
                {
                    ModelState.AddModelError("PaymentStartDate", "Payment Start Date should not be after payment end Date");
                    ModelState.AddModelError("PaymentEndDate", "Payment End Date should not be before payment end Date");
                    isValid = false;
                }


            }
            //
            if (ModelState.IsValidField("PatientPayRate") && ModelState.IsValidField("CompanionPayRate"))
            {
                if ((payment.PatientPayRate == payment.CompanionPayRate && payment.PatientPayRate == 0)
                    || (payment.CompanionPayRate == null && payment.PatientPayRate == 0))
                {
                    // when we wants to make a deduction without payment, we can have both patient and companion rate set to zero
                    if (payment.PaymentTypeId != (int)Enums.PaymentType.Other)
                        ModelState.AddModelError("PatientPayRate", "Patient Pay Rate and Companion Pay Rate cannot both be zero");
                    ModelState.AddModelError("CompanionPayRate", "Companion Pay Rate and Patient Pay Rate cannot both be zero");
                    isValid = false;
                }
            }
            //new 
            var lastPayment = paymentHistory?.OrderByDescending(p => p.PaymentDate)
                                            .FirstOrDefault(p => p.PaymentTypeId == null
                                            || p.PaymentTypeId == (int)Enums.PaymentType.Regular);
            if (!isEdit && lastPayment?.PaymentEndDate != null)
            {
                var lastEndDate = (DateTime)lastPayment?.PaymentEndDate?.Date;
                if (payment?.PaymentStartDate?.Date != null)
                {
                    var currentStartDate = (DateTime)payment?.PaymentStartDate?.Date;
                    TimeSpan dateDiff = currentStartDate - lastEndDate;
                    //the current start date should not be less than the last payment end date
                    if (dateDiff.Days <= 0)
                    {
                        // new 07-17-2019
                        // sometimes they need to make payment adjustment for some repeated dates old payments
                        if (payment.PaymentTypeId == (int)Enums.PaymentType.Regular)
                            if (ModelState.IsValidField("PaymentStartDate") && ModelState.IsValidField("PaymentEndDate"))
                            {
                                //Todo: check for current date as well
                                ModelState.AddModelError("PaymentStartDate", "Conflict with old payment, Payment Start date should be after last payment end date");
                                ModelState.AddModelError("PaymentEndDate", "Conflict with old payment, Payment End date should be after last payment end date");
                                isValid = false;
                            }
                    }

                }
            }

            // end new
            return isValid;
        }

        private bool CheckIfPaymentAlreadyMade(PaymentModel payment, List<PaymentModel> historyPayments, bool isEdit)
        {
            var isValid = true;

            var paymentMadeForBoth = historyPayments.Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                            && p.PaymentEndDate == payment.PaymentEndDate)
                                                            && (p.PayRateID == 1
                                                            || (p.PatientAmount == payment.PatientAmount
                                                            && p.CompanionAmount == payment.CompanionAmount)));
            if (paymentMadeForBoth && !isEdit)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient and companion already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient and companion already has a payment made with this end date");
                isValid = false;

            }
            var paymentMadeForPatient = historyPayments.Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                               && p.PaymentEndDate == payment.PaymentEndDate) &&
                                                                //When payrateid = 2, for the same payment period, means  patient get paid and not the compnaion
                                                                ((p.PayRateID == 2 && payment.PatientPayRate == 75)
                                                              || (p.PatientAmount == payment.PatientAmount)));
            if (paymentMadeForPatient && !isEdit)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with the same rate and with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with the same rate and with this end date");
                isValid = false;
            }
            var paymentMadeForCompanion = historyPayments.Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                                 && p.PaymentEndDate == payment.PaymentEndDate)
                                                                 &&
                                                                  //When payrateid = 3, for the same payment period, means the companion get paid and not the patient
                                                                  ((p.PayRateID == 3 && payment.CompanionPayRate == 25)
                                                                 || (p.CompanionAmount == payment.CompanionAmount)));
            if (paymentMadeForCompanion && !isEdit)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Companion already has a payment made on this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Companion already has a payment made on this end date");
                isValid = false;
            }
            return isValid;
        }

        private bool CheckIfDuplicatePayment(PaymentModel payment, List<PaymentModel> historyPayments, bool isEdit)
        {
            var isValid = true;

            var duplicatPayment = historyPayments.Any(p => p.PaymentStartDate == payment.PaymentStartDate
                                                        && p.PaymentEndDate == payment.PaymentEndDate
                                                        && p.PatientPayRate == payment.PatientPayRate
                                                        && p.CompanionPayRate == payment.CompanionPayRate
                                                        && ((p.TotalDue == payment.TotalDue)
                                                        || (p.CompanionAmount == payment.CompanionAmount
                                                        && p.PatientAmount == payment.PatientAmount)));
            if (!isEdit && duplicatPayment)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with this end date");
                //total due should not be the same for the same dates
                if (ModelState.IsValidField("TotalDue"))

                    ModelState.AddModelError("TotalDue", "Patient already has a payment with the same amount made with these start and end dates");
                isValid = false;
            }

            var duplicatCompanionPayment = historyPayments
                                                  .Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                          && p.PaymentEndDate == payment.PaymentEndDate)
                                                          && (p.CompanionPayRate == payment.CompanionPayRate
                                                          && p.TotalDue == payment.TotalDue));
            if (!isEdit && duplicatCompanionPayment)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Companion already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Companion already has a payment made with this end date");
                //total due should not be the same for the same dates
                if (ModelState.IsValidField("TotalDue"))
                    ModelState.AddModelError("TotalDue", "Companion already has a payment with the same amount made with these start and end dates");
                isValid = false;
            }
            var duplicatPatientPayment = historyPayments
                                            .Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                    && p.PaymentEndDate == payment.PaymentEndDate)
                                                    && (p.PatientPayRate == payment.PatientPayRate
                                                    && p.TotalDue == payment.TotalDue));
            if (!isEdit && duplicatPatientPayment)
            {
                if (ModelState.IsValidField("PaymentStartDate"))
                    ModelState.AddModelError("PaymentStartDate", "Patient already has a payment made with this start date");
                if (ModelState.IsValidField("PaymentEndDate"))
                    ModelState.AddModelError("PaymentEndDate", "Patient already has a payment made with this end date");
                //total due should not be the same for the same dates
                if (ModelState.IsValidField("TotalDue"))
                    ModelState.AddModelError("TotalDue", "Patient already has a payment with the same amount made with these start and end dates");
                isValid = false;
            }
            return isValid;
        }

        private void CheckIfNoDuplicatePayment(PaymentModel payment, List<PaymentModel> historyPayments, bool isEdit)
        {

            // sometimes we can make payment with the same dates but for different 
            var isNotduplicatPayment = historyPayments.Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                                && p.PaymentEndDate == payment.PaymentEndDate)
                                                                && ((p.TotalDue != payment.TotalDue)
                                                                || (p.CompanionAmount != payment.CompanionAmount
                                                                || p.PatientAmount != payment.PatientAmount)));
            if (isNotduplicatPayment)//same dates, different total
            {
                //check if the payrate for each payee is different than our history records
                var samePatientPayRates = historyPayments.Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                                   && p.PaymentEndDate == payment.PaymentEndDate
                                                                   && p.PatientPayRate == payment.PatientPayRate));
                if (!isEdit && samePatientPayRates)
                {
                    ModelState.AddModelError("PatientPayRate", "Our records indicates that there is payment conflict with this Patient Pay Rate");

                }
                var sameCompanionPayRates = historyPayments.Any(p => (p.PaymentStartDate == payment.PaymentStartDate
                                                                    && p.PaymentEndDate == payment.PaymentEndDate
                                                                    && p.CompanionPayRate == payment.CompanionPayRate));
                if (!isEdit && sameCompanionPayRates && payment.CompanionPayRate != null)
                {
                    ModelState.AddModelError("CompanionPayRate", "Our records indicates that there is payment conflict with this Companion Pay Rate");

                }

            }
            else
            {           // check if the start date is between the history start date and end date   history.startDate  <= new.startDate <= history.endDate
                        //history.endDate  <= new.EndDate <= history.startDate
                var startDateConflictPayment = historyPayments.Any
                    (p => (payment.PaymentStartDate <= p.PaymentEndDate) && (payment.PaymentEndDate >= p.PaymentStartDate));

                if (!isEdit && startDateConflictPayment)
                {
                    var duplicateList =
                        historyPayments.Where(
                            p =>
                                (payment.PaymentStartDate <= p.PaymentEndDate) &&
                                (payment.PaymentEndDate >= p.PaymentStartDate)).ToList();
                    //add error only when
                    if (payment.CompanionCID != null
                        && duplicateList.Any(dp => (dp.CompanionCID == payment.CompanionCID)
                        && payment.CompanionAmount > 0
                        && dp.CompanionAmount > 0
                        //&& ( dp.CompanionAmount== payment.CompanionAmount )
                        ))// trying to pay companion that was already paid
                    {
                        ModelState.AddModelError("PaymentStartDate", "Conflicts with old payment, This Companion already got paid with this start date, please check history payments");
                        ModelState.AddModelError("PaymentEndDate", "Conflicts with old payment, This Companion already got paid with this end date, please check history payments");
                    }
                    if (payment.PatientAmount > 0
                        && duplicateList.Any(dp => (dp.PatientAmount > 0)))// patient already got paid in the conflicted period
                    {
                        ModelState.AddModelError("PaymentStartDate", "Conflicts with old payment, This Patient already got paid with this start date, please check history payment");
                        ModelState.AddModelError("PaymentEndDate", "Conflicts with old payment, This Patient already got paid with this start date, please check history payment");
                    }
                }
            }

        }

    }
}