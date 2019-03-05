using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class CompanionTypeModel: BaseEntity
    {
        [Required]
        [DisplayName("Companion Type")]
        [MaxLength(12, ErrorMessage = "Companion Type Name should not be more than 12 characters")]
        public string CompanionType { get; set; }
    }
}
