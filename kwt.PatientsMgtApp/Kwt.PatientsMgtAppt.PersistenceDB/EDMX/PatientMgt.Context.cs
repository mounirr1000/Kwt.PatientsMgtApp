﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PatientsMgtEntities : DbContext
    {
        public PatientsMgtEntities()
            : base("name=PatientsMgtEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Beneficiary> Beneficiaries { get; set; }
        public virtual DbSet<CompanionHistory> CompanionHistories { get; set; }
        public virtual DbSet<Companion> Companions { get; set; }
        public virtual DbSet<CompanionType> CompanionTypes { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<ExceptionLogger> ExceptionLoggers { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<PatientHistory> PatientHistories { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PayRate> PayRates { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Specialty> Specialties { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
    
        public virtual ObjectResult<GetPatientListReport_SP_Result> GetPatientListReport_SP(string pCid, string hospital, string doctor, Nullable<bool> status, string speciality)
        {
            var pCidParameter = pCid != null ?
                new ObjectParameter("pCid", pCid) :
                new ObjectParameter("pCid", typeof(string));
    
            var hospitalParameter = hospital != null ?
                new ObjectParameter("hospital", hospital) :
                new ObjectParameter("hospital", typeof(string));
    
            var doctorParameter = doctor != null ?
                new ObjectParameter("doctor", doctor) :
                new ObjectParameter("doctor", typeof(string));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(bool));
    
            var specialityParameter = speciality != null ?
                new ObjectParameter("speciality", speciality) :
                new ObjectParameter("speciality", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPatientListReport_SP_Result>("GetPatientListReport_SP", pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter);
        }
    
        public virtual ObjectResult<GetPaymentListReport_SP_Result> GetPaymentListReport_SP(string pCid, Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate)
        {
            var pCidParameter = pCid != null ?
                new ObjectParameter("pCid", pCid) :
                new ObjectParameter("pCid", typeof(string));
    
            var startDateParameter = startDate.HasValue ?
                new ObjectParameter("startDate", startDate) :
                new ObjectParameter("startDate", typeof(System.DateTime));
    
            var endDateParameter = endDate.HasValue ?
                new ObjectParameter("endDate", endDate) :
                new ObjectParameter("endDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPaymentListReport_SP_Result>("GetPaymentListReport_SP", pCidParameter, startDateParameter, endDateParameter);
        }
    }
}
