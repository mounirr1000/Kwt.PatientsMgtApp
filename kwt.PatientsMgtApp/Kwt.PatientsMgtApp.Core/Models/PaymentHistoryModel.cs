using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PaymentHistoryModel:BaseEntity
    {

        public int PaymentHistoryID { get; set; }
        public int PaymentID { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PaymentDateFormated { get { return String.Format("{0:d}", PaymentDate); } }
        public string PatientCID { get; set; }
        public string CompanionCID { get; set; }
        public Nullable<int> BeneficiaryID { get; set; }
        public string BeneficiaryPatientCid { get; set; }
        public string BeneficiaryCompanionCid { get; set; }
        public string BeneficiaryCID { get; set; }
        public string BeneficiaryFName { get; set; }
        public string BeneficiaryMName { get; set; }
        public string BeneficiaryLName { get; set; }
        public Nullable<int> BeneficiaryBankId { get; set; }
        public string BeneficiaryBankName { get; set; }
        public string BeneficiaryBankCode { get; set; }
        public string BeneficiaryBankIban { get; set; }
        public Nullable<int> HospitalID { get; set; }
        public string HospitalName { get; set; }
        public Nullable<int> AgencyID { get; set; }
        public string AgencyName { get; set; }
        public Nullable<int> PayRateID { get; set; }
        public Nullable<decimal> CompanionRate { get; set; }
        public Nullable<decimal> PatientRate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string StartDateFormated { get { return  String.Format("{0:d}", StartDate); } }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string EndDateFormated { get { return String.Format("{0:d}", EndDate); } }
        public Nullable<int> Period { get; set; }
        public Nullable<decimal> PAmount { get; set; }
        public Nullable<decimal> CAmount { get; set; }
        public Nullable<decimal> TotalDue { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedDateFormated { get { return String.Format("{0:d}", CreatedDate); } }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedDateFormated { get { return String.Format("{0:d}", ModifiedDate); } }
        public Nullable<decimal> FinalAmountAfterCorrection { get; set; }
        public Nullable<decimal> TotalCorrection { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public Nullable<int> RejectedPaymentId { get; set; }
        public Nullable<int> AdjustmentReasonID { get; set; }
        public string AdjustmentReason { get; set; }
        public  PaymentModel Payment { get; set; }
    }
}
