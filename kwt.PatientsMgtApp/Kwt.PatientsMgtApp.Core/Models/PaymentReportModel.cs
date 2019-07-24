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
        //new 
        public int? TotalPayments { get; set; }
        public int? TotalPatients { get; set; }
        public decimal? TotalAmount { get; set; }

        // new 

        
        public Nullable<int> DeductionReason { get; set; }

        public string DeductionReasonText { get; set; }
        public Nullable<System.DateTime> CompanionDeductionStartDate { get; set; }
        public Nullable<System.DateTime> CompanionDeductionEndDate { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public string DeductionNotes { get; set; }
        public Nullable<System.DateTime> PatientDeductionStartDate { get; set; }
        public Nullable<System.DateTime> PatientDeductionEndDate { get; set; }
        public Nullable<decimal> CompanionDeduction { get; set; }
        public Nullable<decimal> PatientDeduction { get; set; }
        public Nullable<decimal> AmountBeforeDeduction { get; set; }

        // new 
        public Nullable<int> PaymentTypeId { get; set; }
        public Nullable<int> RejectedPaymentId { get; set; }
        public Nullable<bool> IsPaymentRejected { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string RejectionNotes { get; set; }
        public Nullable<int> RejectionReasonID { get; set; }
        public Nullable<System.DateTime> RejectionDate { get; set; }
        public String RejectionDateFormatted { get { return String.Format("{0:d}", RejectionDate); } }
        public string RejectionReason { get; set; }
        public Nullable<int> AdjustmentReasonID { get; set; }
        public string AdjustmentReason { get; set; }
        public Nullable<long> RowNumber { get; set; }

    }
}
