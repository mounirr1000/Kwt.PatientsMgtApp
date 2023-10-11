using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
   public class DepositReportModel
    {
        public int DepositID { get; set; }
        public string DepositTitle { get; set; }
        public System.DateTime DepositDate { get; set; }
        public string DepositType { get; set; }
        public string DepositDepartment { get; set; }
        public string AgencyName { get; set; }
        public string PayeeName { get; set; }
        public decimal AmountDeposited { get; set; }
        public string Descriptions { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDare { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<decimal> totalAmount { get; set; }

    }
}
