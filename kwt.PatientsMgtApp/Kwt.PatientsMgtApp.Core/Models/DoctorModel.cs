using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class DoctorModel:BaseEntity
    {
        [DisplayName("ID")]
        public int DoctorID { get; set; }


        [Required]
        [DisplayName("Doctor Full Name")]
        [MaxLength(50, ErrorMessage = "Doctor Name should not be more than 50 characters")]
        public string DoctorName { get; set; }
    }
}
