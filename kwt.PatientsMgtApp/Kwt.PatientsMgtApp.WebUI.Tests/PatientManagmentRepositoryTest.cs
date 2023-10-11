using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Kwt.PatientsMgtApp.DataAccess.SQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kwt.PatientsMgtApp.WebUI.Tests
{
    [TestClass]
    public class PatientManagmentRepositoryTest
    {

        //IDomainObjectRepository _domainObjectRepository = new DomainObjectRepository();
        //ISpecialityRepository _specialityRepository= new SpecialityRepository();
        [TestMethod]
        public void GetSpecialitiesTest()


        {
            //var result = _specialityRepository.GetSpecialities();
            //var salary = employee.Salary;
            var baseSalary = 3500;
            decimal? calculatedSalary = baseSalary;

            int taxCategoryId = 3; // 1 SSN * 7.065%  --2 US TAX * 30% --3 None * 0 
            var taxCaregoryValue = 0.00;
            var increaseAmount = 100;//salary.YearlySalaryIncrement;
            bool isBetweenJan1_June30 = false;
            bool isBetweenJuly1_Dec30 = false;
            bool isExperienceOverAYear = false;
            int increaseMonth = 0;
            int increaseYearStart = 0;
            DateTime increaseDateStart = new DateTime();
            var hireDate = new DateTime(2015,6,30);// exp: 6/30/2015

            var hireMonth = hireDate.Month;
            var hireDay = hireDate.Day;
            var hireYear = hireDate.Year;
            var todayDate = DateTime.Now;
            var daysOfExperience = (todayDate - hireDate);
            var yearsOfexperience = daysOfExperience != null ? (daysOfExperience.Days / 365) : 0;
            DateTime Jan1 = new DateTime(hireYear, 1, 1);
            DateTime June30 = new DateTime(hireYear, 6, 30);
            DateTime July1 = new DateTime(hireYear, 7, 1);
            DateTime Dec31 = new DateTime(hireYear, 12, 31);

            if (hireDate <= June30 && hireDate >= Jan1)
            {
                isBetweenJan1_June30 = true;
            }
            else if (hireDate <= Dec31 && hireDate >= July1)
            {
                isBetweenJuly1_Dec30 = true;
            }

            if (yearsOfexperience >= 1)// over a year of experince
            {
                isExperienceOverAYear = true;
            }

            if (isExperienceOverAYear && isBetweenJan1_June30)
            {
                increaseMonth = 7;
                increaseYearStart = hireYear + 1;
                increaseDateStart = new DateTime(increaseYearStart, increaseMonth, 1);
            }
            if (isExperienceOverAYear && isBetweenJuly1_Dec30)
            {
                increaseMonth = 1;
                increaseYearStart = hireYear + 2;
                increaseDateStart = new DateTime(increaseYearStart, increaseMonth, 1);
            }
            var daysDiff = (todayDate - increaseDateStart);
            var numOfDays = daysDiff.Days;
            var yearsOfIncrease = numOfDays / 365;
            if (isExperienceOverAYear)
            {
                for (int j = 0; j <= yearsOfIncrease; j++)// we include the first year
                {
                    calculatedSalary += increaseAmount;
                }
            }

            if (taxCategoryId != 0)
            {
                calculatedSalary += (calculatedSalary * (decimal)taxCaregoryValue)/100;
            }
                
        }
    }
}
