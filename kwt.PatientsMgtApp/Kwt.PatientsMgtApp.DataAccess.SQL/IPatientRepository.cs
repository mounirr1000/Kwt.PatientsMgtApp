using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IPatientRepository
    {
        List<PatientModel> GetPatients();

        PatientModel GetPatient(string patientcid);

        List<CompanionModel> GetPatientCompanions(string patientcid);

        void AddPatient(PatientModel patient);

        PatientModel UpdatePatient(PatientModel patient);

        int DeletePatient(PatientModel patient);


    }
}
