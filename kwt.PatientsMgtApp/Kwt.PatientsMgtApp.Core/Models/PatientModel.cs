using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PatientModel: BaseEntity
    {
        [DisplayName("Civil ID")]
        [Required, MaxLength(12)]
        public string PatientCID { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string PatientFName { get; set; }

        [DisplayName("Middle Name")]
        public string PatientMName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string PatientLName { get; set; }

        [DisplayName("Kuwait Phone#")]
        [MaxLength(12),Phone]
        public string KWTPhone { get; set; }

        [DisplayName("US Phone#"), Phone]
        public string USPhone { get; set; }

        [Required]
        public string Hospital { get; set; }
        [Required]
        public string Doctor { get; set; }
        public string Agency { get; set; }

        [DisplayName("Bank")]
        public string BankName { get; set; }

        [DisplayName("Bank Code")]
        public string BankCode { get; set; }

        [DisplayName("Bank Account#")]
        public string Iban { get; set; }

        [Required]
        [DisplayName("First Appointment Date")]
        public Nullable<System.DateTime> FirstApptDAte { get; set; }

        [DisplayName("End Treatment Date")]
        public Nullable<System.DateTime> EndTreatDate { get; set; }
        //public Nullable<decimal> PatientRate { get; set; }

        [Required]
        [DisplayName("Is Beneficiary?")]
        public Nullable<bool> IsBeneficiary { get; set; }
    
        [DisplayName("Is Active?")]
        [Required]
        public Nullable<bool> IsActive { get; set; }
        public string Notes { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created Date")]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [DisplayName("Modified Date")]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //========================
        public List<AgencyModel> Agencies { get; set; }
        public List<BankModel> Banks { get; set; }
        public List<DoctorModel> Doctors { get; set; }
        public List<HospitalModel> Hospitals { get; set; }
    }
}
