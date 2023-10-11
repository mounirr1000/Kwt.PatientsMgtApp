using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayeesTypeModel
    {
        [DisplayName("Payee Type ID")]
        public int PayeeTypeID { get; set; }
        [DisplayName("Payee Type")]
        public string PayeeType { get; set; }
    }
}
