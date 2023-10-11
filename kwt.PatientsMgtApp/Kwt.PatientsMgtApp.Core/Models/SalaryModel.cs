using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class SalaryModel
    {
        [DisplayName("Hire Date")]
       // [Required]
        public int EmployeeID { get; set; }

        [DisplayName("Current Salary")]
        public decimal Salary1 { get; set; }
        public string Salary { get { return string.Format("{0:0.00}", Salary1); } }

        [DisplayName("From Date")]
        public System.DateTime? FromDate { get; set; }

        [DisplayName("To Date")]
        public System.DateTime? ToDate { get; set; }

        [DisplayName("Starting Salary")]
        public decimal StartingSalary { get; set; }
        public string StartingSalaryFormated { get { return string.Format("{0:0.00}", StartingSalary); } }

        [DisplayName("Yearly Salary Incremnt")]
        public Nullable<decimal> YearlySalaryIncrement { get; set; }

        [DisplayName("Tax Category")]
        public Nullable<int> TaxCategoryID { get; set; }

        [DisplayName("Account Type")]
        public int SalaryPayAccountID { get; set; }

        public int SalaryID { get; set; }
        public Nullable<decimal> ExperienceSalary { get; set; }
        public Nullable<decimal> ExperienceWithInsuranceSalary { get; set; }
        public Nullable<decimal> ExperienceWithTaxSalary { get; set; }
        public Nullable<decimal> TotalSalary { get; set; }
    }
}
