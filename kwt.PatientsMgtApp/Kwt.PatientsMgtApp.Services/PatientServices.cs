using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PatientServices: IPatientServices
    {
        readonly IPatientRepository _patientRepository = new PatientRepository();
        public List<PatientModel> GetPatients()
        {
            return _patientRepository.GetPatients();
        }

        public PatientModel GetPatient(string patientcid)
        {

            return _patientRepository.GetPatient(patientcid);
                
        }

        public List<CompanionModel> GetPatientCompanions(string patientcid)
        {
            return _patientRepository.GetPatientCompanions(patientcid);
            
        }

        public void AddPatient(PatientModel patient)
        {
            _patientRepository.AddPatient(patient);
            
        }

        public PatientModel UpdatePatient(PatientModel patient)
        {
            return _patientRepository.UpdatePatient(patient);
            
        }

        public int DeletePatient(PatientModel patient)
        {
            return _patientRepository.DeletePatient(patient);
            
        }
    }
}
