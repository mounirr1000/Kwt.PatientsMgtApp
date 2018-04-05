using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.WebUI.Models;

namespace Kwt.PatientsMgtApp.WebUI.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        private readonly IPatientRepository _patientRepository;
        private readonly IPatientManagmentRepository _patientManagmentRepository;

       public PatientController()
        {
            _patientRepository= new PatientRepository();
            _patientManagmentRepository= new PatientManagmentRepository();
        }
        public ActionResult Index()
        {

            return RedirectToAction("GetPatients");
        }

        

        public ActionResult GetPatients()
        {
           var patients=  _patientRepository.GetPatients();
          
            return View(patients);
        }

        public ActionResult Details(string patientCid)
        {
            var patient = _patientRepository.GetPatient(patientCid);

            if (patient != null)
            {
                return View(patient);
            }
            else return View("GetPatients");
        }

        [HttpGet]
        public ActionResult Create()
        {
            PatientModel patient = new PatientModel();
            patient.Agencies = _patientManagmentRepository.GetAgencies();
            patient.Banks = _patientManagmentRepository.GetBanks();
            patient.Hospitals = _patientManagmentRepository.GetHospitals();
            patient.Doctors = _patientManagmentRepository.GetDoctors();
                return View(patient);
        }
        [HttpPost]
        public ActionResult Create(PatientModel patient)
        {
            
            if (ModelState.IsValid)
            {
                _patientRepository.AddPatient(patient);
                return RedirectToAction("Index");
            }
            else
            {
                patient.Agencies = _patientManagmentRepository.GetAgencies();
                patient.Banks = _patientManagmentRepository.GetBanks();
                patient.Hospitals = _patientManagmentRepository.GetHospitals();
                patient.Doctors = _patientManagmentRepository.GetDoctors();
                return View(patient);
            }
                
        }
    }
}