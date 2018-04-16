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
    public class PatientManagmentRepositoryTest
    {

        IDomainObjectRepository _domainObjectRepository = new DomainObjectRepository();
        ISpecialityRepository _specialityRepository= new SpecialityRepository();
        [TestMethod]
        public void GetSpecialitiesTest()
        {
            var result = _specialityRepository.GetSpecialities();
        }
    }
}
