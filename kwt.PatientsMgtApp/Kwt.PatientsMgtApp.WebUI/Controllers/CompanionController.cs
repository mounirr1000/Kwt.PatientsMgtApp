using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using System.Text.RegularExpressions;
using Kwt.PatientsMgtApp.WebUI.Infrastructure;
using PagedList;
namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "ExceptionHandler")]
    //[Authorize(Roles = "Admin, Manager, Super Admin, Auditor, Editor, User")]
    [CustomAuthorize(Roles = CrudRoles.CompanionCrudRolesForAutorizeAttribute)]
    public class CompanionController : BaseController
    {
        private const int PageSize = 5;
        // GET: Patient
        private readonly ICompanionRepository _companionRepository;
        private readonly ICompanionManagmentRepository _companionManagmentRepository;
        private readonly IPatientManagmentRepository _patientManagmentRepository;
        private readonly IPatientRepository _patientRepository;

        public CompanionController()
        {
            _companionRepository = new CompanionRepository();
            _companionManagmentRepository = new CompanionManagmentRepository();
            _patientManagmentRepository = new PatientManagmentRepository();
            _patientRepository = new PatientRepository();
        }

        //
        //public ActionResult Index()
        //{
        //    return System.Web.UI.WebControls.View(UserManager.Users);
        //}

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        //
        // GET: Companion
        public ActionResult List(string searchCompanionText, string currentFilter, string sortOrder, int? page, bool? clearSearch)
        {
            //
            var users = UserManager.Users;
            //
            int pageNumber = (page ?? 1);
            //ViewBag.isBeneficiary = isBeneficiary ?? false;
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateInSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.CidSortParm = String.IsNullOrEmpty(sortOrder) ? "Cid" : "";
            ViewBag.BeneficiarySortParm = String.IsNullOrEmpty(sortOrder) ? "Beneficiary" : "";

            var companions = _companionRepository.GetCompanions();
            if (companions != null)
            {
                companions = companions.OrderByDescending(c => c.CreatedDate).ThenBy(co => co.IsActive).ToList();

                if (clearSearch != true)
                {
                    if (searchCompanionText != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchCompanionText = currentFilter;
                    }

                    ViewBag.CurrentFilter = searchCompanionText;
                    var result = new List<CompanionModel>();
                    if (!String.IsNullOrEmpty(searchCompanionText?.Trim()))
                    {
                        var term = searchCompanionText.Trim().ToLower();
                        result = companions
                            .Where(p =>
                                   p.CompanionCID.ToLower().Contains(term)
                                || p.CompanionFName.Trim().ToLower().Contains(term)
                                || p.CompanionLName.Trim().ToLower().Contains(term)
                                || (p.CompanionMName != null
                                    && p.CompanionMName.Trim().ToLower().Contains(term))
                                || p.Name.Trim().ToLower().Contains(term)
                            ).ToList();
                    }

                    if (result?.Count > 0)
                    {
                        Success(string.Format("We have <b>{0}</b> returned results from the searched criteria", result.Count),
                            true);
                        //return View(result.ToPagedList(pageNumber, PageSize));
                        return View(result);
                    }
                    if (!String.IsNullOrEmpty(searchCompanionText))
                    {
                        if (result?.Count == 0)
                        {
                            Information(
                                string.Format(
                                    "There is no patient in our records with the selected search criteria <b>{0}</b>",
                                    searchCompanionText), true);
                        }
                    }
                }
            }

            // return View(companions.ToPagedList(pageNumber, PageSize));
            return View(companions);
        }

        [ExceptionHandler]
        //public ActionResult Details(string companionCid)
        public ActionResult Details(string companionCid, string patientCid)
        {

            //CompanionModel companion = new CompanionModel();
            var companion = patientCid != null ? _companionRepository.GetCompanion(companionCid, patientCid) : _companionRepository.GetCompanion(companionCid);

            if (companion != null)
            {
                return View(companion);
            }
            else
            {
                Information(string.Format("Companion with Civil Id <b>{0}</b> Does Not exist in our records.", companionCid), true);
                return RedirectToAction("List");// View("List");
            }
        }

        [ExceptionHandler]
        [HttpGet]
        [CustomAuthorize(Roles = CrudRoles.CompanionCreateRolesForAutorizeAttribute)]
        public ActionResult Create(string patientcid)
        {
            CompanionModel companion = new CompanionModel();
            if (!string.IsNullOrEmpty(patientcid))
            {
                companion.PatientCID = patientcid;
            }
            companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
            companion.Banks = _patientManagmentRepository.GetBanks();
            return View(companion);
        }

        [ExceptionHandler]
        [HttpPost]
        [CustomAuthorize(Roles = CrudRoles.CompanionCreateRolesForAutorizeAttribute)]
        public ActionResult Create(CompanionModel companion)
        {
            ////
            //throw new PatientsMgtException(1, "error", "Creating new Companion",
            //        "You can't have two companions as primary type associated to the same user");

            ////
            ValidateCompanion(companion);
            ViewBag.IsValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                companion.CreatedBy = User.Identity.Name;
                _companionRepository.AddCompanion(companion);
                Success(string.Format("Patient with Civil ID <b>{0}</b> was successfully added.", companion.CompanionCID), true);
                return RedirectToAction("List");
            }

            else
            {
                Danger(string.Format("Please correct the error list before proceeding"), true);
                companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
                companion.Banks = _patientManagmentRepository.GetBanks();
                return View(companion);
            }

        }

        [ExceptionHandler]
        //public ActionResult Edit(string companionCid)
        [CustomAuthorize(Roles = CrudRoles.CompanionUpdateRolesForAutorizeAttribute)]
        public ActionResult Edit(string companionCid, string patientCid)
        {
            //var companion = _companionRepository.GetCompanion(companionCid);
            //var companion = _companionRepository.GetCompanion(companionCid, patientCid);
            var companion = patientCid != null ? _companionRepository.GetCompanion(companionCid, patientCid) : _companionRepository.GetCompanion(companionCid);
            companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
            companion.Banks = _patientManagmentRepository.GetBanks();
            return View(companion);
        }
        [HttpPost]
        [ExceptionHandler]
        [CustomAuthorize(Roles = CrudRoles.CompanionUpdateRolesForAutorizeAttribute)]
        public ActionResult Edit(CompanionModel companion)
        {
            ValidateCompanion(companion, true);
            ViewBag.IsValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {
                companion.ModifiedBy = User.Identity.Name;
                _companionRepository.UpdateCompanion(companion);
                Success(string.Format("companion with Civil Id <b>{0}</b> was successfully updated.", companion.CompanionCID), true);
                return RedirectToAction("Details", "Companion", new { companionCid = companion.CompanionCID });
            }
            else
            {
                Information(string.Format("companion with Civil Id <b>{0}</b> Was Not updated.", companion.CompanionCID), true);
                companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
                companion.Banks = _patientManagmentRepository.GetBanks();
                return View(companion);
            }
        }


        [ExceptionHandler]
        //[Authorize(Roles = "Admin, Manager, Super Admin")]
        [CustomAuthorize(Roles = CrudRoles.CompanionDeleteRolesForAutorizeAttribute)]
        public ActionResult Delete(int id)
        {
            //var companion = _companionRepository.GetCompanion(companionCid);
            var companion = _companionRepository.GetCompanion(id);

            if (companion != null)
            {
                var deleted = _companionRepository.DeleteCompanion(companion.CompanionCID, companion.PatientCID);
                if (deleted > 0)
                {
                    // display delete message success and redirect to patient list
                    Success(
                        string.Format("Companion with Civil ID <b>{0}</b> was Successfully Deleted.", companion.CompanionCID),
                        true);
                }
            }
            else
            {
                if (id != 0)
                    Information(string.Format("This Companion could not be removed", id), true);
            }
            return RedirectToAction("List");
        }

        private void ValidateCompanion(CompanionModel companion, bool isEdit = false)
        {
            string pattern = "^[a-zA-Z0-9]+$";// only allow alphanumeric values ^[a-zA-Z0-9]+$
            Regex rgx = new Regex(pattern);
            var primaryCompanionType = _companionManagmentRepository.GetCompanionTypes()
                                         .Where(c => c.Id == (int)Enums.CompanionType.Primary)
                                         .Select(ct => ct.CompanionType).FirstOrDefault();
            // get the patient Associated with this companion
            if (!companion.PatientCID.IsNullOrWhiteSpace())
            {
                ValidatePatient(companion, primaryCompanionType, isEdit);
            }
            if (ModelState.IsValidField("CompanionCID")
                && companion.CompanionCID?.Trim().Length < 12)
            {
                ModelState.AddModelError("CompanionCID", "The Companion CID should be 12 characters long");
            }
            if (ModelState.IsValidField("CompanionCID")
                && companion.CompanionCID != null
                && !rgx.IsMatch(companion.CompanionCID))
            {
                ModelState.AddModelError("CompanionCID", "The Companion CID should contains only alphanumeric(a-z/0-9) values");
            }
            if (ModelState.IsValidField("IsBeneficiary")
               && companion.IsBeneficiary == true)
            {
                if (companion.IBan == null)
                    ModelState.AddModelError("Iban", "The companion is Beneficiary, so you need to enter the Iban field");
                if (companion.IBan?.Trim().Length < 30)
                    ModelState.AddModelError("Iban", "The Iban should be 30 characters long");
            }
            if (ModelState.IsValidField("IsBeneficiary")
                && companion.IsBeneficiary == true
                && companion.BankName == null)
            {
                ModelState.AddModelError("BankName", "Since the companion is Beneficiary, you need to enter the Bank Name field");
            }
            if (ModelState.IsValidField("IsBeneficiary")
                && companion.IsBeneficiary == false
                && companion.BankName != null)
            {
                ModelState.AddModelError("BankName", "The companion is Not Beneficiary, so no need to enter the Bank Name field");
            }
            if (ModelState.IsValidField("IsBeneficiary")
                && companion.IsBeneficiary == false
                && companion.IBan != null)
            {
                ModelState.AddModelError("IBan", "The companion is Not Beneficiary, so no need to enter the Bank Account field");
            }
            if (ModelState.IsValidField("isActive")
                && companion.IsActive == false
                && companion.DateOut == null)
            {
                ModelState.AddModelError("DateOut", "The companion is not active, so you need to enter the Date out field");
            }
            if (ModelState.IsValidField("DateOut")
                && companion.DateOut != null
                && companion.IsActive)
            {
                ModelState.AddModelError("IsActive", "Date out is set, so the companion Active status should be No");
            }
            // if the companion is not Primary and the user select it to be beneficiary
            if (ModelState.IsValidField("CompanionType"))
            {

                if (companion.IsBeneficiary && companion.CompanionType != primaryCompanionType)
                    ModelState.AddModelError("CompanionType", companion.CompanionType +
                        " Can't be Beneficiary, Only " + primaryCompanionType + " companion who can");
            }
        }


        private void ValidatePatient(CompanionModel companion, string primaryCompanionType, bool isEdit = false)
        {
            var patient = _patientRepository.GetPatient(companion.PatientCID);
            // we need to check if this patient already have companion
            var companionList = _companionRepository.GetCompanions();//check if this companion already exist with the same patient
            var existingCompanions = companionList?.Where(c => c.PatientCID == companion.PatientCID).ToList();

            if (patient == null && ModelState.IsValidField("PatientCID"))
            {
                ModelState.AddModelError("PatientCID", "There is no patient in our record with this CID");
            }
           
            if (patient != null && ModelState.IsValidField("PatientCID"))
            {
                if (patient.IsActive && companion.CompanionType == primaryCompanionType)
                {
                    if (patient.IsBeneficiary && companion.IsBeneficiary)
                        ModelState.AddModelError("IsBeneficiary", "The patient with " + companion.PatientCID + " CID associated with this companion is already Beneficiary, You can't have the companion as beneficiary");
                    if (!patient.IsBeneficiary && !companion.IsBeneficiary)
                    {
                        // new 
                        if ((existingCompanions != null &&
                            !existingCompanions.Exists(
                                c => c.IsActive && c.IsBeneficiary && c.CompanionType == primaryCompanionType))
                                || existingCompanions == null)
                        {
                            // means no primary active beneficiary companion with this patient
                            if (ModelState.IsValidField("CompanionType") && companion.CompanionType == primaryCompanionType)
                                ModelState.AddModelError("IsBeneficiary", "The patient with " + companion.PatientCID +
                                            " CID associated with this companion is Not Beneficiary," +
                                              " and no companion with this patient is set as Beneficiary," +
                                             " So you need to set this companion as Beneficiary since is primary");
                        }


                    }

                }

            }
            if (patient != null)
            {

                //if (ModelState.IsValidField("CompanionType") && companion.CompanionType == primaryCompanionType)//primary companion
                //{


                if (existingCompanions != null && existingCompanions.Count > 0)
                {
                    if (!isEdit)
                    {
                        if (existingCompanions.Exists(c => c.CompanionCID == companion.CompanionCID))
                        {
                            ModelState.AddModelError("CompanionCID",
                                "This companion is already in our records with the same patient");
                            return;
                        }
                        // this means we have already an active companion who is primary with the same patient, so we can't have two primary companion to the same patient
                        if (existingCompanions.Exists(c => c.CompanionType == primaryCompanionType && c.IsActive))
                        {
                            if (companion.CompanionType == primaryCompanionType)
                            {
                                ModelState.AddModelError("CompanionType",
                                   "There is already a primary companion with this patient, you need to make this companion non primary");
                                return;
                            }

                        }
                    }
                    else
                    { // list of companion with the same patient ( 2 primary  1 active 1 nonactive)
                        if (existingCompanions.Exists(c => c.CompanionType == primaryCompanionType
                            && c.IsActive
                            && c.CompanionCID != companion.CompanionCID
                            // new 
                            && companion.CompanionType == primaryCompanionType
                            ))
                        {
                            ModelState.AddModelError("CompanionType",
                                "There is already a primary companion with this patient, you need to make this companion non primary");
                            return;
                        }
                    }

                    //foreach (var comp in existingCompanions.Where(c => c.IsActive == true))
                    //{

                    //    if (comp.CompanionCID == companion.CompanionCID)
                    //    {
                    //        if (comp.CompanionType == primaryCompanionType)
                    //        {
                    //            ModelState.AddModelError("CompanionCID", "This companion is already in our records with the same patient");
                    //            break;
                    //        }
                    //    }
                    //    if (comp.CompanionCID != companion.CompanionCID)
                    //    {
                    //        if (comp.CompanionType == primaryCompanionType)
                    //        {
                    //            ModelState.AddModelError("CompanionType", "There is already a companion with the patient declared as Primary, You can't have this companion as primary");
                    //            break;
                    //        }
                    //    }
                    //}
                }
                //}
                // see if there is a c primary companion with this patient, we can't have two companion as primary
            }
        }
    }
}