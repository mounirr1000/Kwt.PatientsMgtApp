using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayrollAccountModel
    {
        public int PayAccountID { get; set; }
        public int Title { get; set; }
        public int Parent { get; set; }
        public string Description { get; set; }
        public string Grouping { get; set; }
        public string AccountType { get; set; }

    }
}
