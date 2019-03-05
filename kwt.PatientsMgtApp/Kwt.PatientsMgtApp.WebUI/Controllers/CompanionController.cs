using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "PatientMgtException")]
    [Authorize(Roles = "Admin, Manager")]
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
                //switch (sortOrder)
                //{
                //    case "name_desc":
                //        companions = companions.OrderBy(c => c.CompanionFName).ThenBy(c => c.CompanionLName).ToList();
                //        break;
                //    case "cid":
                //        companions = companions.OrderBy(c => c.CompanionCID).ToList();
                //        break;
                //    case "date_desc":
                //        companions = companions.OrderBy(c => c.DateIn).ToList();
                //        break;
                //    case "Beneficiary":
                //        companions = companions.OrderBy(c => c.IsBeneficiary).ToList();
                //        break;
                //    default: // created date ascending 
                //        companions = companions.OrderByDescending(c => c.CreatedDate).ToList();
                //        break;
                //}
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

                        // filter isBeneficiary
                        //if (isBeneficiary == true)
                        //{
                        //    result = result?.Where(p => p.IsBeneficiary).ToList();
                        //}
                        //else if (isBeneficiary == false)
                        //{
                        //    result = result?.Where(p => !p.IsBeneficiary).ToList();
                        //}
                    }
                    //else
                    //{
                    //    // filter isBeneficiary
                    //    if (isBeneficiary == true)
                    //    {
                    //        result = companions?.Where(p => p.IsBeneficiary).ToList();
                    //    }
                    //    else if (isBeneficiary == false)
                    //    {
                    //        result = companions?.Where(p => !p.IsBeneficiary).ToList();
                    //    }
                    //}
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

                            //if (isBeneficiary != null)
                            //{
                            //    benMessage = isBeneficiary == true ? "& is Beneficiary" : "& is Not Beneficiary";
                            //}
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
        public ActionResult Details(string companionCid)
        {

            var companion = _companionRepository.GetCompanion(companionCid);

            if (companion != null)
            {
                return View(companion);
            }
            else
            {
                Information(string.Format("Companion with Civil Id <b>{0}</b> Does Not exist in our records.", companionCid), true);
                return View("List");
            }
        }

        [ExceptionHandler]
        [HttpGet]
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
        public ActionResult Create(CompanionModel companion)
        {
            ValidateCompanion(companion);
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
        public ActionResult Edit(string companionCid)
        {
            var companion = _companionRepository.GetCompanion(companionCid);
            companion.CompanionTypes = _companionManagmentRepository.GetCompanionTypes();
            companion.Banks = _patientManagmentRepository.GetBanks();
            return View(companion);
        }
        [HttpPost]
        [ExceptionHandler]
        public ActionResult Edit(CompanionModel companion)
        {
            ValidateCompanion(companion);
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
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(string companionCid)
        {
            var companion = _companionRepository.GetCompanion(companionCid);

            if (companion != null)
            {
                var deleted = _companionRepository.DeleteCompanion(companion.CompanionCID,companion.PatientCID);
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
                if (!string.IsNullOrEmpty(companionCid))
                    Information(string.Format("Companion with Civil ID <b>{0}</b> was Not Deleted.", companionCid), true);
            }
            return RedirectToAction("List");
        }

        private void ValidateCompanion(CompanionModel companion)
        {
            var companionType = _companionManagmentRepository.GetCompanionTypes()
                                         .Where(c => c.Id == (int)Enums.CompanionType.Primary)
                                         .Select(ct => ct.CompanionType).FirstOrDefault();
            // get the patient Associated with this companion
            if (companion.PatientCID != null)
            {
                var patient = _patientRepository.GetPatient(companion.PatientCID);
                if (patient == null && ModelState.IsValidField("PatientCID"))
                {
                    ModelState.AddModelError("PatientCID", companion.PatientCID + " is an incorrect Patient Cid, There is not patient in our record with this Cid");
                }
                if (patient != null && ModelState.IsValidField("PatientCID"))
                {
                    if (patient.IsActive)
                    {
                        if (patient.IsBeneficiary && companion.IsBeneficiary)
                            ModelState.AddModelError("IsBeneficiary", "The patient with " + companion.PatientCID + " CID associated with this companion is already Beneficiary, You can't have the companion as beneficiary");
                        if (!patient.IsBeneficiary && !companion.IsBeneficiary)
                            if (ModelState.IsValidField("CompanionType") && companion.CompanionType == companionType)
                                ModelState.AddModelError("IsBeneficiary", "The patient with " + companion.PatientCID +
                                            " CID associated with this companion is Not Beneficiary, So you need to set this companion as Beneficiary");
                    }

                }
                if (patient != null)
                {
                    if (ModelState.IsValidField("CompanionType") && companion.CompanionType == companionType)
                    {
                        var existingCompanions = _companionRepository.GetCompanions().Where(c => c.PatientCID == companion.PatientCID && c.IsActive);

                        foreach (var comp in existingCompanions.Where(c => c.IsActive == true))
                        {
                            if (comp.CompanionCID != companion.CompanionCID)
                            {
                                if (comp.CompanionType == companionType)
                                {
                                    ModelState.AddModelError("CompanionType", "There is already a companion with the patient declared as Primary, You can't have this companion as primary");
                                    break;
                                }
                            }
                        }
                    }
                    // see if there is a c primary companion with this patient, we can't have two companion as primary
                }
            }
            if (ModelState.IsValidField("IsBeneficiary")
               && companion.IsBeneficiary == true
               && companion.IBan == null)
            {
                ModelState.AddModelError("IBan", "Since the companion is Beneficiary, you need to enter the Bank Account field");
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

                if (companion.IsBeneficiary && companion.CompanionType != companionType)
                    ModelState.AddModelError("CompanionType", companion.CompanionType +
                        " Can't be Beneficiary, Only " + companionType + " companion who can");
            }
        }
    }
}