using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class SalaryDeductionModel
    {
        public int DeductionID { get; set; }
        public decimal BasedSalary { get; set; }
        public decimal HourlyRate { get; set; }
        public Nullable<int> DeductionPercentage { get; set; }
        public Nullable<decimal> DeductionAmount { get; set; }
        [DisplayName("Number Of Hours")]
        public Nullable<int> DeductedHours { get; set; }

        [DisplayName("Number Of Days")]
        public Nullable<int> DeductedDays { get; set; }
        public int EmployeeID { get; set; }
        public string DeductionType { get; set; }
        public List<string> DeductionTypes { get { return new List<string>() { "Hours", "Days" }; } }

    }
}
