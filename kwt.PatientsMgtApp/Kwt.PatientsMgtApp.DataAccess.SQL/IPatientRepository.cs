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

        List<PatientModel> GetActivePatients();

        List<PatientModel> GetHistoryPatients();

        PatientModel GetPatient(string patientcid);
        PatientModel GetSearchedPatient(string patientcid);
        List<CompanionModel> GetPatientCompanions(string patientcid);

        void AddPatient(PatientModel patient);

        PatientModel UpdatePatient(PatientModel patient);

        int DeletePatient(PatientModel patient);

        List<PatientReportModel> GetPatientsReport(string patientCid = null, string hospital = null, string doctor = null, Nullable<bool> status = null, string speciality = null, DateTime? startDate = null, DateTime? endDate = null, Nullable<bool> isDead = null);


    }
}
