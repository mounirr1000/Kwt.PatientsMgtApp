using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayrollModel
    {
        [DisplayName("Voucher Number")]
        public int TransactionID { get; set; }
        [DisplayName("Voucher Number")]
        public string TransactionIDFormat { get; set; }
        [DisplayName("Payroll Method")]
        [Required]       
        public int PayrollMethodId { get; set; }// wire, check
        [DisplayName("Payroll Status ID")]
        [Required]
        public int PayrollStatusID { get; set; }// entered, approved, authorized, paid , reconciled
        [DisplayName("Payroll Type ID")]
        [Required]
        public int PayrollTypeID { get; set; }// salary, overtime, Bonus, Severance, Contract, Other
        [DisplayName("Agency ID")]
        public Nullable<int> AgencyID { get; set; }
        [DisplayName("Payee ID")]
        public Nullable<int> PayeeID { get; set; }
        [DisplayName("Employee ID")]
        public Nullable<int> EmployeeID { get; set; }
        [DisplayName("Account Number")]
        public Nullable<int> AccountNumber { get; set; }
        [DisplayName("Payee Name")]
        [Required]
        public string PayeeName { get; set; }
        [DisplayName("Payee Type ID")]
        [Required]
        public int PayeeTypeID { get; set; }
        
        public decimal Amount { get; set; }
        public string AmountFormat { get { return Amount.ToString("0.##"); } }
        public string AlphaAmount { get; set; }
        public string AmountFormated { get { return Amount.ToString("C2"); } }
        public string CheckAndAccountNumberFormat { get; set; }
        [DisplayName("Payment Due Date")]
        [Required]
        public System.DateTime PaymentDueDate { get; set; }
        public String PaymentDueDateFormatted { get { return String.Format("{0:d}", PaymentDueDate); } }
        [DisplayName("Payment Created Date")]
        public System.DateTime PaymentCreatedDate { get; set; }
        public String PaymentCreatedDateFormatted { get { return String.Format("{0:d}", PaymentCreatedDate); } }
        [DisplayName("Wire Number")]
        public Nullable<int> WireNumber { get; set; }
        [DisplayName("Wire Number")]
        public string WireNumberFormat { get { return WireNumber != null ? "W" + WireNumber : string.Empty; } set { } }
        [DisplayName("Check Number")]
        public Nullable<int> CheckNumber { get; set; }
        [DisplayName("Check Number")]
        public string CheckNumberFormat { get { return CheckNumber != null ? "C" + CheckNumber : string.Empty; } set { } }
        [DisplayName("Payment Created By")]
        public string PaymentCreatedBy { get; set; }
        [DisplayName("Payment Entered By")]
        public string PaymentEnteredBy { get; set; }
        [DisplayName("Payment Entered Date")]
        public System.DateTime PaymentEnteredDate { get; set; }
        public String PaymentEnteredDateFormatted { get { return String.Format("{0:d}", PaymentEnteredDate); } }
        [DisplayName("Payment Approved By")]
        public string PaymentApprovedBy { get; set; }
        [DisplayName("Payment Approved Date")]
        public Nullable<System.DateTime> PaymentApprovedDate { get; set; }
        public String PaymentApprovedDateFormatted { get { return String.Format("{0:d}", PaymentApprovedDate); } }
        [DisplayName("Payment Authorized By")]
        public string PaymentAuthorizedBy { get; set; }
        [DisplayName("Payment Authorized Date")]
        public Nullable<System.DateTime> PaymentAuthorizedDate { get; set; }
        public String PaymentAuthorizedDateFormatted { get { return String.Format("{0:d}", PaymentAuthorizedDate); } }
        [DisplayName("Payment Paid By")]
        public string PaymentPaidBy { get; set; }
        [DisplayName("Payment Paid Date")]
        public Nullable<System.DateTime> PaymentPaidDate { get; set; }
        public String PaymentPaidDateDateFormatted { get { return String.Format("{0:d}", PaymentPaidDate); } }
        [DisplayName("Payment Reconciled By")]
        public string PaymentReconciledBy { get; set; }
        [DisplayName("Payment Reconciled Date")]
        public Nullable<System.DateTime> PaymentReconciledDate { get; set; }
        public String PaymentReconciledDateFormatted { get { return String.Format("{0:d}", PaymentReconciledDate); } }
        [DisplayName("Payment Modified Date")]
        public Nullable<System.DateTime> PaymentModifiedDate { get; set; }
        public String PaymentModifiedDateFormatted { get { return String.Format("{0:d}", PaymentModifiedDate); } }
        [DisplayName("Payment Modified By")]
        public string PaymentModifiedBy { get; set; }
        public string Descriptions { get; set; }

        [DisplayName("Payment Update By")]
        public string UpdatedBy { get; set; }

        [DisplayName("Payment Updated Date")]
        public DateTime? UpdateDate { get; set; }
        public String UpdateDateFormatted { get { return String.Format("{0:d}", UpdateDate); } }

        [DisplayName("Payment Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Payment Created Date")]
        public DateTime? CreatedDate { get; set; }
        public String CreatedDateFormatted { get { return String.Format("{0:d}", CreatedDate); } }

        public Nullable<System.DateTime> StartDate { get; set; }
        public String StartDateFormatted { get { return String.Format("{0:d}", StartDate); } }
        public Nullable<System.DateTime> EndDate { get; set; }
        public String EndDateFormatted { get { return String.Format("{0:d}", EndDate); } }
        public AccountModel Account { get; set; }
        public  AgencyModel Agency { get; set; }
       
        public  ICollection<CheckPayrollModel> CheckPayrolls { get; set; }
        public  PayeeModel Payee { get; set; }
        public  PayeesTypeModel PayeesType { get; set; }
        public  PayrollMethodModel PayrollMethod { get; set; }
        public  PayrollStatusModel PayrollStatu { get; set; }
        public  PayrollTypeModel PayrollType { get; set; }
        
        public  ICollection<WirePayrollModel> WirePayrolls { get; set; }
        public  EmployeeModel Employee { get; set; }

        public List<PayeeModel> PayeeList { get; set; }
        public List<EmployeeModel> EmployeeList { get; set; }
        public List<AgencyModel> Agencies { get; set; }

        public List<PayrollMethodModel> PayrollModethodList { get; set; }
        public virtual List<AccountModel> Accounts { get; set; }

        public WirePayrollModel WirePayroll { get; set; }
        public CheckPayrollModel CheckPayroll { get; set; }
        public List<PayrollStatusModel> PayrollStatusList { get; set; }//entered, Approved, Authorized, Paid, Reconciled
    }
}
