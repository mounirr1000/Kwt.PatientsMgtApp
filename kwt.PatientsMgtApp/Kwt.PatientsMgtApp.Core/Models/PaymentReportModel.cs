using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PaymentReportModel
    {
        public int PaymentID { get; set; }
        public string PatientCID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string PatientName { get; set; }
        public string BeneficiaryCID { get; set; }
        public string BeneficiaryName { get; set; }
        public string CompanionName { get; set; }
        public string AgencyName { get; set; }
        public string IBan { get; set; }
        public string Bank { get; set; }
        public string Code { get; set; }

        public Nullable<decimal> FinalAmount { get; set; }
        public Nullable<decimal> DeductedAmount { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
