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
    public class PaymentDeductionTest
    {
        private readonly IPaymentDeductionRepository _paymentRepository = new PaymentDeductionRepository();

        [TestMethod]
        public void GetPaymentsTest()
        {
            var payments = _paymentRepository.GetPaymentDeductions();
            // Assert.IsNotNull(payments);
        }

        [TestMethod]
        public void GetPaymentDeductionByPaymentIdTest()
        {
            int paymentId = 22562;
            var payments = _paymentRepository.GetPaymentDeductionByPaymentId(paymentId);
             Assert.IsNotNull(payments);
        }

        [TestMethod]
        public void AddPaymentTets()
        {
            PaymentDeductionModel payment = new PaymentDeductionModel()
            {
                PayRateID = 1,
                PaymentID = 22562,
                PatientCID = "237111500038",
                //Agency = "H*",//payment.Patient?.AgencyID != null ? _domainObjectRepository.Get<Agency>(a => a.AgencyID == payment.Patient.AgencyID).AgencyName : null,
                CompanionAmount = 0,
                //BeneficiaryFName = "محمود",
                //BeneficiaryLName = "الفرج",
                //BeneficiaryMName = "عبد الخالق  حسين",
                //BeneficiaryBank = "Burgan Bank-008",
                //BeneficiaryIBan = "KW46BBYN0000000000000443478001",
                //CompanionCID = "278090300029",
                CreatedBy = "Mounir",
                CreatedDate = DateTime.Now,
                Notes = "test notes",
                DeductionEndDate = new DateTime(2019, 3, 10),//2018-04-15
                //Hospital = "BRIGHAM & WOMAN'S HOSPITAL / DANA-FARBER CANCER CENTER",
                ModifiedBy = null,
                ModifiedDate = null,
                PatientAmount = 0,
              //  CompanionPayRate = 25,
              //  PatientPayRate = 75,
                DeductionDate = DateTime.Now,
              //  PaymentLengthPeriod = 3,// DateTime.Now- new DateTime(2018, 4, 16),
                DeductionStartDate = new DateTime(2019, 3, 5),//2018-04-02
               // FinalAmount = 230,
                //BeneficiaryCID = "278090300029",
                //CompanionFName = "محمود",
                //CompanionLName = "الفرج",
                //CompanionMName = "عبد الخالق  حسين",
            };
            _paymentRepository.AddPaymentDeduction(payment);
        }
    }
}
