using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class DoctorModel:BaseEntity
    {
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
    }
}
