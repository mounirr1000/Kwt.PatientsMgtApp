using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class WirePayrollModel
    {
        [DisplayName("Wire Number")]
        public int WireNumber { get; set; }
        [DisplayName("Voucher Number")]
        public int PayrollID { get; set; }

        [DisplayName("Wire Number")]
        public string WireNumberFormat { get; set; }
    }
}
