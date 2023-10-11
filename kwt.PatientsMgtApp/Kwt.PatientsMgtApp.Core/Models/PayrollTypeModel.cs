using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayrollTypeModel
    {
        [DisplayName("Payroll Type ID")]
        public int PayrollTypeID { get; set; }

        [DisplayName("Payroll Type")]
        public string PayrollTypeName { get; set; }
    }
}
