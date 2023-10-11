using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class TitleModel
    {
        public int EmployeeID { get; set; }
        public string Title1 { get; set; }
        public System.DateTime FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }

        public int TitleID { get; set; }
        public int TitleTypeId { get; set; }
    }
}
