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
    
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            this.CompanionHistories = new HashSet<CompanionHistory>();
            this.Companions = new HashSet<Companion>();
            this.Payments = new HashSet<Payment>();
            this.PatientHistories = new HashSet<PatientHistory>();
            this.PaymentDeductions = new HashSet<PaymentDeduction>();
        }
    
        public int Id { get; set; }
        public string PatientCID { get; set; }
        public string PatientFName { get; set; }
        public string PatientMName { get; set; }
        public string PatientLName { get; set; }
        public string KWTphone { get; set; }
        public string USphone { get; set; }
        public Nullable<int> HospitalID { get; set; }
        public int DoctorID { get; set; }
        public Nullable<int> SpecialtyId { get; set; }
        public string Diagnosis { get; set; }
        public int AgencyID { get; set; }
        public Nullable<int> BankID { get; set; }
        public string Iban { get; set; }
        public Nullable<System.DateTime> FirstApptDate { get; set; }
        public Nullable<System.DateTime> EndTreatDate { get; set; }
        public Nullable<bool> IsBeneficiary { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> AuthorizedDate { get; set; }
        public Nullable<bool> IsBlocked { get; set; }
    
        public virtual Agency Agency { get; set; }
        public virtual Bank Bank { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompanionHistory> CompanionHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Companion> Companions { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Hospital Hospital { get; set; }
        public virtual Specialty Specialty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PatientHistory> PatientHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaymentDeduction> PaymentDeductions { get; set; }
    }
}
