using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
   public interface ICompanionRepository
    {
        List<CompanionModel> GetCompanions();

        CompanionModel GetCompanion(string companioncid);
        CompanionModel GetCompanion(string companioncid, string patientCid);

        CompanionModel GetCompanion(int id);
        CompanionModel GetPatientByCompanionCid(string companioncid);

        List<Companion> GetCompanionListByPatientCid(string patientcid);

        void AddCompanion(CompanionModel companion);

        CompanionModel UpdateCompanion(CompanionModel companion);

       Companion GetCompanionByPatientCid(string patientcid);
        int DeleteCompanion(string companionCid, string patientCid);

        int DeleteCompanion(int id);

        void DataMigrationToInsertIntoBeneficiaryTable();
    }
}
