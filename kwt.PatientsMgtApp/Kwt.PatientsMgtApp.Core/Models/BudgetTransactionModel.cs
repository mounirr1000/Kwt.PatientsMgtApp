using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BudgetTransactionModel
    {
        public int ID { get; set; }
        public Nullable<decimal> BudgetAmount { get; set; }
        public Nullable<int> PayrollID { get; set; }
        public Nullable<int> DepositAccountID { get; set; }
    
    }
}
