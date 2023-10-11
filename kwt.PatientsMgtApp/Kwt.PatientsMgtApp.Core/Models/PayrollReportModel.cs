using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PayrollReportModel
    {
        public string Agency { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<decimal> Total { get; set; }
        public decimal Amount { get; set; }
        public int TransactionID { get; set; }
        public string TransactionIDFormat { get; set; }
        public string PaymentMethod { get; set; }
        public string PayrollStatus { get; set; }
        public string PayrollType { get; set; }
        public string PayeeName { get; set; }
        public string EmployeeName { get; set; }
        public string PayeeType { get; set; }
        public string AccountName { get; set; }
        public string CheckNumber { get; set; }
        public Nullable<decimal> AccountAmount { get; set; }
        public Nullable<decimal> DepositedActualBalance { get; set; }
        public Nullable<decimal> TotalAmountByStatus { get; set; }
        public Nullable<decimal> OutstandingChecksAndWires { get; set; }
        public System.DateTime DueDate { get; set; }

    }
}
