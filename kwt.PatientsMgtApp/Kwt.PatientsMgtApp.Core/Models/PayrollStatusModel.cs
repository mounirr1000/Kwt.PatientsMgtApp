using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayrollStatusModel
    {
        [DisplayName("Payroll Status ID")]
        public int PayrollStatusID { get; set; }

        [DisplayName("Payroll Status")]
        public string PayrollStatusName { get; set; }
    }
}
