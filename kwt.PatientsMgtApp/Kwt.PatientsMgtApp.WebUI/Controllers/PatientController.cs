﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
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
using System.IO;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.WebUI.Infrastructure;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{

    [HandleError(ExceptionType = typeof(PatientsMgtException), View = "ExceptionHandler")]
    //[Authorize]
    [CustomAuthorize(Roles = CrudRoles.PatientCrudRolesForAutorizeAttribute)]
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
        public ActionResult List2(string searchPatientText, string currentFilter, string sortOrder, int? page, bool? clearSearch)
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
        //new list
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
                // filter just active patients
                patients = patients.Where(p => p.IsActive).OrderByDescending(c => c.CreatedDate).ToList();
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
                               //|| p.PatientFName.ToLower().Trim().Contains(term.Trim())
                               //|| p.PatientLName.ToLower().Trim().Contains(term.Trim()))
                               //|| p.Name.ToLower().Trim().Contains(term.Trim()
                               )
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
            return View(patients);

        }
        // end new list
        // new 2020
        [HttpGet]
        public JsonResult GetPatientsJson(string query)
        {
            var patients = _patientRepository.GetPatients();
            if (!String.IsNullOrEmpty(query))
                patients = patients?
                                .Where(p =>
                                    p.PatientCID.ToLower().Trim().Contains(query.ToLower().Trim()) ||
                                    p.Name.ToLower().ToString().Trim().Contains(query.ToLower().Trim())
                                ).OrderByDescending(c => c.CreatedDate).ToList();
            else
            {
                patients = patients.Where(p => p.IsActive).OrderByDescending(c => c.CreatedDate).ToList();
            }
            var jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = Int32.MaxValue;
            jsonResult.Data = patients;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jsonResult;
        }
        //emd new 2020
        [ExceptionHandler]
        public ActionResult Details(string patientCid)
        {

            var patient = _patientRepository.GetPatient(patientCid);

            if (patient != null)
            {
                patient.PatientFiles = new PatientFileModel[] {
                    GetFolders(patient.PatientCID, patient.Name,PatientFolders.PatientInfo).PatientFile,
                    GetFolders(patient.PatientCID, patient.Name,PatientFolders.Appointments).PatientFile,
                 };
                //patient.PatientFile = GetFolders(patient.PatientCID, patient.Name,"PATIENT INFO").PatientFile;

                return View(patient);
            }
            else
            {
                Information(string.Format("Patient with Civil ID <b>{0}</b> Does Not exist in our records.", patientCid), true);
                return RedirectToAction("List");// View("List");
            }
        }
        [ExceptionHandler]
        [HttpGet]
        [CustomAuthorize(Roles = CrudRoles.PatientCreateRolesForAutorizeAttribute)]
        public ActionResult Create()
        {
            PatientModel patient = _patientRepository.GetPatientObject();// new PatientModel();
            //patient.Agencies = _patientManagmentRepository.GetAgencies();
            //patient.Banks = _patientManagmentRepository.GetBanks();
            //patient.Hospitals = _patientManagmentRepository.GetHospitals();
            //patient.Doctors = _patientManagmentRepository.GetDoctors();
            //patient.Sepcialities = _patientManagmentRepository.GetSpecialities();
            return View(patient);
        }
        [ExceptionHandler]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Roles = CrudRoles.PatientCreateRolesForAutorizeAttribute)]
        public ActionResult Create(PatientModel patient)
        {
            //throw new PatientsMgtException(1, "error", "Creating new Companion",
            //        "You can't have two companions as primary type associated to the same user");
            ValidatePatientModel(patient);
            ViewBag.IsValid = ModelState.IsValid;
            if (ModelState.IsValid)
            {


                patient.CreatedBy = User.Identity.Name;
                _patientRepository.AddPatient(patient);
                // new implelemtation for file creating during patinent creations
                CreatePatientFiles(patient);
                // end new
                Success(string.Format("Patient with Civil ID <b>{0}</b> was successfully added.", patient.PatientCID), true);
                if (patient.HasCompanion)
                {
                    return RedirectToAction("Create", "Companion", new { patientcid = patient.PatientCID });
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
            string pattern = "^[a-zA-Z0-9]+$";// only allow alphanumeric values ^[a-zA-Z0-9]+$
            Regex rgx = new Regex(pattern);
            if (ModelState.IsValidField("PatientCID")
                && patient.PatientCID?.Trim().Length < 12)
            {
                ModelState.AddModelError("PatientCID", "The patient CID should be 12 characters long");
            }
            if (ModelState.IsValidField("PatientCID")
               && patient.PatientCID != null
               && !rgx.IsMatch(patient.PatientCID))
            {
                ModelState.AddModelError("PatientCID", "The Patient CID should contains only alphanumeric(a-z/0-9) values");
            }
            if (ModelState.IsValidField("IsBeneficiary")
               && patient.IsBeneficiary == true)
            {
                if (patient.Iban == null)
                    ModelState.AddModelError("Iban", "The patient is Beneficiary, so you need to enter the Iban field");
                if (patient.Iban?.Trim().Length < 30)
                    ModelState.AddModelError("Iban", "The Iban should be 30 characters long");
            }
            if (ModelState.IsValidField("IsBeneficiary")
                && patient.IsBeneficiary == true
                && patient.BankName == null)
            {
                ModelState.AddModelError("BankName", "The patient is Beneficiary, so you need to enter the Bank Name field");
            }

            if (ModelState.IsValidField("isActive")
                && patient.IsActive == false
                && patient.EndTreatDate == null)
            {
                ModelState.AddModelError("EndTreatDate", "The patient is not active, so you need to enter the end treatment date");
            }
            // Patient extension
            if (ModelState.IsValidField("HasExtension")
                && patient.HasExtension == true
                && (patient.PatientExtension.ExtensionEndDate == null || patient.PatientExtension.ExtensionStartDate == null || patient.PatientExtesionFile == null))
            {
                ModelState.AddModelError("HasExtension", "The patient has extension, make sure you add extension start date, extension end date & you upload the extension document");
            }
        }

        private void CreatePatientFiles(PatientModel patient)
        {
            var patientName = patient.PatientFName + " " + patient.PatientMName + " " + patient.PatientLName;
            if (patient.PatientPasportFile != null)
            {
                var pat = GetFolders(patient.PatientCID, patientName, PatientFolders.PatientInfo);
                Upload(patient.PatientPasportFile, pat.PatientFile.FolderPath, pat.PatientFile.FolderName);
            }
            if (patient.PatientFirstApointmentFile != null)
            {
                var pat = GetFolders(patient.PatientCID, patientName, PatientFolders.Appointments);
                Upload(patient.PatientFirstApointmentFile, pat.PatientFile.FolderPath, pat.PatientFile.FolderName);
            }
            if (patient.PatientEndTreatmentFile != null)
            {
                var pat = GetFolders(patient.PatientCID, patientName, PatientFolders.Appointments);
                Upload(patient.PatientEndTreatmentFile, pat.PatientFile.FolderPath, pat.PatientFile.FolderName);
            }
            //new patient extensions
            if (patient.PatientExtesionFile != null)
            {
                var pat = GetFolders(patient.PatientCID, patientName, PatientFolders.Extensions);
                Upload(patient.PatientExtesionFile, pat.PatientFile.FolderPath, pat.PatientFile.FolderName);
                //new Patient Extension
                var inputFileName = Path.GetFileName(patient.PatientExtesionFile[0]?.FileName);
                var path = @pat.PatientFile.FolderPath;
                string uploadFilePathAndName = Path.Combine(path, inputFileName);
                patient.ExtensionDocLink = uploadFilePathAndName;
                patient.ExtensionFileName = inputFileName;
                _patientRepository.CreatePatientExtension(patient);
            }
        }
        [ExceptionHandler]

        //[CustomAuthorize(Roles = "Admin, Manager, Super Admin, Medical, Editor")]
        [CustomAuthorize(Roles = CrudRoles.PatientUpdateRolesForAutorizeAttribute)]
        public ActionResult Edit(string patientCid)
        {
            var patient = _patientRepository.GetPatient(patientCid);
            if (patient != null)
            {
                patient.Agencies = _patientManagmentRepository.GetAgencies();
                patient.Banks = _patientManagmentRepository.GetBanks();
                patient.Hospitals = _patientManagmentRepository.GetHospitals();
                patient.Doctors = _patientManagmentRepository.GetDoctors();
                patient.Sepcialities = _patientManagmentRepository.GetSpecialities();
                patient.PatientFiles = new PatientFileModel[] {
                    GetFolders(patient.PatientCID, patient.Name,PatientFolders.PatientInfo).PatientFile,
                    GetFolders(patient.PatientCID, patient.Name,PatientFolders.Appointments).PatientFile,
                    // new for extensions
                    GetFolders(patient.PatientCID,patient.Name,PatientFolders.Extensions).PatientFile

            };
                //patient extension
                patient.HasExtension = patient.PatientExtension != null;
                return View(patient);
            }
            Information(string.Format("Patient with Civil ID <b>{0}</b> Does Not exist in our records.", patientCid), true);
            return RedirectToAction("List");



        }
        [HttpPost]
        [ExceptionHandler]
        //[CustomAuthorize(Roles = "Admin, Manager, Super Admin, Medical, Editor")]
        [CustomAuthorize(Roles = CrudRoles.PatientUpdateRolesForAutorizeAttribute)]
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
            // new check isdead validity
            if (ModelState.IsValidField("IsDead")
                && patient.IsDead == true
                && (patient.DeathDate == null || patient.EndTreatDate == null))
            {
                if (patient.DeathDate == null)
                    ModelState.AddModelError("DeathDate", "The patient is Dead, so you need to enter the Death Date field");
                if (patient.EndTreatDate == null)
                    ModelState.AddModelError("EndTreatDate", "The patient is Dead, so you need to enter the End Treat Date field");
                if (patient.IsActive == true)
                    ModelState.AddModelError("IsActive", "The patient is Dead, so the patient should not be active");
            }
            //
            if (ModelState.IsValid)
            {
                patient.ModifiedBy = User.Identity.Name;
                _patientRepository.UpdatePatient(patient);
                CreatePatientFiles(patient);
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
                patient.PatientFiles = new PatientFileModel[] {
                    GetFolders(patient.PatientCID, patient.Name,PatientFolders.PatientInfo).PatientFile,
                    GetFolders(patient.PatientCID, patient.Name,PatientFolders.Appointments).PatientFile,
                    // new for extensions
                    GetFolders(patient.PatientCID,patient.Name,PatientFolders.Extensions).PatientFile
            };
                return View(patient);
            }
        }

        // Todo: Should fix Delete to be only with Httppost method
        [ExceptionHandler]
        //[CustomAuthorize(Roles = "Super Admin, Manager, Admin, Editor")]
        [CustomAuthorize(Roles = CrudRoles.PatientDeleteRolesForAutorizeAttribute)]
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
                else
                {
                    if (!string.IsNullOrEmpty(patientCid))
                        Information(string.Format("Patient with Civil ID <b>{0}</b> was not deleted.", patientCid), true);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(patientCid))
                    Information(string.Format("There is no patient in our records with CID <b> {0}</b>", patientCid), true);
            }
            return RedirectToAction("List");
        }

        //Search Patient
        public ActionResult Search(string patientCid)
        {
            var url = System.Web.HttpContext.Current.Request.UrlReferrer;

            var patient = _patientRepository.GetSearchedPatient(patientCid);
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


        //new Books


        [ExceptionHandler]
        [HttpGet]
        public ActionResult AddPatientBooks(string patientCid)
        {
            var patients = _patientRepository.GetActivePatients();

            if (patients != null)
            {
                var cids = patients.Select(pcid => pcid.PatientCID).ToList();
                ViewBag.Patients = cids;

            }
            if (!String.IsNullOrEmpty(patientCid))
            {
                var patient = _patientRepository.GetPatient(patientCid);
                return View(patient);
            }
            //return new RazorPDF.PdfResult(patients, "PdfDoc");
            return View();
            //payment = patientCid != null ? _paymentRepository.GetPaymentObject(patientCid) : _paymentRepository.GetPaymentObject();
            //if (!String.IsNullOrEmpty(patientCid) &&
            //    (payment.PatientCID == null))
            //{

            //    Information(
            //        String.Format(
            //            "There is No patient in our records with this CID <b>{0}</b> Please Enter a Valid CID",
            //            patientCid), true);
            //}
            //else if (!String.IsNullOrEmpty(patientCid) &&
            //     (payment.BeneficiaryCID == null))
            //{

            //    Information(
            //        String.Format(
            //            "There is No Beneficiary with this patient <b>{0}</b> You can't make payment to this patient",
            //            patientCid), true);
            //}
            //else if (!String.IsNullOrEmpty(patientCid)
            //    && payment.PatientCID != null
            //    && !payment.IsActive)
            //{

            //    Information(
            //        String.Format(
            //            "The patient with CID <b>{0}</b> is Not Active, You cannot add a payment to this patient",
            //            patientCid), true);
            //}

            //return View(payment);
        }

        public ActionResult GetPatientFolders(string patientCid, string patientName)
        {
            //GetFolders(patientCid, patientName);
            //GetDirectories (string path, string searchPattern, System.IO.SearchOption searchOption);
            var appSettings = ConfigurationManager.AppSettings;
            string sharedPath = appSettings["SharedFolderLink"] ?? "";
            // replace the next line with sharedPath from appSetting in configfile.
            //string sharedForlder = @"C:\Users\mouni\Desktop\shared";// @sharedPath;
            string sharedForlder = @sharedPath;
            string searchPattern = "" + patientCid + "*";
            var fileViewModel = new FileViewModel();
            string[] directoriesPaths = Directory.GetDirectories(sharedForlder, searchPattern);
            // string[] filePaths = Directory.GetFiles(@"C:\Users\mouni\Desktop\2860128700414- MOHAMMAD JAMAL ABDULLAH AL-FARHAN");
            if (directoriesPaths.Length > 0)
            {
                string[] patientDirectoriesPaths = Directory.GetDirectories(directoriesPaths[0]);

                if (patientDirectoriesPaths.Length > 0)
                {
                    fileViewModel.FoldersPath = patientDirectoriesPaths;
                    var foldersName = ExtractNames(patientDirectoriesPaths);
                    //
                    bool[] emptyFolders = new bool[patientDirectoriesPaths.Length];
                    int i = 0;
                    foreach (var path in patientDirectoriesPaths)
                    {
                        var files = Directory.GetFiles(path);
                        if (files.Length > 0)
                        {
                            emptyFolders[i++] = true;
                        }
                        else
                        {
                            emptyFolders[i++] = false;
                        }
                    }
                    fileViewModel.IsFolderEmpty = emptyFolders;
                    //
                    fileViewModel.FoldersName = foldersName;
                }
            }
            //Uncoment the next lines once the implementation is approved by Hasbaoui
            else
            {
                CreatePatientFolders(patientCid, patientName, sharedForlder);
                return RedirectToAction("GetPatientFolders", new { patientCid = patientCid, patientName = patientName });
            }
            fileViewModel.PatientCid = patientCid;
            fileViewModel.PatientName = patientName;
            return View(fileViewModel);
        }

        // new for file creation during new patient
        private PatientModel GetFolders(string patientCid, string patientName, string fName = null)
        {
            //string sharedForlder = @"C:\Users\mouni\Desktop\shared";// @sharedPath;
            var appSettings = ConfigurationManager.AppSettings;
            string sharedPath = appSettings["SharedFolderLink"] ?? "";
            string sharedForlder =  @sharedPath;
            string searchPattern = "" + patientCid + "*";
            var patientModel = new PatientModel();
            patientModel.PatientFile = new PatientFileModel();
            string[] directoriesPaths = Directory.GetDirectories(sharedForlder, searchPattern);
            // string[] filePaths = Directory.GetFiles(@"C:\Users\mouni\Desktop\2860128700414- MOHAMMAD JAMAL ABDULLAH AL-FARHAN");
            if (directoriesPaths.Length == 0)
            {
                CreatePatientFolders(patientCid, patientName, sharedForlder);
                directoriesPaths = Directory.GetDirectories(sharedForlder, searchPattern);
            }
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
                        if (!foldersName.Contains("\\"+fName))
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
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] patientFiles, string folderPath, string foldername)
        {
            if (patientFiles != null)
            {
                foreach (HttpPostedFileBase file in patientFiles)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        string path = @folderPath;// @"C:\Users\mouni\Desktop\shared";
                                                  //string path = Server.MapPath("~/Uploads/");
                        if (path != null && !Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var inputFileName = Path.GetFileName(file.FileName);
                        string uploadFilePathAndName = Path.Combine(path, inputFileName);
                        file.SaveAs(uploadFilePathAndName);
                        // postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));
                        ViewBag.UploadState = patientFiles.Count().ToString() + " files uploaded successfully.";
                    }
                }
            }

            return RedirectToAction("GetPatientFiles", new { patientFolderPath = folderPath, folderName = foldername, uploadState = ViewBag.UploadState });
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

        public ActionResult GetPatientFiles(string patientFolderPath, string folderName, string uploadState, string patientCid, string patientName)
        {
            //GetDirectories (string path, string searchPattern, System.IO.SearchOption searchOption);
            //ViewBag.pageTitle = folderName;
            string[] filePaths = Directory.GetFiles(patientFolderPath);
            var fileNames = ExtractNames(filePaths);
            var fileViewModel = new FileViewModel();

            fileViewModel.FilesPath = filePaths;
            fileViewModel.FilesName = fileNames;
            fileViewModel.FolderName = folderName;
            fileViewModel.FolderPath = patientFolderPath;
            fileViewModel.PatientCid = patientCid;
            fileViewModel.PatientName = patientName;
            ViewBag.UploadState = uploadState;

            return View(fileViewModel);
        }
        public FileResult Download(string link, string filename)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(link);
            string fileName = filename;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private void CreatePatientFolders(string patientCid, string patientName, string folderPath)
        {
            var path = @folderPath + @"\" + patientCid + "- " + patientName;
            if (!Directory.Exists(path))
            {
                Information(string.Format("We Just created the patient folders in the shared: [{0}]", folderPath), true);
                Information(string.Format("This is the link to the shared: [{0}]", path), true);
                // return;
            }
            try
            {

                var createdDirectory = Directory.CreateDirectory(path);
                var patientFolderPath = createdDirectory.FullName;

                //new for extension
                //var exesitingDirectories=Directory.GetDirectories(path);
                //var folderNames = ExtractNames(exesitingDirectories);
                //var isExtensionFolderExist = folderNames.Contains("EXTENSIONS");


                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.Allowances);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.Appointments);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.CompanionInfo);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.FuneralArrangement);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.GuaranteeLetters);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.IncomingMph);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.MedicalReports);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.OutgoingMph);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.PatientInfo);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.ProgressReports);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.TicketingTransportation);
                //new create extension folder
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.Extensions);
                Directory.CreateDirectory(@patientFolderPath + @"\" + PatientFolders.ExtensionsHistory);
            }
            catch (Exception e)
            {
                Danger(string.Format("An Error accured while trying to create patient folders, Try Again...!"), true);
            }


        }

        //new create extension folder
        private void CreatePatientSpecificFolder(string patientCid, string patientName, string folderPath, string folderName)
        {
            var path = @folderPath ;
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
    }
}