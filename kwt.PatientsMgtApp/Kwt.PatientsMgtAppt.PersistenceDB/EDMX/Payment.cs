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
    
    public partial class Payment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Payment()
        {
            this.PaymentDeductions = new HashSet<PaymentDeduction>();
        }
    
        public int PaymentID { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PatientCID { get; set; }
        public string CompanionCID { get; set; }
        public Nullable<int> BeneficiaryID { get; set; }
        public Nullable<int> HospitalID { get; set; }
        public Nullable<int> AgencyID { get; set; }
        public Nullable<int> PayRateID { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Period { get; set; }
        public Nullable<decimal> PAmount { get; set; }
        public Nullable<decimal> CAmount { get; set; }
        public Nullable<decimal> TotalDue { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<decimal> FinalAmountAfterCorrection { get; set; }
        public Nullable<decimal> TotalCorrection { get; set; }
    
        public virtual Beneficiary Beneficiary { get; set; }
        public virtual Companion Companion { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual PayRate PayRate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDeduction> PaymentDeductions { get; set; }
    }
}
