using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class OvertimeModel
    {
        public int OvertimeID { get; set; }
        [DisplayName("Curent Salary")]
        public Nullable<decimal> BasedSalary { get; set; }

        public string BasedSalaryFormated { get { return string.Format("{0:0.00}", BasedSalary); } }

        [DisplayName("Hourly Rate")]
        public Nullable<decimal> HourlyRate { get; set; }

        [DisplayName("Regular Hours")]
        public Nullable<int> RegularHours { get; set; }

        [DisplayName("Overtime Hours Worked")]
        public Nullable<int> OvertimeHours { get; set; }
        [DisplayName("Overtime Amount")]
        public Nullable<decimal> CalculatedOverTimeAmount { get; set; }
        public int EmployeeID { get; set; }

        [DisplayName("Account Type")]
        public int OvertimePayAccountID { get; set; }
    }
}
