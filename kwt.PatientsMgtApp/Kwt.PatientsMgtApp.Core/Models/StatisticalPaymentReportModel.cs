using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class StatisticalPaymentReportModel
    {
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public Nullable<decimal> KfhBank { get; set; }
        public Nullable<decimal> OtherBank { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> Rejected { get; set; }
        public Nullable<decimal> DAAgency { get; set; }
        public Nullable<decimal> OtherAgencies { get; set; }
        public Nullable<decimal> TotalDeduction { get; set; }
        public Nullable<decimal> FinalTotal { get; set; }
    }
}
