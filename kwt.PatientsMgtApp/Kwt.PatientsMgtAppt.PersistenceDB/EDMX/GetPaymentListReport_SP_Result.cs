//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kwt.PatientsMgtApp.PersistenceDB.EDMX
{
    using System;
    
    public partial class GetPaymentListReport_SP_Result
    {
        public int PaymentID { get; set; }
        public string PatientCID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public System.DateTime CreatedDate { get; set; }
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
        public Nullable<int> DeductionReason { get; set; }
        public Nullable<System.DateTime> CompanionDeductionStartDate { get; set; }
        public Nullable<System.DateTime> CompanionDeductionEndDate { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public string DeductionNotes { get; set; }
        public Nullable<System.DateTime> PatientDeductionStartDate { get; set; }
        public Nullable<System.DateTime> PatientDeductionEndDate { get; set; }
        public Nullable<decimal> CompanionDeduction { get; set; }
        public Nullable<decimal> PatientDeduction { get; set; }
        public Nullable<decimal> AmountBeforeDeduction { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
        public Nullable<int> RejectedPaymentId { get; set; }
        public Nullable<bool> IsPaymentRejected { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string RejectionNotes { get; set; }
        public Nullable<int> RejectionReasonID { get; set; }
        public Nullable<System.DateTime> RejectionDate { get; set; }
        public string RejectionReason { get; set; }
        public Nullable<int> AdjustmentReasonID { get; set; }
        public string AdjustmentReason { get; set; }
        public Nullable<long> RowNumber { get; set; }
        public string DeductionReasonText { get; set; }
        public string IbanHash { get; set; }
        public Nullable<double> TotalHash { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsVoid { get; set; }
        public string PaymentBeneficiaryName { get; set; }
        public string PaymentIban { get; set; }
        public string PaymentBankName { get; set; }
        public string PaymentBankCode { get; set; }
        public string PaymentBeneficiaryCID { get; set; }
        public Nullable<decimal> SumTotalHash { get; set; }
    }
}
