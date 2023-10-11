using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class DepositAccountModel
    {

        public int DepositID { get; set; }

        [DisplayName("Title")]
        public string DepositTitle { get; set; }

        [DisplayName("Deposit Date")]
        public System.DateTime DepositDate { get; set; }
        [DisplayName("Deposit Type")]
        public int DepositTypeID { get; set; }
        public string DepositType { get; set; }
        [DisplayName("Deposit Department")]
        public int DepositDepartmentID { get; set; }
        public string DepositDepartment { get; set; }

        [DisplayName("Agency")]
        public int AgencyID { get; set; }
        public string Agency { get; set; }

        [DisplayName("Payee")]
        public int PayeeID { get; set; }

        [DisplayName("Payee Name")]
        public string PayeeName { get; set; }

        [DisplayName("Amount")]
        public decimal AmountDeposited { get; set; }
        public string Descriptions { get; set; }
        [DisplayName("Account")]
        public Nullable<int> AccountID { get; set; }
        public string AccountName { get; set; }

        [DisplayName("Start Date")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DisplayName("End Date")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [DisplayName("Created Date")]
        public System.DateTime CreatedDate { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        public List<DepositTypeModel> DepositTypes { get; set; }
        public List<DepositDepartmentModel> DepositDepartments { get; set; }
        public List<PayrollAccountModel> PayrollAccounts { get; set; }
        public List<PayeeModel> PayeeList { get; set; }
        public List<AgencyModel> Agencies { get; set; }

    }
}
