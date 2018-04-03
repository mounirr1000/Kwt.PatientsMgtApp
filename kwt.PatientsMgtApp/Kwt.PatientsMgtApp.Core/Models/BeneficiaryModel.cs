using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BeneficiaryModel   :  BaseEntity
    {
        //public int BeneficiaryID { get; set; }
        public string PatientCID { get; set; }
        public string CompanionCID { get; set; }
        public string BeneficiaryCID { get; set; }
        public string BeneficiaryFName { get; set; }
        public string BeneficiaryMName { get; set; }
        public string BeneficiaryLName { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }

        public string IBan { get; set; }
    }
}
