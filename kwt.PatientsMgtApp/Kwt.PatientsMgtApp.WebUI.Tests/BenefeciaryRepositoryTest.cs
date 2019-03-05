using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwt.PatientsMgtApp.WebUI.Tests
{
    [TestClass]
    public class BenefeciaryRepositoryTest
    {
        IBeneficiaryRepository _beneficiaryRepository= new BeneficiaryRepository();
        [TestMethod]
        public void GetBeneficiariesTest()
        {

          var benList=  _beneficiaryRepository.GetBeneficiaries();
        }

        [TestMethod]
        public void GetBeneficiaryTest()
        {
            string patientCid = "333432901010";//333432901010
            var benList = _beneficiaryRepository.GetBeneficiary(patientCid);
        }
    }
}
