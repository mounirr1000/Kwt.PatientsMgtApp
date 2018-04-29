
namespace Kwt.PatientsMgtApp.PersistenceDB.EDMX
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Kwt.PatientsMgtApp.Core.Interfaces;

    //IAuditObject
    public partial class Agency : IDomainObject
    {
            public int Id
            {
                get { return this.AgencyID; } 

                set { this.AgencyID = value; }
            }
        
        }

    public partial class Bank : IDomainObject
    {
        public int Id
        {
            get { return this.BankID; }

            set { this.BankID = value; }
        }
    }
    public partial class ExceptionLogger : IDomainObject
    {
        
    }
    public partial class Beneficiary : IDomainObject
    {
        public int Id
        {
            get { return this.BeneficiaryID; }

            set { this.BeneficiaryID = value; }
        }
    }

    public partial class Companion : IDomainObject, IAuditObject
    {

    }

    public partial class CompanionHistory : IDomainObject, IAuditObject
    {

        public int Id
        {
            get { return this.HistoryID; }

            set { this.HistoryID = value; }
        }
    }

    public partial class CompanionType : IDomainObject
    {
        public int Id
        {
            get { return this.CompanionTypeID; }

            set { this.CompanionTypeID = value; }
        }
    }
    public partial class Specialty : IDomainObject
    {
        public int Id
        {
            get { return this.SpecialtyId; }

            set { this.SpecialtyId = value; }
        }
    }
    public partial class Doctor : IDomainObject
    {
        public int Id
        {
            get { return this.DoctorID; }

            set { this.DoctorID = value; }
        }
    }

    public partial class Hospital : IDomainObject
    {
        public int Id
        {
            get { return this.HospitalID; }

            set { this.HospitalID = value; }
        }
    }

    public partial class Patient : IDomainObject, IAuditObject
    {
        
    }

    public partial class Payment : IDomainObject, IAuditObject
    {
        public int Id
        {
            get { return this.PaymentID; }

            set { this.PaymentID = value; }
        }
    }

    public partial class PayRate : IDomainObject
    {
        public int Id
        {
            get { return this.PayRateID; }

            set { this.PayRateID = value; }
        }
    }

    public partial class Role : IDomainObject
    {
        public int Id
        {
            get { return Int16.Parse(this.RoleID); }

            set { this.RoleID = value.ToString(); }
        }
    }
    public partial class User : IDomainObject
    {
       
    }
    //public partial class UserRole : IDomainObject
    //{
       
    //}

    public partial class PatientHistory : IDomainObject
    {

    }
}

