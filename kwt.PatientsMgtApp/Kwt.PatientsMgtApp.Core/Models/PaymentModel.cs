using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PaymentModel :BaseEntity
    {
        
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PatientCID { get; set; }
        public string PatientFName { get; set; }
        public string PatientMName { get; set; }
        public string PatientLName { get; set; }
        public string CompanionCID { get; set; }
        public string CompanionFName { get; set; }
        public string CompanionMName { get; set; }
        public string CompanionLName { get; set; }

        public string BeneficiaryCID { get; set; }
        public string BeneficiaryFName { get; set; }
        public string BeneficiaryLName { get; set; }
        public string BeneficiaryBank { get; set; }
        public string BeneficiaryIBan { get; set; }
        public string BeneficiaryMName { get; set; }
        public string Hospital { get; set; }
        public string Agency { get; set; }
        public decimal? CompanionPayRate { get; set; }
        public decimal? PatientPayRate { get; set; }
        public Nullable<System.DateTime> PaymentStartDate { get; set; }
        public Nullable<System.DateTime> PaymentEndDate { get; set; }
        public Nullable<int> PaymentLengthPeriod { get; set; }
        public Nullable<decimal> PatientAmount { get; set; }
        public Nullable<decimal> CompanionAmount { get; set; }
        public Nullable<decimal> TotalDue { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
