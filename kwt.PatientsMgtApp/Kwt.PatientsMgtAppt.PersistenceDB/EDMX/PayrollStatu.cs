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
    
    public partial class PayrollStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PayrollStatu()
        {
            this.Payrolls = new HashSet<Payroll>();
        }
    
        public int PayrollStatusID { get; set; }
        public string PayrollStatusName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payroll> Payrolls { get; set; }
    }
}