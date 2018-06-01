using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class HospitalModel:BaseEntity
    {
        [DisplayName("Hospital ID")]
        public int HospitalID { get; set; }

        [Required]
        [DisplayName("Hospital Name")]
        [MaxLength(50, ErrorMessage = "Hospital Name should not be more than 50 characters")]
        public string HospitalName { get; set; }
    }
}
