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
        public string Name { get; set; }
        public string EnglishComFName { get; set; }        
        public string EnglishComMName { get; set; }        
        public string EnglishComLName { get; set; }

        public string PatientCID { get; set; }
        public Nullable<System.DateTime> DateIn { get; set; }
        public String DateInFormatted { get { return String.Format("{0:d}", DateIn); } }

        public Nullable<System.DateTime> DateOut { get; set; }
        public String DateOutFormatted { get { return String.Format("{0:d}", DateOut); } }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CompanionType { get; set; }
        public Nullable<bool> IsBeneficiary { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }
    }
}
