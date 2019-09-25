using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwt.PatientsMgtApp.WebUI.Tests
{
    [TestClass]
    public class PaymentRepositoryTest
    {

       readonly IPaymentRepository _paymentRepository = new PaymentRepository();
        [TestMethod]
        public void GetPaymentsTest()
        {
           var payments= _paymentRepository.GetPayments();
            Assert.IsNotNull(payments);
        }

        [TestMethod]
        public void GetNextPatientPayment()
        {
            int? numberOfDays = -3;
            var patientList = _paymentRepository.GetNextPatientPayment(numberOfDays);
            Assert.IsNotNull(patientList);

        }
        [TestMethod]
        public void GetPaymentByIdTest()
        {
            int id = 5;
            var payment=_paymentRepository.GetPaymentById(id);
            Assert.IsNotNull(payment);
        }
        [TestMethod]
        public void GetPaymentByCompanionTest()
        {
            string coampnionCId = "345678901021";
           var payments= _paymentRepository.GetPaymentsByCompanionCid(coampnionCId);
            Assert.IsNotNull(payments);
        }
        [TestMethod]
        public void GetPaymentByPatientTest()
        {
            string patientCId = "345678901010";
           var payments= _paymentRepository.GetPaymentsByPatientCid(patientCId);

            Assert.IsNotNull(payments);
        }

        [TestMethod]
        public void AddPaymentTets()
        {
            PaymentModel payment = new PaymentModel()
            {
                PatientCID = "345678901010",
                Agency = "H*",//payment.Patient?.AgencyID != null ? _domainObjectRepository.Get<Agency>(a => a.AgencyID == payment.Patient.AgencyID).AgencyName : null,
                CompanionAmount = 0,
                BeneficiaryFName = "New Boston",
                BeneficiaryLName = "Newman",
                BeneficiaryMName = "",
                BeneficiaryBank = "Burgan Bank-008",                        
                BeneficiaryIBan = "3333333333333",
                CompanionCID = "345678901021",
                CreatedBy = "Mounir",
                CreatedDate = DateTime.Now,
                Notes = "test notes",
                PaymentEndDate = new DateTime(2018,5,1),//2018-04-15
                Hospital = "BARROW NEUROSURGICAL ASSOCIATES",
                ModifiedBy = null,
                ModifiedDate = null,
                PatientAmount = 0,
                CompanionPayRate = 25,
                PatientPayRate = 75,
                PaymentDate = DateTime.Now,
                PaymentLengthPeriod = 3,// DateTime.Now- new DateTime(2018, 4, 16),
                PaymentStartDate = new DateTime(2018, 4, 16),//2018-04-02
                TotalDue = 230,                
                BeneficiaryCID = "345678901010",
                CompanionFName = "New Boston",
                CompanionLName = "Newman",
                CompanionMName = "",
            };
            _paymentRepository.AddPayment(payment);
        }

        [TestMethod]
        public void GetStatisticalPaymentsReport()
        {
            
            var payments = _paymentRepository.GetStatisticalPaymentsReport(new DateTime(2019,07,21), new DateTime(2019, 07, 23),1,4);

            Assert.IsNotNull(payments);
        }
    }
}
