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
    using System.Collections.Generic;
    
    public partial class PaymentDeduction
    {
        public int DeductionID { get; set; }
        public int PaymentID { get; set; }
        public Nullable<System.DateTime> DeductionDate { get; set; }
        public string PatientCID { get; set; }
        public string CompanionCID { get; set; }
        public Nullable<int> BeneficiaryID { get; set; }
        public Nullable<int> PayRateID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<decimal> PDeduction { get; set; }
        public Nullable<decimal> CDeduction { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public Nullable<System.DateTime> PatientStartDate { get; set; }
        public Nullable<System.DateTime> PatientEndDate { get; set; }
        public Nullable<System.DateTime> CompanionStartDate { get; set; }
        public Nullable<System.DateTime> CompanionEndDate { get; set; }
        public Nullable<decimal> PatientDeductionRate { get; set; }
        public Nullable<decimal> CompanionDeductionRate { get; set; }
        public Nullable<int> ReasonId { get; set; }
    
        public virtual Beneficiary Beneficiary { get; set; }
        public virtual Companion Companion { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual PayRate PayRate { get; set; }
        public virtual DeductionReason DeductionReason { get; set; }
    }
}