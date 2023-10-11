using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayrollMethodModel
    {
        [DisplayName("Payroll Method Id")]
        public int PayrollMethodId { get; set; }
        [DisplayName("Payroll Method")]
        public string PayrollMethodName { get; set; }
    }
}
