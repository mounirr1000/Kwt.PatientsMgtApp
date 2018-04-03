using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BankModel:BaseEntity
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }
}
