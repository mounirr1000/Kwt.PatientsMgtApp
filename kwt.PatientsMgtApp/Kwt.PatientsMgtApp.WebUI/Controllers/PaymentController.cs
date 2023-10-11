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
using System.Text;
using Microsoft.Security.Application;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iText.Html2pdf;
using iTextSharp.tool.xml;
using System.Web.UI;
using iTextSharp.text.html.simpleparser;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using Rotativa;
using System.Configuration;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    //[CustomAuthorize(Roles = "Admin, Manager, Super Admin, User, Accountant, Auditor")]
    [CustomAuthorize(Roles = CrudRoles.PaymentCrudRolesForAutorizeAttribute)]
    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "ExceptionHandler")]
    public class PaymentController : BaseController
    {
        // private const int PageSize = 2;
        // GET: Patient
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IPayRateRepository _payRateRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPayrollRepository _payrollRepository;
        private readonly IDepositRepository _depositRepository;
        private readonly IPatientExtensionRepository _petientExtensionRepository;
        public PaymentController()
        {
            _paymentRepository = new PaymentRepository();
            _beneficiaryRepository = new BeneficiaryRepository();
            _payRateRepository = new PayRateRepository();
            _employeeRepository = new EmployeeRepository();
            _payrollRepository = new PayrollRepository();
            _depositRepository = new DepositRepository();
            _petientExtensionRepository = new PatientExtensionRepository();

        }
        #region Patient Payment

        // GET: Companion
        public ActionResult List(string searchpaymentText, string currentFilter, int? page, bool? clearSearch)
        {
            int pageNumber = (page ?? 1);

            //testing sending messages using eztextingManager class
            // EZTextingManager.SendMessages();
            //EZTextingManager.SendSms("5714570919", "Hi there, this is a test from eztexting");

            //
            var payments = _paymentRepository.GetPayments();

            if (payments != null)
            {
                payments = payments.OrderByDescending(c => c.CreatedDate).ToList();

                // get only last 30 days payments to avoid loading too many anneccessary payments

                payments = payments.Where(p => p.IsActive == true)
                                   .Where(p => p.CreatedDate >= DateTime.Now.AddDays(-60))
                                   .OrderByDescending(c => c.CreatedDate).ToList();

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
                                p.PatientCID.ToLower().Trim().Contains(term) || p.PaymentID.ToString().Contains(term)
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
            // try to use json instead, so outofstack exception wont be thrown
            //if (User.IsInAnyRoles2(CrudRoles.PaymentApprovalRoles))
            //{
            //    payments = payments?.Where(p => p.IsApproved != true).ToList();// get only nonApproved payment
            //}
            return View(payments);
        }
        // new 2020
        [HttpGet]
        public JsonResult GetPaymentsJson(string query)
        {
            var payments = _paymentRepository.GetPayments();
            if (!String.IsNullOrEmpty(query))
                payments = payments?
                                .Where(p =>
                                    p.PatientCID.ToLower().Trim().Contains(query) || p.PaymentID.ToString().Contains(query)
                                ).OrderByDescending(c => c.CreatedDate).ToList();
            else
            {
                payments = payments.Where(p => p.IsActive == true)
                                   .Where(p => p.CreatedDate >= DateTime.Now.AddDays(-60))
                                   .OrderByDescending(c => c.CreatedDate).ToList();
            }
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payments;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        //emd new 2020
        // new 2020
        [HttpGet]
        public JsonResult GetPaymentsBetweenTwoDatesJson(string date1, string date2)
        {
            var payments = new List<PaymentModel>();
            if (!String.IsNullOrEmpty(date1) && !String.IsNullOrEmpty(date2))
            {
                payments = _paymentRepository.GetPayments();
                DateTime d1;
                DateTime d2;
                DateTime.TryParse(date1, out d1);
                DateTime.TryParse(date2, out d2);
                payments = payments?
                                .Where(p =>
                                    p.PaymentDate >= d1 && p.PaymentDate <= d2
                                ).OrderByDescending(c => c.CreatedDate).ToList();
            }

            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payments;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        //emd new 2020
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
        public JsonResult RefreshPaymentListAfterApproval(int? paymentId, string selectedPaymentListId)
        {
            PaymentModel payment = null;
            bool sentSuccess = false;
            if (paymentId != null)
            {
                var id = paymentId ?? 0;
                payment = _paymentRepository.GetPaymentById(id);
                if (payment != null)
                {


                    //Edit(payment);

                    // when the payment is approved, send an sms message to the patient about the amount to be paid to him/her
                    StringBuilder message = new StringBuilder();
                    var patientPhone = payment.PatientPhone;
                    var patientCid = payment.PatientCID;
                    var paymentAmount = payment.TotalDue;
                    var startDate = payment.PaymentStartDateFormatted;
                    var endDate = payment.PaymentEndDateFormatted;
                    var paymentDate = payment.PaymentDateFormatted;
                    message.Append(" Greetings, ");
                    message.Append(paymentAmount + " KD");
                    message.Append(" will be sent on " + paymentDate + " to Kuwait for Civil# ");
                    message.Append(patientCid);
                    message.Append(" for the period From " + startDate);
                    message.Append(" To " + endDate);

                    sentSuccess = EZTextingManager.SendMessages("Kuwait Health Office", message.ToString(), patientPhone);
                    if (sentSuccess)
                    {
                        payment.SMSConfirmation = true;
                    }
                    else
                    {
                        payment.SMSConfirmation = false;
                    }
                    payment.IsApproved = true;
                    _paymentRepository.UpdateApprovedPayment(payment);

                }
            }
            var data = new DataWithConfirmation();
            var payments = _paymentRepository.GetPayments();
            if (payments != null)
                payments = payments.Where(p => p.CreatedDate >= DateTime.Now.AddDays(-60)).ToList();
            var unApprovedPayment = payments.OrderByDescending(p => p.CreatedDate).ToList();
            if (selectedPaymentListId == "2")// only get unapproved payment
                unApprovedPayment = unApprovedPayment.Where(p => p.IsApproved != true).OrderByDescending(p => p.CreatedDate).ToList();
            else if (selectedPaymentListId == "1")// only approved payment
                unApprovedPayment = unApprovedPayment.Where(p => p.IsApproved == true).OrderByDescending(p => p.CreatedDate).ToList();

            data.Payments = unApprovedPayment;
            data.Confirmation = sentSuccess;
            var jsonResult = new JsonResult();// Json(payments.Where(p => p.IsApproved == false), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            //jsonResult.Data = unApprovedPayment;
            jsonResult.Data = data;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        [ExceptionHandler]
        public JsonResult ToggleApprovedPayment(int? toggleType)
        {
            var payments = _paymentRepository.GetPayments();
            // get only last 30 days payments to avoid loading too many anneccessary payments
            if (payments != null)
                payments = payments.Where(p => p.CreatedDate >= DateTime.Now.AddDays(-60)).ToList();

            List<PaymentModel> unApprovedPayment = new List<PaymentModel>();
            if (toggleType != null)
            {
                if (toggleType == 2) // not approved
                {
                    unApprovedPayment = payments.Where(p => p.IsApproved != true).OrderByDescending(p => p.CreatedDate).ToList();// get null and false isapproved
                }
                else if (toggleType == 1) //  approved
                {
                    unApprovedPayment = payments.Where(p => p.IsApproved == true).OrderByDescending(p => p.CreatedDate).ToList();
                }
                else // just approved Payment
                {
                    unApprovedPayment = payments.OrderByDescending(p => p.CreatedDate).ToList();
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
            // get only last 30 days payments to avoid loading too many anneccessary payments
            if (payments != null)
                payments = payments.Where(p => p.CreatedDate >= DateTime.Now.AddDays(-30)).ToList();
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
            // patient extension
            payment.SelectedPatientExtensionCID = patientCid;
            if (!String.IsNullOrEmpty(patientCid) &&
                (payment.PatientCID != null))
                payment.Patientfiles = new PatientFileModel[] {
                    GetFolders(payment.PatientCID, payment.PatientFName,PatientFolders.Extensions).PatientFile,
                 };
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
                var voucherId = _paymentRepository.AddPayment(payment);
                if (voucherId > 0)
                {
                    Success(string.Format("Payment for Patient with Civil ID <b>{0}</b> was successfully added.", payment.PatientCID), true);

                    //patient extension
                    //create patient extension history and mark patient extension as paid
                    if (payment.PatientExtension != null)
                    {
                        UpdatePatientExtensionAndCreateHistory(payment.PatientExtension);
                        // then delete the file from the folder providing the file path stored in extension table
                        DeletePatientExtensionFile(payment.PatientExtension.ExtensionDocLink, payment.PatientExtension.FileName, payment);
                    }

                }
                else
                {
                    Danger(string.Format("An Error Has Accured while creating the payment, The Payment was not created Successfully contact your Admin!"), true);
                }

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
            if (payment == null)
            {
                Warning(string.Format("There is no payment id with {0}", paymentId), true);
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
            var historyPayments = _paymentRepository.GetPaymentsByPatientCid(payment.PatientCID)?.Where(p => p.IsVoid != true).ToList();
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
        #endregion


        #region Employee CRUD Operations
        public ActionResult NewEmployee()
        {
            var employee = _employeeRepository.GetEmployeeObject();
            return View(employee);

        }

        [HttpPost]
        public ActionResult NewEmployee(EmployeeModel employee)
        {

            if (ModelState.IsValid)
            {
                employee.CreatedBy = User.Identity.Name;
                //try
                //{
                _employeeRepository.AddEmployee(employee);
                //}
                //catch (NullReferenceException e)
                //{
                //var error = e.Message;
                //}
                Success(string.Format("Employee <b>{0}</b> was successfully added.", employee.EmployeeFName + " " + employee.EmployeeLName), true);
                return RedirectToAction("EmployeeList");
            }
            else
            {
                var emp = _employeeRepository.GetEmployeeObject();
                employee.BonusTypes = emp.BonusTypes;
                employee.EmployeeInsurances = emp.EmployeeInsurances;
                employee.InsuranceOptions = emp.InsuranceOptions;
                employee.InsuranceTypes = emp.InsuranceTypes;
                employee.TaxCategories = emp.TaxCategories;
                employee.SocialStatuses = emp.SocialStatuses;
                employee.PayrollAccounts = emp.PayrollAccounts;
                employee.TitleTypes = emp.TitleTypes;
                return View(employee);
            }

        }
        public ActionResult EditEmployee(int employeeId)
        {
            var employee = _employeeRepository.GetEmployee(employeeId);

            if (employee != null)
            {
                var emp = _employeeRepository.GetEmployeeObject();
                employee.BonusTypes = emp.BonusTypes;
                // employee.EmployeeInsurances = emp.EmployeeInsurances;
                employee.InsuranceOptions = emp.InsuranceOptions;
                employee.InsuranceTypes = emp.InsuranceTypes;
                employee.TaxCategories = emp.TaxCategories;
                employee.SocialStatuses = emp.SocialStatuses;
                employee.PayrollAccounts = emp.PayrollAccounts;
                employee.TitleTypes = emp.TitleTypes;
                return View(employee);
            }
            else
            {
                Information(string.Format("Employee with Id <b>{0}</b> Does Not exist in our records.", employeeId), true);
                return RedirectToAction("EmployeeList");
            }
        }

        [HttpPost]
        public ActionResult EditEmployee(EmployeeModel employee)
        {
            try
            {
                _employeeRepository.UpdateEmployee(employee);
                Success(string.Format("Employee <b>{0}</b> was successfully updated.", employee.EmployeeFName + " " + employee.EmployeeLName), true);
            }
            catch (Exception ex)
            {
                Danger(string.Format("An error accurs during updated <b>{0}</b> Please fix the issue to update.", ex.Message), true);
            }
            return RedirectToAction("EmployeeList"); //View();
        }
        public ActionResult EmployeeDetails(int employeeId)
        {
            var employee = _employeeRepository.GetEmployee(employeeId);

            if (employee != null)
            {
                var emp = _employeeRepository.GetEmployeeObject();
                employee.BonusTypes = emp.BonusTypes;
                // employee.EmployeeInsurances = emp.EmployeeInsurances;
                employee.InsuranceOptions = emp.InsuranceOptions;
                employee.InsuranceTypes = emp.InsuranceTypes;
                employee.TaxCategories = emp.TaxCategories;
                employee.SocialStatuses = emp.SocialStatuses;
                employee.PayrollAccounts = emp.PayrollAccounts;
                employee.TitleTypes = emp.TitleTypes;
                return View(employee);
            }
            else
            {
                Information(string.Format("Employee with Id <b>{0}</b> Does Not exist in our records.", employeeId), true);
                return RedirectToAction("EmployeeList");
            }
        }
        public ActionResult DeleteEmployee()
        {
            return View();
        }
        public ActionResult EmployeeList()
        {
            var emps = _employeeRepository.GetEmployees();
            return View(emps);
        }

        [HttpGet]
        public JsonResult GetEmployeeListJson(string query)
        {
            var emps = _employeeRepository.GetEmployees();
            //if (!String.IsNullOrEmpty(query))
            //{
            //    emps = emps?.Where(p =>
            //                                      p.EmployeeID.ToString().Contains(query) ||
            //                                      p.EmployeeName.ToLower().Contains(query.ToLower()) ||
            //                                      p.Title.Title1.ToLower().Contains(query.ToLower())
            //                                      ).OrderBy(c => c.EmployeeID).ThenByDescending(e => e.EmployeeName).ToList();
            //}

            //else
            //{
            //    emps = emps.OrderBy(c => c.EmployeeID).ThenByDescending(e => e.EmployeeName).ToList();
            //}
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = emps;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion


        #region Employees payments
        public ActionResult EmployeePayments()
        {
            return RedirectToAction("EmployeeList");// View();
        }
        public ActionResult EmployeeRegular()
        {
            var emps = _employeeRepository.GetEmployees();
            var employeesViewModel = new EmployeesPaymentViewModel();
            employeesViewModel.EmployeeList = emps;
            return View(employeesViewModel);
        }

        [HttpPost]
        public ActionResult EmployeeRegular(EmployeesPaymentViewModel employeesModel)
        {
            List<int> TagIds = employeesModel.EmployeeSelecedIds?.Split(',').Select(int.Parse).ToList();
            var emps = _employeeRepository.GetEmployees();
            employeesModel.EmployeeList = emps;

            List<EmployeeModel> selectedEmployees = emps.Where(x => TagIds.Contains(x.EmployeeID)).ToList();

            // createRegularPayroll
            if (selectedEmployees.Count > 0)
            {
                foreach (var emp in selectedEmployees)
                {
                    emp.CreatedBy = (string)Session["FullName"];// User.Identity.Name;
                }
                _employeeRepository.CreateEmployeeRegularPayment(selectedEmployees);
                // if (payRollList.Count > 0)
                //{
                Success(string.Format("The Regual Payments were successfully submitted"), true);
                //}
            }

            return RedirectToAction("Payrolls");
        }

        #endregion


        #region Others payments
        public ActionResult OthersPayments()
        {
            return View();
        }
        #endregion

        #region Payrolls
        public ActionResult Payrolls()
        {
            var payroll = _payrollRepository.GetPayrollList();
            return View(payroll);
        }
        [HttpGet]
        public ActionResult NewPayroll()
        {
            var payrollObject = _payrollRepository.GetPayrollObject();
            return View(payrollObject);
        }
        [HttpPost]
        public ActionResult NewPayroll(PayrollModel payroll)
        {
            if (ModelState.IsValid)
            {

                payroll.CreatedBy = (string)Session["FullName"];// ViewData.ContainsKey("FullName")? (string) ViewData["FullName"]: User.Identity.Name;
                payroll.PaymentEnteredBy = (string)Session["FullName"];// ViewData.ContainsKey("FullName") ? (string)ViewData["FullName"] : User.Identity.Name;
                payroll.PaymentCreatedBy = (string)Session["FullName"];// ViewData.ContainsKey("FullName") ? (string)ViewData["FullName"] : User.Identity.Name;
                _payrollRepository.NewPayroll(payroll);
                return View("Payrolls");
            }
            else
            {
                var payrollObject = _payrollRepository.GetPayrollObject();
                payroll.Accounts = payrollObject.Accounts;
                payroll.PayeeList = payrollObject.PayeeList;
                payroll.Agencies = payrollObject.Agencies;
                payroll.PayrollModethodList = payrollObject.PayrollModethodList;
                return View(payroll);
            }


        }
        public JsonResult GetPayrollsJson(string query)
        {
            var payroll = _payrollRepository.GetPayrollList();
            switch (query)
            {
                case "entered":
                    payroll = payroll?
                                .Where(p =>
                                    p.PayrollStatusID == 1
                                ).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "approved":
                    payroll = payroll?
                                .Where(p =>
                                    p.PayrollStatusID == 2
                                ).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "authorized":
                    payroll = payroll?
                                .Where(p =>
                                    p.PayrollStatusID == 3
                                ).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "paid":
                    payroll = payroll?
                                .Where(p =>
                                    p.PayrollStatusID == 4
                                ).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "reconciled":
                    payroll = payroll?
                                .Where(p =>
                                    p.PayrollStatusID == 5
                                ).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                default:
                    payroll = payroll.OrderByDescending(c => c.TransactionID).ToList();
                    break;
            }

            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payroll;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult SearchPayrollsJson(string query, string selectedList)
        {
            var payroll = _payrollRepository.GetPayrollList();
            switch (selectedList)
            {
                case "entered":
                    payroll = payroll?.Where(p => p.PayrollStatusID == 1).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "approved":
                    payroll = payroll?.Where(p => p.PayrollStatusID == 2).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "authorized":
                    payroll = payroll?.Where(p => p.PayrollStatusID == 3).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "paid":
                    payroll = payroll?.Where(p => p.PayrollStatusID == 4).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                case "reconciled":
                    payroll = payroll?.Where(p => p.PayrollStatusID == 5).OrderByDescending(c => c.TransactionID).ToList();
                    break;
                default:
                    payroll = payroll.OrderByDescending(c => c.TransactionID).ToList();
                    break;
            }
            if (!string.IsNullOrEmpty(query))
                payroll = payroll?
                            .Where(p => (p.TransactionID.ToString().Contains(query.Trim()) ||
                                         p.PayeeID != null ? p.PayeeID.ToString().Contains(query.Trim()) : p.EmployeeID.ToString().Contains(query.Trim()) ||
                                         p.PaymentEnteredBy.Trim().ToLower().ToString().Contains(query.Trim().ToLower()) ||
                                         p.PayeeName.ToLower().Trim().ToString().Contains(query.Trim().ToLower()))
                                         ).OrderByDescending(c => c.TransactionID).ToList();
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payroll;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult ApprovePayrollJson(int id, int payrollStatusId)
        {
            var pay = _payrollRepository.GetPayroll(id);
            pay.UpdatedBy = (string)Session["FullName"];// User.Identity.Name; 

            // generate check when the payrollstatus is 3
            //if (payrollStatusId == 3)
            //  Export(GenerateCheckHTML(pay.Amount.ToString(), NumberToWords((int)pay.Amount), pay.PayeeName, pay.Payee.PayeeStreetAddress, pay.Descriptions, DateTime.Now.Date.ToString()));
            //
            _payrollRepository.UpdatePayrollStatus(pay, payrollStatusId);
            var payroll = _payrollRepository.GetPayrollList();

            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payroll.Where(p => p.PayrollStatusID == payrollStatusId).ToList();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ApproveSelectedPayrollJson(int[] selectedIds, int payrollStatusId)
        {
            _payrollRepository.UpdatePayrollsStatus(selectedIds, payrollStatusId, (string)Session["FullName"]);
            var payroll = _payrollRepository.GetPayrollList();

            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payroll.Where(p => p.PayrollStatusID == payrollStatusId).ToList();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult ResetPayrollJson(int id, int payrollStatusId)
        {
            var pay = _payrollRepository.GetPayroll(id);

            _payrollRepository.ResetPayroll(pay, payrollStatusId);
            var payroll = _payrollRepository.GetPayrollList();
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payroll.Where(p => p.PayrollStatusID == payrollStatusId).ToList();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult DeletePayrollJson(int id, int payrollStatusId)
        {

            _payrollRepository.DeletePayroll(id);
            var payroll = _payrollRepository.GetPayrollList();
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = payroll.Where(p => p.PayrollStatusID == payrollStatusId).ToList();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }

        public JsonResult UpdatedPayrollAccountJson(int accountNumber, int payAccountID, decimal amount, string descriptions,
                                                    decimal discount, int payrollID, int discountTypeId, string discountTypeName)
        {
            AccountModel account = new AccountModel()
            {
                PayrollAccountID = payAccountID,
                AccountNumber = accountNumber,
                Amount = amount,
                Descriptions = Sanitizer.GetSafeHtmlFragment(descriptions),
                Discount = discount,
                PayrollID = payrollID,
                DiscountTypeId = discountTypeId,
                DiscountTypeName = discountTypeName
            };
            var updatedAccount = _payrollRepository.UpdatePayrollAccount(account);
            var accounts = _payrollRepository.GetPayrollAccountList(account.PayrollID ?? 0);
            account.Accounts = accounts;
            account.TotalAmount = updatedAccount.TotalAmount;
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = account;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public JsonResult DeletePayrollAccountJson(int accountNumber, int payrollId)
        {

            _payrollRepository.DeletePayrollAccount(accountNumber);
            var accounts = _payrollRepository.GetPayrollAccountList(payrollId);
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = accounts;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult PayrollDetails(int payrollId)
        {
            var payroll = _payrollRepository.GetPayroll(payrollId);
            return View(payroll);
        }

        public ActionResult EditPayroll(int payrollId)
        {
            var payroll = _payrollRepository.GetPayroll(payrollId);
            return View(payroll);
        }

        [HttpPost]
        public ActionResult EditPayroll(PayrollModel payroll)
        {
            if (ModelState.IsValid)
            {

                payroll.PaymentModifiedBy = (string)Session["FullName"];// ViewData.ContainsKey("FullName")? (string) ViewData["FullName"]: User.Identity.Name;                                
                _payrollRepository.UpdatePayroll(payroll);
                return View("Payrolls");
            }
            else
            {
                return View(payroll);
            }
        }
        public ActionResult AccountEntryRow()
        {
            var account = _payrollRepository.GetPayrollObject();

            return PartialView("_NewPayrollAccountEntryEditor", account.Account);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export(string GridHtml)
        {
            GridHtml = GenerateCheckHTML("3212", "three thousand twelve", "Hamid", "6161 edsall rd Alexandria va", "this a test check", "09/09/2020");
            //using (MemoryStream stream = new System.IO.MemoryStream())
            //{
            //    StringReader sr = new StringReader(GridHtml);
            //    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
            //    pdfDoc.Open();
            //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //    pdfDoc.Close();

            //    return File(stream.ToArray(), "application/pdf", "Check.pdf");
            //}


            int voucherNumber = 44;
            var a = new ViewAsPdf();

            a.ViewName = "_PrintedCheck";
            a.Model = _payrollRepository.GetPayroll(voucherNumber);
            var pdfBytes = a.BuildFile(ControllerContext);

            // Optionally save the PDF to server in a proper IIS location.
            //var fileName = string.Format("Check_{0}.pdf", voucherNumber);
            //var path = Server.MapPath("~/App_Data/" + fileName);
            //System.IO.File.WriteAllBytes(path, pdfBytes);

            // return ActionResult
            MemoryStream ms = new MemoryStream(pdfBytes);
            return File(ms.ToArray(), "application/pdf", "Check.pdf");
            // return new FileStreamResult(ms, "application/pdf");
            //int payrollId = 44;
            //var report = new Rotativa.ActionAsPdf("PayrollDetails", new { payrollId =payrollId});
            //return report;
        }

        public ActionResult ExportPrintedCheckToPdf(int voucherNumber)
        {
            var payroll = _payrollRepository.GetPayroll(voucherNumber);
            payroll.AlphaAmount = NumWordsWrapper((double)payroll.Amount);
            payroll.CheckAndAccountNumberFormat = "C" + voucherNumber + "C  A054001204A  001921530454C";
            var a = new ViewAsPdf();
            a.ViewName = "_PrintedCheck";
            a.Model = payroll;
            var pdfBytes = a.BuildFile(ControllerContext);
            MemoryStream ms = new MemoryStream(pdfBytes);
            // use the next line to open the partial view instead of dlownload it

            //return new FileStreamResult(ms, "application/pdf");
            return new FileStreamResult(ms, "application/pdf")
            {
                FileDownloadName = "Check_" + voucherNumber + "_.pdf",
            };
            //use the next line to download pdf
            //return File(ms.ToArray(), "application/pdf", "Check_"+ voucherNumber + "_.pdf");

        }
        public PartialViewResult PrintedCheck()
        {
            //var payroll = _payrollRepository.GetPayroll(PayrollId);
            //payroll.AlphaAmount = NumWordsWrapper((double)payroll.Amount);
            return PartialView("_PrintedCheck");
        }
        public PartialViewResult PrintedCheckTest(int PayrollId)
        {
            var payroll = _payrollRepository.GetPayroll(PayrollId);
            payroll.AlphaAmount = NumWordsWrapper((double)payroll.Amount);
            payroll.CheckAndAccountNumberFormat = "C" + PayrollId + "C  A054001204A  001921530454C";

            return PartialView("_PrintedCheck", payroll);
        }
        private string GenerateCheckHTML(string amount, string amountText, string payeeName, string payeeAddress, string description, string Date)
        {
            StringBuilder st = new StringBuilder();
            st.Append("<div class='check checkDate'>");
            st.Append(Date);
            st.Append("</div>");
            st.Append("<div  class='check checkName'>");
            st.Append(payeeName);
            st.Append("</div>");
            st.Append("<div  class='check checkAmount'> ");
            st.Append(amount);
            st.Append("</div>");
            st.Append("<div  class='check checkAmountText'>");
            st.Append(amountText);
            st.Append("</div>");
            st.Append("<div  class='check checkAddress'>");
            st.Append(payeeAddress);
            st.Append("</div>");
            st.Append("<div  class='check checkDescription'>");
            st.Append(description);
            st.Append("</div>");
            return st.ToString();
        }
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
        static String NumWordsWrapper(double n)
        {
            string words = "";
            double intPart;
            double decPart = 0;
            if (n == 0)
                return "zero";
            try
            {
                string[] splitter = n.ToString().Split('.');
                intPart = double.Parse(splitter[0]);
                decPart = double.Parse(splitter[1]);
            }
            catch
            {
                intPart = n;
            }

            words = NumWords(intPart);

            if (decPart > 0)
            {
                if (words != "")
                    words += " and ";
                int counter = decPart.ToString().Length;
                switch (counter)
                {
                    //case 1: words += NumWords(decPart) + " tenths"; break;
                    //case 2: words += NumWords(decPart) + " hundredths"; break;
                    //case 3: words += NumWords(decPart) + " thousandths"; break;
                    //case 4: words += NumWords(decPart) + " ten-thousandths"; break;
                    //case 5: words += NumWords(decPart) + " hundred-thousandths"; break;
                    //case 6: words += NumWords(decPart) + " millionths"; break;
                    //case 7: words += NumWords(decPart) + " ten-millionths"; break;
                    case 1: words += decPart + " tenths"; break;
                    case 2: words += decPart + "/100 Cents"; break;

                }
            }
            return words;
        }

        static String NumWords(double n) //converts double to words
        {
            string[] numbersArr = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string[] tensArr = new string[] { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninty" };
            string[] suffixesArr = new string[] { "thousand", "million", "billion", "trillion", "quadrillion", "quintillion", "sextillion", "septillion", "octillion", "nonillion", "decillion", "undecillion", "duodecillion", "tredecillion", "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septdecillion", "Octodecillion", "Novemdecillion", "Vigintillion" };
            string words = "";

            bool tens = false;

            if (n < 0)
            {
                words += "negative ";
                n *= -1;
            }

            int power = (suffixesArr.Length + 1) * 3;

            while (power > 3)
            {
                double pow = Math.Pow(10, power);
                if (n >= pow)
                {
                    if (n % pow > 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1] + ", ";
                    }
                    else if (n % pow == 0)
                    {
                        words += NumWords(Math.Floor(n / pow)) + " " + suffixesArr[(power / 3) - 1];
                    }
                    n %= pow;
                }
                power -= 3;
            }
            if (n >= 1000)
            {
                if (n % 1000 > 0) words += NumWords(Math.Floor(n / 1000)) + " thousand, ";
                else words += NumWords(Math.Floor(n / 1000)) + " thousand";
                n %= 1000;
            }
            if (0 <= n && n <= 999)
            {
                if ((int)n / 100 > 0)
                {
                    words += NumWords(Math.Floor(n / 100)) + " hundred";
                    n %= 100;
                }
                if ((int)n / 10 > 1)
                {
                    if (words != "")
                        words += " ";
                    words += tensArr[(int)n / 10 - 2];
                    tens = true;
                    n %= 10;
                }

                if (n < 20 && n > 0)
                {
                    if (words != "" && tens == false)
                        words += " ";
                    words += (tens ? "-" + numbersArr[(int)n - 1] : numbersArr[(int)n - 1]);
                    n -= Math.Floor(n);
                }
            }

            return words;

        }
        #endregion

        #region Deposit Account
        public ActionResult DepositList()
        {
            var payroll = _depositRepository.GetDepositList();
            return View(payroll);
        }

        [HttpGet]
        public ActionResult NewDeposit()
        {
            var deposit = _depositRepository.GetDepositObject();
            return View(deposit);
        }
        [HttpPost]
        public ActionResult NewDeposit(DepositAccountModel depositAccount)
        {
            depositAccount.CreatedBy = (string)Session["FullName"];// ViewData.ContainsKey("FullName")? (string) ViewData["FullName"]: User.Identity.Name;
            _depositRepository.NewDeposit(depositAccount);
            return RedirectToAction("DepositList");
        }

        [HttpGet]
        public ActionResult EditDeposit(int depositId)
        {
            var deposit = _depositRepository.GetDeposit(depositId);
            return View(deposit);
        }
        [HttpPost]
        public ActionResult EditDeposit(DepositAccountModel deposit)
        {
            _depositRepository.UpdateDeposit(deposit);
            return RedirectToAction("DepositList");
        }
        public ActionResult DeleteDeposit(int depositId)
        {
            _depositRepository.DeleteDeposit(depositId);
            return RedirectToAction("DepositList");
        }
        public JsonResult DeleteDepositJson(int id)
        {

            _depositRepository.DeleteDeposit(id);
            var deposit = _depositRepository.GetDepositList();
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = deposit.ToList();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        public ActionResult DepositDetails(int depositId)
        {
            var deposit = _depositRepository.GetDeposit(depositId);
            return View(deposit);
        }
        public JsonResult SearchDepositJson(string query)
        {
            var deposit = _depositRepository.GetDepositList();

            if (!string.IsNullOrEmpty(query))
                deposit = deposit?
                            .Where(d => (d.DepositID.ToString().Contains(query.Trim()) ||
                                          d.PayeeID.ToString().Contains(query.Trim()) ||
                                          d.AmountDeposited.ToString().Contains(query.Trim()) ||
                                         d.DepositDepartment.Trim().ToLower().ToString().Contains(query.Trim().ToLower()) ||
                                         d.DepositType.Trim().ToLower().ToString().Contains(query.Trim().ToLower()) ||
                                         d.PayeeName.ToLower().Trim().ToString().Contains(query.Trim().ToLower()))
                                         ).OrderByDescending(c => c.DepositID).ToList();
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = deposit;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        #endregion

        #region Patient Extension List 

        private List<PatientExtensionModel> GetPatientExtesionList()
        {
            var extansions = _petientExtensionRepository.GetExtensionList();

            return extansions;

        }

        private PatientModel GetFolders(string patientCid, string patientName, string fName = null)
        {
            //string sharedForlder = @"C:\Users\mouni\Desktop\shared";// @sharedPath;
            var appSettings = ConfigurationManager.AppSettings;
            string sharedPath = appSettings["SharedFolderLink"] ?? "";
            string sharedForlder = @sharedPath;
            string searchPattern = "" + patientCid + "*";
            var patientModel = new PatientModel();
            patientModel.PatientFile = new PatientFileModel();
            string[] directoriesPaths = Directory.GetDirectories(sharedForlder, searchPattern);
            // string[] filePaths = Directory.GetFiles(@"C:\Users\mouni\Desktop\2860128700414- MOHAMMAD JAMAL ABDULLAH AL-FARHAN");
            directoriesPaths = Directory.GetDirectories(sharedForlder, searchPattern);
            string[] patientDirectoriesPaths = Directory.GetDirectories(directoriesPaths[0]);
            if (patientDirectoriesPaths.Length > 0)
            {

                patientModel.PatientFile.FoldersPath = patientDirectoriesPaths;
                var foldersName = ExtractNames(patientDirectoriesPaths);
                // new patient extension
                // when we trying to get the patient extension and the patient doesnt have the folder already created, we create one
                if (fName != null)
                    if (fName == PatientFolders.Extensions || fName == PatientFolders.ExtensionsHistory)
                    {
                        if (!foldersName.Contains("\\" + fName))
                        {
                            // create this folder
                            CreatePatientSpecificFolder(patientCid, patientName, directoriesPaths[0], fName);
                        }
                    }
                patientModel.PatientFile.FoldersName = foldersName;
                foreach (var path in patientDirectoriesPaths)
                {
                    int lastIndex = path.LastIndexOf(@"\");
                    string folderName = path.Substring(lastIndex + 1);// path.Substring(lastIndex, path.Length - (lastIndex));// exp: "PATIENT INFO"
                    if (fName != null && folderName.Equals(fName, StringComparison.OrdinalIgnoreCase))
                    {
                        patientModel.PatientFile.FolderName = folderName;
                        patientModel.PatientFile.FolderPath = path;
                        string[] filePaths = Directory.GetFiles(path);
                        var fileNames = ExtractNames(filePaths);
                        patientModel.PatientFile.FilesPath = filePaths;
                        patientModel.PatientFile.FilesName = fileNames;
                        return patientModel;
                    }
                }
            }

            return patientModel;
        }
        private static string[] ExtractNames(string[] directoriesPaths)
        {
            string[] foldersName = new string[directoriesPaths.Length];

            int i = 0;
            foreach (var path in directoriesPaths)
            {
                int lastIndex = path.LastIndexOf(@"\");

                string folderName = path.Substring(lastIndex, path.Length - (lastIndex));
                //int firstIndex = folderName.LastIndexOf(@"\");
                foldersName[i] = folderName;//.Substring(firstIndex, folderName.Length );
                i++;
            }
            return foldersName;
        }
        public FileResult Download(string link, string filename)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
            string fileName = filename;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public void Upload(HttpPostedFileBase file, string folderPath, string foldername)
        {
            //Checking file is available to save.  
            if (file != null)
            {
                string path = @folderPath;// @"C:\Users\mouni\Desktop\shared";
                                          //string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var inputFileName = Path.GetFileName(file.FileName);
                string uploadFilePathAndName = Path.Combine(path, inputFileName);
                file.SaveAs(uploadFilePathAndName);
                // postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
            }
        }

        private void UpdatePatientExtensionAndCreateHistory(PatientExtensionModel patExt)
        {
            patExt.IsPaid = true;
            _petientExtensionRepository.UpdatePatientExtension(patExt);
            //once the payment is done to the patient, the patient extension become a history, and the document transfered from extension folder,
            //to extension history
            //  Upload();
        }
        public void CopyFile(string source, string destination)
        {

            try
            {
                System.IO.File.Copy(source, destination);


            }
            catch (Exception ex) { };
        }
        private void DeletePatientExtensionFile(string filePath, string fileName, PaymentModel payment)
        {
            // before deleting the file from the folder we need to recreate the file in Extension  history
            var destinationFile = "";
            destinationFile = GetPatientExtensionHistoryFilePath(payment) + @"\" + payment.PatientExtension.FileName;
            CopyFile(@filePath, @destinationFile);
            System.IO.File.Delete(@filePath);

        }

        private string GetPatientExtensionHistoryFilePath(PaymentModel payment)
        {
            var folderPath = "";
            var patientFile = GetFolders(payment.PatientCID, payment.PatientFullName, PatientFolders.ExtensionsHistory).PatientFile;
            folderPath = patientFile.FolderPath;
            return folderPath;
        }
        //new create extension folder
        private void CreatePatientSpecificFolder(string patientCid, string patientName, string folderPath, string folderName)
        {
            var path = @folderPath;
            try
            {
                string subdir = path + @"\" + folderName;
                Directory.CreateDirectory(subdir);
            }
            catch (Exception e)
            {
                Danger(string.Format("An Error accured while trying to create patient folders, Try Again...!"), true);
            }
        }
        #endregion


    }
}