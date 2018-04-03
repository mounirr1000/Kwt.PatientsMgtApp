using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
   public class PayRateModel:BaseEntity
    {
        public int PayRateID { get; set; }
        public Nullable<decimal> CompanionRate { get; set; }
        public Nullable<decimal> PatientRate { get; set; }
    }
}
