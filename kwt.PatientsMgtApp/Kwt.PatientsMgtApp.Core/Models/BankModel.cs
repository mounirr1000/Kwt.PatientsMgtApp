using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class BankModel:BaseEntity
    {
        
        [DisplayName("ID")]
        public int BankID { get; set; }

        [Required]
        [DisplayName("Bank Name")]
        [MaxLength(50, ErrorMessage = "Bank Name should not be more than 50 characters")]
        public string BankName { get; set; }

        [Required]
        [DisplayName("Bank Code")]
        [MaxLength(3, ErrorMessage = "Bank Code should not be more than 3 characters")]
        public string BankCode { get; set; }
    }
}
