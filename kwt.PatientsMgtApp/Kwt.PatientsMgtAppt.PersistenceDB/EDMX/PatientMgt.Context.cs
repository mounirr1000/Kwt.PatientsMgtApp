﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kwt.PatientsMgtAppt.PersistenceDB.EDMX
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
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
        public virtual DbSet<CompanionType> CompanionTypes { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PayRate> PayRates { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CompanionHistory> CompanionHistories { get; set; }
        public virtual DbSet<Companion> Companions { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
    }
}
