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
    public class PatientRepositoryTest
    {
            IPatientRepository patientRepository = new PatientRepository();

            [TestMethod]
            public void GetPatientTest()
            {
                var result = patientRepository.GetPatient("1234567891012");

                Assert.IsNotNull(result);
            }
         

    }
    }



