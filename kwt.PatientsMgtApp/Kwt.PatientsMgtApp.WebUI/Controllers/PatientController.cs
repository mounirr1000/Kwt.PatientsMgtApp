using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;
using Kwt.PatientsMgtApp.WebUI.Models;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{

    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "PatientMgtException")]
    [Authorize]
    public class PatientController : BaseController
    {
        private const int PageSize = 5;
        // GET: Patient
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientManagmentRepository _patientManagmentRepository;

        //Patients List
        //private List<PatientModel> _patientList; 

        public PatientController()
        {
            _patientRepository = new PatientRepository();
            _patientManagmentRepository = new PatientManagmentRepository();
            // _patientList = _patientRepository.GetPatients();
        }

        public ActionResult Index()
        {

            return RedirectToAction("List");
        }

        [ExceptionHandler]
        public ActionResult List(string searchPatientText, string currentFilter, string sortOrder, int? page, bool? clearSearch)
        {
            int pageNumber = (page ?? 1);
            // ViewBag.isBeneficiary = isBeneficiary ?? false;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AptSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.CidSortParm = String.IsNullOrEmpty(sortOrder) ? "Cid" : "";
            ViewBag.BeneficiarySortParm = String.IsNullOrEmpty(sortOrder) ? "Beneficiary" : "";

            var patients = _patientRepository.GetPatients();
            if (patients != null)
            {
                switch (sortOrder)
                {
                    case "name_desc":
                        patients = patients.OrderBy(p => p.PatientFName).ThenBy(p => p.PatientLName).ToList();
                        break;
                    case "cid":
                        patients = patients.OrderBy(p => p.PatientCID).ToList();
                        break;
                    case "date_desc":
                        patients = patients.OrderBy(p => p.FirstApptDAte).ToList();
                        break;
                    case "Beneficiary":
                        patients = patients.OrderBy(p => p.IsBeneficiary).ToList();
                        break;
                    default: // created date ascending 
                        patients = patients.OrderByDescending(p => p.CreatedDate).ThenByDescending(c => c.IsActive).ToList();
                        break;
                }
                if (clearSearch != true)
                {
                    if (searchPatientText != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchPatientText = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchPatientText;
                    var result = new List<PatientModel>();
                    if (!String.IsNullOrEmpty(searchPatientText))
                    {
                        var term = searchPatientText.ToLower();
                        result = patients?
                            .Where(p => (
                                  p.PatientCID.ToLower().Contains(term.Trim())
                               || p.PatientFName.ToLower().Trim().Contains(term.Trim())
                               || p.PatientLName.ToLower().Trim().Contains(term.Trim()))
                               || p.Name.ToLower().Trim().Contains(term.Trim())
                            ).ToList();
                    }
                    
                    if (result?.Count > 0)
                    {
                        Success(string.Format("We have <b>{0}</b> returned results from the searched criteria", result.Count),
                            true);
                        //return View(result.ToPagedList(pageNumber, PageSize));
                        return View(result);
                    }

                    if (result?.Count == 0 && !String.IsNullOrEmpty(searchPatientText))
                    {
                        Information(
                                string.Format(
                                    "There is no patient in our records with the selected search criteria <b>{0}</b>",
                                    searchPatientText), true);
                    }
                    
                }
            }

            //return View(patients.ToPagedList(pageNumber, PageSize));
            return View(patients);

        }

        [ExceptionHandler]
        public ActionResult Details(string patientCid)
        {

            var patient = _patientRepository.GetPatient(patientCid);

            if (patient != null)
            {
                return View(patient);
            }
            else
            {
                Information(string.Format("Patient with Civil ID <b>{0}</b> Does Not exist in our records.", patientCid), true);
                return View("List");
            }
        }
        [ExceptionHandler]
        [HttpGet]
        public ActionResult Create()
        {
            PatientModel patient = new PatientModel();
            patient.Agencies = _patientManagmentRepository.GetAgencies();
            patient.Banks = _patientManagmentRepository.GetBanks();
            patient.Hospitals = _patientManagmentRepository.GetHospitals();
            patient.Doctors = _patientManagmentRepository.GetDoctors();
            patient.Sepcialities = _patientManagmentRepository.GetSpecialities();
            return View(patient);
        }
        [ExceptionHandler]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientModel patient)
        {
            ValidatePatientModel(patient);
            if (ModelState.IsValid)
            {
                patient.CreatedBy = User.Identity.Name;
                _patientRepository.AddPatient(patient);
                Success(string.Format("Patient with Civil ID <b>{0}</b> was successfully added.", patient.PatientCID), true);
                if (patient.HasCompanion)
                {
                    return RedirectToAction("Create","Companion",new {patientcid=patient.PatientCID});
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                Danger(string.Format("Please correct the error list before proceeding"), true);
                patient.Agencies = _patientManagmentRepository.GetAgencies();
                patient.Banks = _patientManagmentRepository.GetBanks();
                patient.Hospitals = _patientManagmentRepository.GetHospitals();
                patient.Doctors = _patientManagmentRepository.GetDoctors();
                patient.Sepcialities = _patientManagmentRepository.GetSpecialities();
                return View(patient);
            }

        }

        private void ValidatePatientModel(PatientModel patient)
        {
            if (ModelState.IsValidField("IsBeneficiary")
               && patient.IsBeneficiary == true
               && patient.Iban == null)
            {
                ModelState.AddModelError("Iban", "The patient is Beneficiary, so you need to enter the Iban field");
            }
            if (ModelState.IsValidField("IsBeneficiary")
                && patient.IsBeneficiary == true
                && patient.BankName == null)
            {
                ModelState.AddModelError("BankName", "The patient is Beneficiary, so you need to enter the Bank Name field");
            }
            //if (ModelState.IsValidField("IsBeneficiary")
            //    && patient.IsBeneficiary == false
            //    && patient.BankName != null)
            //{
            //    ModelState.AddModelError("BankName", "The patient is Not Beneficiary, so no need to enter the Bank Name field");
            //}
            if (ModelState.IsValidField("isActive")
                && patient.IsActive == false
                && patient.EndTreatDate == null)
            {
                ModelState.AddModelError("EndTreatDate", "The patient is not active, so you need to enter the end treatment date");
            }

        }
        [ExceptionHandler]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(string patientCid)
        {
            var patient = _patientRepository.GetPatient(patientCid);
            patient.Agencies = _patientManagmentRepository.GetAgencies();
            patient.Banks = _patientManagmentRepository.GetBanks();
            patient.Hospitals = _patientManagmentRepository.GetHospitals();
            patient.Doctors = _patientManagmentRepository.GetDoctors();
            patient.Sepcialities = _patientManagmentRepository.GetSpecialities();
            return View(patient);
        }
        [HttpPost]
        [ExceptionHandler]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientModel patient)
        {
            if (ModelState.IsValidField("IsBeneficiary")
                && patient.IsBeneficiary == true
                && patient.Iban == null)
            {
                ModelState.AddModelError("Iban", "The patient is Beneficiary, so you need to enter the Iban field");
            }
            if (ModelState.IsValidField("IsBeneficiary")
                && patient.IsBeneficiary == true
                && patient.BankName == null)
            {
                ModelState.AddModelError("BankName", "The patient is Beneficiary, so you need to enter the Bank Name field");
            }
            if (ModelState.IsValid)
            {
                patient.ModifiedBy = User.Identity.Name;
                _patientRepository.UpdatePatient(patient);
                Success(string.Format("Patient with Civil ID <b>{0}</b> was successfully updated.", patient.PatientCID), true);
                return RedirectToAction("Details", "Patient", new { patientCid = patient.PatientCID });
            }
            else
            {
                Information(string.Format("Patient with Civil ID <b>{0}</b> Was Not updated.", patient.PatientCID), true);
                patient.Agencies = _patientManagmentRepository.GetAgencies();
                patient.Banks = _patientManagmentRepository.GetBanks();
                patient.Hospitals = _patientManagmentRepository.GetHospitals();
                patient.Doctors = _patientManagmentRepository.GetDoctors();
                patient.Sepcialities = _patientManagmentRepository.GetSpecialities();
                return View(patient);
            }
        }

        // Todo: Should fix Delete to be only with Httppost method
        [ExceptionHandler]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(string patientCid)
        {
            var patient = _patientRepository.GetPatient(patientCid);

            if (patient != null)
            {
                var deleted = _patientRepository.DeletePatient(patient);
                if (deleted > 0)
                {
                    // display delete message success and redirect to patient list
                    Success(
                        string.Format("Patient with Civil ID <b>{0}</b> was Successfully Deleted.", patient.PatientCID),
                        true);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(patientCid))
                    Information(string.Format("Patient with Civil ID <b>{0}</b> was not deleted.", patientCid), true);
            }
            return RedirectToAction("List");
        }

        //Search Patient
        public ActionResult Search(string patientCid)
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer;

            var patient = _patientRepository.GetPatient(patientCid);
            string controllerName = (string)TempData["controller"] ?? "Home";
            string actionName = (string)TempData["action"] ?? "Index";

            if (patient != null)
            {
                TempData["searchedPatient"] = patient;

                if (url != null)
                    return Redirect(url.PathAndQuery);
                return RedirectToAction("List", controllerName);
            }
            else
            {
                //TempData["searchedPatient"] = null;
                if (!string.IsNullOrEmpty(patientCid))
                    Information(string.Format("Patient with Civil ID <b>{0}</b> Does Not exist in our records.", patientCid), true);
                if (url != null)
                    return Redirect(url.PathAndQuery);
                return RedirectToAction(actionName, controllerName);
            }

        }

        //public ActionResult PdfDoc()
        //{
        //    var patients = _patientRepository.GetPatients();

        //    //return new RazorPDF.PdfResult(patients, "PdfDoc");
        //    return null;
        //}
    }
}