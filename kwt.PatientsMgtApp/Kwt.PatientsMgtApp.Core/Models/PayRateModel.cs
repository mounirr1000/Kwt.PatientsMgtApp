using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
   public class PayRateModel:BaseEntity
    {

        [DisplayName("ID")]
        public int PayRateID { get; set; }
        [Required]
        [DisplayName("Companion Rate")]
        //[DataType(DataType.Custom)]
        public Nullable<decimal> CompanionRate { get; set; }

        [Required]
        [DisplayName("Patient Rate")]
        public Nullable<decimal> PatientRate { get; set; }
    }
}
