using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PatientExtensionHistoryModel
    {
        public int ExtensionHistoryId { get; set; }
        public int ExtensionId { get; set; }
        public string PatientCID { get; set; }
        public System.DateTime ExtensionStartDate { get; set; }
        public System.DateTime ExtensionEndDate { get; set; }
        public string ExtensionDocLink { get; set; }
        public Nullable<bool> IsPaid { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string FileName { get; set; }
    }
}
