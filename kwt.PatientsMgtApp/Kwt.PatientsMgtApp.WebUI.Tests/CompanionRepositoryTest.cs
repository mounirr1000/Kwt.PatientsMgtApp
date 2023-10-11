using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Kwt.PatientsMgtApp.Core.Models;
//using Kwt.PatientsMgtApp.DataAccess.SQL;
//using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Kwt.PatientsMgtApp.WebUI.Tests
{
    [TestClass]
    public class CompanionRepositoryTest
    {
       // ICompanionRepository _companionRepository = new CompanionRepository();

        [TestMethod]

        public void GetCompanionListTest()
        {
           // var result = _companionRepository.GetCompanions();
        }
        [TestMethod]

        public void AddCompanionTest()
        {
            //CompanionModel newCompanion = new CompanionModel()
            //{
            //    CompanionCID = "345678901021",
            //    CompanionFName = "Newton",
            //    CompanionMName = "tim",
            //    CompanionLName = "Newoman",
            //    CompanionType = "secondary",
            //    DateIn = new DateTime(2018,3,3),
            //    DateOut = null,
            //    IsActive = true,
            //    IBan = "2222222225555",
            //    BankName = "bank of america",
            //    IsBeneficiary = false,
            //    Notes = "Some notes from companion",
            //    PatientCID = "345678901010",
            //    CreatedBy = "Mounir",
            //};
            //_companionRepository.AddCompanion(newCompanion);
        }

        [TestMethod]

        public void UpdateCompanionTest()
        {
            //CompanionModel companion = new CompanionModel()
            //{
            //    CompanionCID = "345345901122",
            //    CompanionFName = "Newton",
            //    CompanionMName = "tim",
            //    CompanionLName = "Newoman",
            //    CompanionType = "primary",
            //    DateIn = new DateTime(2018, 3, 3),
            //    DateOut = null,
            //    IsActive = true,
            //    IBan = "2222222225555",
            //    BankName = "bank of america",
            //    IsBeneficiary = false,
            //    Notes = "Some notes from companion",
            //    PatientCID = "333432901010",
            //    CreatedBy = "Mounir",
            //};
            //_companionRepository.UpdateCompanion(companion);
        }
        [TestMethod]
       public void GetCompanionTest()
        {          
            //Assert.IsNotNull(_companionRepository.GetCompanion("3456789010215"));
        }

        [TestMethod]
        public void DeleteCompanion()
        {
          //  _companionRepository.DeleteCompanion("345345901122", "333432901010");
        }

        [TestMethod]
        public void DataMigration()
        {
//_companionRepository.DataMigrationToInsertIntoBeneficiaryTable();
        }
    }
}
