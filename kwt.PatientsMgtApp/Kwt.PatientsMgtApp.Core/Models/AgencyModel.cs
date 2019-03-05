using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
      public class AgencyModel:BaseEntity
    {
        [DisplayName("ID")]
        public int AgencyID { get; set; }

        [Required]
        [DisplayName("Agency Name")]
        [MaxLength(50, ErrorMessage = "Agency Name should not be more than 50 characters")]
        public string AgencyName { get; set; }

    }
}
