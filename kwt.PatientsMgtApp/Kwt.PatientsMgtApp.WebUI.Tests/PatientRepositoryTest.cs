using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwt.PatientsMgtApp.WebUI.Tests
{
    [TestClass]
    public class PatientRepositoryTest
    {
        IPatientRepository patientRepository = new PatientRepository();

        [TestMethod]
        public void GetPatientListTest()
        {
           var patientsList= patientRepository.GetPatients();
            Assert.IsNotNull(patientsList);
        }

        [TestMethod]
        public void GetPatientReportTest()
        {
            var patientsList = patientRepository.GetPatientsReport(null, "BARROW NEUROSURGICAL ASSOCIATES", null, null, "NEUROLOGY");
           //var patientsList = patientRepository.GetPatientsReport(null, null, null, null, null);

            Assert.IsNotNull(patientsList);
        }
        [TestMethod]
        public void GetPatientTest()
        {
            var result = patientRepository.GetPatient("1234567891012");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddPatientRepoTest()
        {
            PatientModel p = new PatientModel()
            {
                Hospital = "Inova",
                BankName = "bank of america",
                Agency = "eo",
                Doctor = "ahmed hossien",
                IsActive = true,
                IsBeneficiary = true,
                PatientCID = "333432901010",
                BankCode = "001",
                CreatedBy = "Jemy",
                Iban = "3333222233333333",
                EndTreatDate = new DateTime(2019, 1, 1),
                Notes = "Some notes 2",
                PatientFName = "Mohamed",
                PatientLName = "Hasien",
                PatientMName = "Ahmed",
                KWTPhone = "3456783232",
                FirstApptDAte = DateTime.Now,
                USPhone = "5712222444",

            };
            patientRepository.AddPatient(p);
        }

        [TestMethod]
        public void UpdatePatientTest()
        {
            PatientModel p = new PatientModel()
            {
                Hospital = "Inova",
                BankName = "bank of america",
                Agency = "Mo",
                Doctor = "Tom",
                IsActive = false,
                IsBeneficiary = false,
                PatientCID = "345678901010",
                BankCode = "001",
                CreatedBy = "Jemy",
                Iban = "33333333333333333",
                EndTreatDate = new DateTime(2018, 2, 4),
                Notes = "Some notes 2 for updates",
                PatientFName = "Mohamed",
                PatientLName = "Hasbaoui",
                PatientMName = "",
                KWTPhone = "+3456789012",
                FirstApptDAte = new DateTime(2018, 1, 1),
                USPhone = "5712666666",

            };
            var result = patientRepository.UpdatePatient(p);

            
        }

        [TestMethod]
        public void DeletePatientTest()
        {
            int num = 0;
            PatientModel p = new PatientModel()
            {
                PatientCID = "123456789101"
            };
            var result = patientRepository.DeletePatient(p);
            Assert.AreNotEqual(num,result);
        }
    }
}



