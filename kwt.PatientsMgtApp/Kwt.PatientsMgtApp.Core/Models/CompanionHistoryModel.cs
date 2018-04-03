using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class CompanionHistoryModel : BaseEntity
    {
        public int HistoryID { get; set; }
        public string CompanionCID { get; set; }
        public string PatientCID { get; set; }
        public Nullable<System.DateTime> DateIn { get; set; }
        public Nullable<System.DateTime> DateOut { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
