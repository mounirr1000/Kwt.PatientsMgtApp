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
    }
}
