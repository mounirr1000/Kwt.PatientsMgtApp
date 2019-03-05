using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.Models;
using Kwt.PatientsMgtApp.WebUI.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Kwt.PatientsMgtApp.Core;


namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class AdminController : BaseController
    {

        private  ApplicationSignInManager _signInManager;
        private  ApplicationUserManager _userManager;
        private readonly DoctorRepository _doctorRepository;
        private readonly HospitalRepository _hospitalRepository;
        private readonly BankRepository _bankRepository;
        private readonly AgencyRepository _agencyRepository;
        private readonly SpecialityRepository _specialityRepository;
        private readonly PayRateRepository _payRateRepository;
        private readonly CompanionTypeRepository _companionTypeRepository;
        private readonly CompanionHistoryRepository _companionHistoryRepository;
        private readonly PatientHistoryRepository _patientHistoryRepository;
        public AdminController()
        {
            _doctorRepository = new DoctorRepository();
            _hospitalRepository =new HospitalRepository();
            _bankRepository = new BankRepository();
            _agencyRepository= new AgencyRepository();
            _specialityRepository= new SpecialityRepository();
            _payRateRepository = new PayRateRepository();
            _companionTypeRepository= new CompanionTypeRepository();
            _companionHistoryRepository = new CompanionHistoryRepository();
            _patientHistoryRepository= new PatientHistoryRepository();
        }

        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }


        public ActionResult List()
        {

            return View();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user,
                    model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }


        [HttpGet]
        public  ActionResult Edit(string id)
        {
            ApplicationUser user =  UserManager.FindById(id);
            if (user!=null)
            {
                return View(user);
            }
            Information(string.Format("There is no user in our records with id {0}",id),true);
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string userName, string password,bool lockoutEnabled, DateTime? lockoutEndDateUtc)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                user.UserName = userName;
                IdentityResult validEmail
                    = await UserManager.UserValidator.ValidateAsync(user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded
                        && password != string.Empty && validPass.Succeeded))
                {
                    user.LockoutEnabled = lockoutEnabled;
                    //Lock user for 100 Years
                    user.LockoutEndDateUtc = lockoutEndDateUtc;
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    
                    if (result.Succeeded)
                    {
                        Success(string.Format("User Account {0} was successfully updated",user.Email),true);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
              var result=  await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    Success(string.Format("<b>{0}</b> Was Successfully Deleted", user.Email));
                }
                else
                {
                    Information(string.Format("<b>{0}</b> Was Not Deleted", user.Email));
                }
            }
            else
            {
                Information(string.Format("No User was found with id  <b>{0}</b>", id));
            }
            return View("Index");
        }

        public async Task<ActionResult> LockoutUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.LockoutEnabled = true;
                //Lock user for 100 Years
                user.LockoutEndDateUtc= new DateTime(DateTime.Now.Year*200);
                
            }
            else
            {
                Information(string.Format("No User was found with id  <b>{0}</b>", id));
            }
            return View("Index");
        }
        public async Task<ActionResult> UnLockUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.LockoutEnabled = false;
                //Lock user for 100 Years
                user.LockoutEndDateUtc =null;

            }
            else
            {
                Information(string.Format("No User was found with id  <b>{0}</b>", id));
            }
            return View("Index");
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        //Doctors

        public ActionResult DoctorsList()
        {
            var doctors =_doctorRepository.GetDoctors();
            return View(doctors);
        }
        [HttpGet]
        public ActionResult CreateDoctor()
        {
            DoctorModel doctor = new DoctorModel();
            return View(doctor);
        }

        [HttpPost]
        public ActionResult CreateDoctor(DoctorModel doctor)
        {
            if (ModelState.IsValid)
            {
                _doctorRepository.AddDorctor(doctor);
            }
            else
            {
                return View(doctor);
            }
            return RedirectToAction("DoctorsList");
        }
        //Hospital

        public ActionResult HospitalsList()
        {
            var hospitals = _hospitalRepository.GetHospitals();
            return View(hospitals);
        }
        [HttpGet]
        public ActionResult CreateHospital()
        {
            HospitalModel hospital = new HospitalModel();
            return View(hospital);
        }

        [HttpPost]
        public ActionResult CreateHospital(HospitalModel hospital)
        {
            if (ModelState.IsValid)
            {
                _hospitalRepository.AddHospital(hospital);
            }
            else
            {
                return View(hospital);
            }
            return RedirectToAction("HospitalsList");
        }
        //Banks

        public ActionResult BanksList()
        {
            var banks = _bankRepository.GetBanks();
            return View(banks);
        }
        [HttpGet]
        public ActionResult CreateBank()
        {
            BankModel bank = new BankModel();
            return View(bank);
        }

        [HttpPost]
        public ActionResult CreateBank(BankModel bank)
        {
            ValidateBank(bank);
            if (ModelState.IsValid)
            {
                _bankRepository.AddBank(bank);
                return RedirectToAction("BanksList");
            }
            else
            { 
                return View(bank);
            }
           
        }

        private void ValidateBank(BankModel bank)
        {
            if (ModelState.IsValidField("BankCode")
               && !string.IsNullOrEmpty(bank.BankCode))
            {
                var banks = _bankRepository.GetBanks();
                if (banks.Any(c => c.BankCode.Trim() == bank.BankCode.Trim()))
                {
                    ModelState.AddModelError("BankCode", "This Bank Code is already in our records, please use different bank code");
                }
                
                    if (banks.Any(c => c.BankName.Trim() == bank.BankName.Trim()))
                {
                    ModelState.AddModelError("BankName", "This Bank Name is already in our records, please use different bank Name");
                }
            }
            
        }
        //Agencies

        public ActionResult AgenciesList()
        {
            var agencies = _agencyRepository.GetAgencies();
            return View(agencies);
        }
        [HttpGet]
        public ActionResult CreateAgency()
        {
            AgencyModel agency = new AgencyModel();
            return View(agency);
        }

        [HttpPost]
        public ActionResult CreateAgency(AgencyModel agency)
        {
 
            if (ModelState.IsValid)
            {
                _agencyRepository.AddAgency(agency);
                return RedirectToAction("AgenciesList");
            }
            else
            {
                return View(agency);
            }

        }
        //Specialties

        public ActionResult SpecialtiesList()
        {
            var specialties = _specialityRepository.GetSpecialities();
            return View(specialties);
        }
        [HttpGet]
        public ActionResult CreateSpecialty()
        {
            SpecialtyModel specialty = new SpecialtyModel();
            return View(specialty);
        }

        [HttpPost]
        public ActionResult CreateSpecialty(SpecialtyModel specialty)
        {

            if (ModelState.IsValid)
            {
                _specialityRepository.AddSpeciality(specialty);
                return RedirectToAction("SpecialtiesList");
            }
            else
            {
                return View(specialty);
            }

        }
        //PayRates

        public ActionResult PayRatesList()
        {
            var payRates = _payRateRepository.GetPayRatesList();
            return View(payRates);
        }
        [HttpGet]
        public ActionResult CreatePayRate()
        {
            PayRateModel payRate = new PayRateModel();
            return View(payRate);
        }

        [HttpPost]
        public ActionResult CreatePayRate(PayRateModel payRate)
        {

            if (ModelState.IsValid)
            {
                _payRateRepository.AddPayRate(payRate);
                return RedirectToAction("PayRatesList");
            }
            else
            {
                return View(payRate);
            }

        }
        //CompanionTypes

        public ActionResult CompanionTypesList()
        {
            var companionTypes = _companionTypeRepository.GetCompanionTypes();
            return View(companionTypes);
        }
        [HttpGet]
        public ActionResult CreateCompanionType()
        {
            CompanionTypeModel companionType = new CompanionTypeModel();
            return View(companionType);
        }

        [HttpPost]
        public ActionResult CreateCompanionType(CompanionTypeModel ct)
        {

            if (ModelState.IsValid)
            {
                _companionTypeRepository.AddCompanionType(ct);
                return RedirectToAction("CompanionTypesList");
            }
            else
            {
                return View(ct);
            }

        }
        //CompanionHistory

        public ActionResult CompanionHistoryList()
        {
            var companionHistory = _companionHistoryRepository.GetCompanionsHistory();
            return View(companionHistory);
        }
        //PatientHistory

        public ActionResult PatientHistoryList()
        {
            var patientHistory = _patientHistoryRepository.GetPatientsHistory();
            return View(patientHistory);
        }
    }
}