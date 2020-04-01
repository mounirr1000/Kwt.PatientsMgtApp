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
    
    public partial class CompanionHistory
    {
        public int HistoryID { get; set; }
        public string CompanionCID { get; set; }
        public string Name { get; set; }
        public string PatientCID { get; set; }
        public Nullable<System.DateTime> DateIn { get; set; }
        public Nullable<System.DateTime> DateOut { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CompanionType { get; set; }
        public Nullable<bool> IsBeneficiary { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }
        public string EnglishComFName { get; set; }
        public string EnglishComMName { get; set; }
        public string EnglishComLName { get; set; }
    
        public virtual Companion Companion { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
