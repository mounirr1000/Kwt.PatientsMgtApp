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
    
    public partial class WirePayroll
    {
        public int WireNumber { get; set; }
        public int PayrollID { get; set; }
        public string WireNumberFormat { get; set; }
    
        public virtual Payroll Payroll { get; set; }
    }
}
