using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class SpecialtyModel
    {
        [DisplayName("ID")]
        public int SpecialtyId { get; set; }

        [Required]
       
        [MaxLength(100, ErrorMessage = "Speciality should not be more than 100 characters")]
        public string Speciality { get; set; }
    }
}
