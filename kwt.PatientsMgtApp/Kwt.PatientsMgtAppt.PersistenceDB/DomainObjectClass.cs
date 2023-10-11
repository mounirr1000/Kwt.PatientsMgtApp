
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
    //new
    public partial class DeductionReason : IDomainObject
    {
        public int Id
        {
            get { return this.ReasonId; }

            set { this.ReasonId = value; }
        }
    }//new
    public partial class BookType : IDomainObject
    {
        public int Id
        {
            get { return this.BookTypeID; }

            set { this.BookTypeID = value; }
        }
    }
    //new
    public partial class RejectionReason : IDomainObject
    {
        public int Id
        {
            get { return this.RejectionReasonID; }

            set { this.RejectionReasonID = value; }
        }
    }
    //new
    public partial class AdjustmentReason : IDomainObject
    {
        public int Id
        {
            get { return this.AdjustmentReasonID; }

            set { this.AdjustmentReasonID = value; }
        }
    }
    public partial class RejectedPayment : IDomainObject, IAuditObject
    {
        public int Id
        {
            get { return this.RejectionID; }

            set { this.RejectionID = value; }
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

    //new 
    public partial class PaymentDeduction : IDomainObject, IAuditObject
    {
        public int Id
        {
            get { return this.DeductionID; }

            set { this.DeductionID = value; }
        }
    }

    //new

    public partial class PaymentHistory : IDomainObject, IAuditObject
    {

        public int Id
        {
            get { return this.PaymentHistoryID; }

            set { this.PaymentHistoryID = value; }
        }
    }
    public partial class PaymentType : IDomainObject
    {
        public int Id
        {
            get { return this.PaymentTypeId; }

            set { this.PaymentTypeId = value; }
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
    // new payment app
    public partial class WirePayroll : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.WireNumber;
            }

            set
            {
                this.WireNumber = value;
            }
        }
    }
    public partial class Title : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class Salary : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.SalaryID; // this.Id;
            }

            set
            {
                this.Id = this.SalaryID;//value;
            }
        }
    }
    public partial class PayrollType : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.PayrollTypeID;
            }

            set
            {
                this.PayrollTypeID = value;
            }
        }
    }
    public partial class PayrollStatu : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.PayrollStatusID;
            }

            set
            {
                this.PayrollStatusID = value;
            }
        }
    }
    public partial class PayrollMethod : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.PayrollMethodId;
            }

            set
            {
                this.PayrollMethodId = value;
            }
        }
    }
    public partial class Payroll : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.TransactionID;
            }

            set
            {
                this.TransactionID = value;
            }
        }
    }
    public partial class PayeesType : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.PayeeTypeID;
            }

            set
            {
                this.PayeeTypeID = value;
            }
        }
    }
    public partial class Payee : IDomainObject
    {
        public int Id
        {
            get { return this.PayeeID; }

            set { this.PayeeID = value; }
        }
    }
    public partial class Employee : IDomainObject
    {
        public int Id
        {
            get { return this.EmployeeID; }

            set { this.EmployeeID = value; }
        }
    }
    public partial class DepartmentManager : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class DepartmentEmployee : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class Department : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class CheckPayroll : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.CheckNumber;
            }

            set
            {
                this.CheckNumber = value;
            }
        }
    }
    public partial class Account : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.AccountNumber;
            }

            set
            {
                this.AccountNumber = value;
            }
        }
    }

    public partial class Bonus : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.BonusID;
            }

            set
            {
                this.Id = this.BonusID;// = value;
            }
        }
    }
    public partial class EmployeeInsurance : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.InsuranceID;
            }

            set
            {
                this.InsuranceID = value;
            }
        }
    }
    public partial class Overtime : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.OvertimeID;
            }

            set
            {
                this.OvertimeID = value;
            }
        }
    }
    public partial class EmployeeAccountType : IDomainObject
    {

    }
    public partial class TaxCategory : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class InsuranceOption : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class InsuranceType : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class BonuseType : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.Id;
            }

            set
            {
                this.Id = value;
            }
        }
    }
    public partial class PayrollAccount : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.PayAccountID;
            }

            set
            {
                this.PayAccountID = value;
            }
        }
    }
    public partial class SocialStatus : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.SocialStatusID;
            }

            set
            {
                this.SocialStatusID = value;
            }
        }
    }
    public partial class TitleType : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.TitleTypeId;
            }

            set
            {
                this.TitleTypeId = value;
            }
        }
    }
    public partial class DiscountType : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.DiscountTypeId;
            }

            set
            {
                this.DiscountTypeId = value;
            }
        }
    }
    public partial class DepositType : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.DepositTypeID;
            }

            set
            {
                this.DepositTypeID = value;
            }
        }
    }
    public partial class DepositDepartment : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.DepositDepartmentID;
            }

            set
            {
                this.DepositDepartmentID = value;
            }
        }
    }
    public partial class DepositAccount : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.DepositID;
            }

            set
            {
                this.DepositID = value;
            }
        }
    }
    public partial class BudgetTransaction : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.ID;
            }

            set
            {
                this.ID = value;
            }
        }
    }
    public partial class PatientExtension : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.ExtensionId;
            }

            set
            {
                this.ExtensionId = value;
            }
        }
    }
    public partial class PatientExtensionHistory : IDomainObject
    {
        public int Id
        {
            get
            {
                return this.ExtensionHistoryId;
            }

            set
            {
                this.ExtensionHistoryId = value;
            }
        }
    }
}

