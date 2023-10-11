using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class CheckPayrollModel
    {
        [DisplayName("Check Number")]
        public int CheckNumber { get; set; }
        [DisplayName("Voucher Number")]
        public int PayrollID { get; set; }
        [DisplayName("Check Number")]
        public string CheckNumberFormat { get; set; }
    }
}
