using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Infrastructure;
namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PatientModel : BaseEntity, IValidatableObject
    {
        [DisplayName("Civil ID")]
        [Required]
        [MaxLength(12, ErrorMessage = "patient Civil Id should not be more than 12 characters")]
        [MinLength(12, ErrorMessage = "patient Civil Id should not be Less than 12 characters")]
        public string PatientCID { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string PatientFName { get; set; }

        [DisplayName("Middle Name")]
        public string PatientMName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string PatientLName { get; set; }

        public string Name { get { return this.PatientFName + " " + this.PatientMName + " " + this.PatientLName; } }

        [DisplayName("Kuwait Phone#")]
        [Phone]
        public string KWTPhone { get; set; }

        [DisplayName("US Phone#"), Phone]
        [Required(ErrorMessage = "If there is No US phone, enter hospital's phone#")]
        public string USPhone { get; set; }

        [Required(ErrorMessage = "If the Hospital is not known yet, Select TBD")]
        public string Hospital { get; set; }
        [Required(ErrorMessage = "If the Doctor is not known yet, Select TBD")]
        public string Doctor { get; set; }
        [Required]
        public string Agency { get; set; }

        [DisplayName("Bank")]
        //[RequiredIf("IsBeneficiary",true, ErrorMessage = "Bank Name is required since the patient is Beneficiary")]
        public string BankName { get; set; }

        [DisplayName("Bank Code")]

        public string BankCode { get; set; }

        [DisplayName("Bank Account#")]
        [MaxLength(30, ErrorMessage = "Bank Account should not be more than 30 characters")]
        [MinLength(30, ErrorMessage = "Bank Account should not be Less than 30 characters")]
        //[RequiredIf("IsBeneficiary", true, ErrorMessage = "Bank Account is required since the patient is Beneficiary")]
        public string Iban { get; set; }

        [Required]
        [DisplayName("First Appointment Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> FirstApptDAte { get; set; }

        [DisplayName("End Treatment Date")]
        [DataType(DataType.Date)]
        //[RequiredIf("IsActive", true,ErrorMessage = "End Treatment Date is required since the patient is not active")]
        public Nullable<System.DateTime> EndTreatDate { get; set; }
        //public Nullable<decimal> PatientRate { get; set; }

        [Required(ErrorMessage = "If the patient is not Beneficiary, select No")]
        [DisplayName("Is Beneficiary?")]
        public bool IsBeneficiary { get; set; } = true;

        [DisplayName("Is Active?")]
        [Required(ErrorMessage = "The patient is either active or inactive, select one")]
        public bool IsActive { get; set; } = true;

        [MaxLength(250)]
        public string Notes { get; set; }

        [DisplayName("Specialty")]
        public string Specialty { get; set; }

        [DisplayName("Diagnosis")]
        public string Diagnosis { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CreatedDate { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //========================
        public List<AgencyModel> Agencies { get; set; }
        public List<BankModel> Banks { get; set; }
        public List<DoctorModel> Doctors { get; set; }
        public List<HospitalModel> Hospitals { get; set; }
        public List<SpecialtyModel> Sepcialities { get; set; }

        //==============================
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (IsBeneficiary == true)
            {
                if (String.IsNullOrEmpty(Iban) || String.IsNullOrEmpty(BankName))
                    errors.Add(new ValidationResult("Bank Information is required when the patient become beneficiary"));
            }
            if (!String.IsNullOrEmpty(Iban) || !String.IsNullOrEmpty(BankName))
            {
                if (IsBeneficiary == false)
                    errors.Add(new ValidationResult("Bank information are not needed since the patient is not beneficiary"));
            }
            if (IsActive == true && EndTreatDate != null)
            {
                errors.Add(new ValidationResult("The patient has to be Inactive since the end treatment date is set"));
            }
            if (EndTreatDate == null && IsActive == false)
            {
                errors.Add(new ValidationResult("End Treatment date has to be set since the patient is no longer active"));
            }
            return errors;
        }
    }
}
