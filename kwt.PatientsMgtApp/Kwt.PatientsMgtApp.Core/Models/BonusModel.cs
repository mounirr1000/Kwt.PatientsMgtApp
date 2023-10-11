using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BonusModel
    {
        public int BonusID { get; set; }
        [DisplayName("Current Salary")]
        [Required]
        public decimal BasedSalary { get; set; }
        public string BasedSalaryFormated { get { return string.Format("{0:0.00}", BasedSalary); } }

        [DisplayName("Bonus Type ID")]
        public Nullable<int> BonusTypeID { get; set; }

        [DisplayName("Bonus")]
        public Nullable<int> BonusValue { get; set; }

        [DisplayName("Bonus Final Amount")]
        public Nullable<int> BonusFinalAmount { get; set; }
        public int EmployeeID { get; set; }
        [DisplayName("Account Type")]
        public int BonusPayAccountID { get; set; }
    }
}
