using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class EmployeeInsuranceModel
    {
        public int InsuranceID { get; set; }
        [DisplayName("Current Salary")]
        [Required]
        public decimal BasedSalary { get; set; }

        public string BasedSalaryFormated { get { return string.Format("{0:0.00}", BasedSalary); } }

        [DisplayName("Insuarnce Type ID")]
        public Nullable<int> InsuranceTypeID { get; set; }
        public int InsuranceOptionID { get; set; }

        [DisplayName("Insuarnce Amount")]
        public int InsuranceAmount { get; set; }
        public int EmployeeID { get; set; }

        [DisplayName("Account Type")]
        public int InsurancePayAccountID { get; set; }
        
    }
}
