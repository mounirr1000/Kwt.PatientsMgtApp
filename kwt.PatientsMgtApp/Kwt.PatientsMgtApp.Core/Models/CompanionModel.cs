using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class CompanionModel : BaseEntity, IValidatableObject
    {
        [Required(ErrorMessage = "Companion Civil Id is required"),
         DisplayName("Companion Civil Id")]
        [MinLength(12, ErrorMessage = "Companion Civil Id should not be Less than 12 characters")]
        [MaxLength(12,ErrorMessage = "Companion Civil Id should not be more than 12 characters")]
        public string CompanionCID { get; set; }

        [Required(ErrorMessage = "Companion First Name is required"),
         DisplayName("First Name")]
        public string CompanionFName { get; set; }

        [DisplayName("Middle Name")]
        public string CompanionMName { get; set; }

        [Required(ErrorMessage = "Companion Last Name is required"),
         DisplayName("Last Name")]
        public string CompanionLName { get; set; }

        public string Name { get { return this.CompanionFName + " " + this.CompanionMName + " " + this.CompanionLName; } }

        [Required(ErrorMessage = "Patient Civil Id is required"),
         DisplayName("Patient Civil Id")]
        [MaxLength(12, ErrorMessage = "Patient Civil Id should not be more than 12 characters")]
        public string PatientCID { get; set; }

        [DisplayName("Bank Name")]
        public string BankName { get; set; }

        [DisplayName("Bank Code")]
        public string BankCode { get; set; }

        [DisplayName("IBan")]
        [MaxLength(30, ErrorMessage = "Bank Account should not be more than 30 characters")]
        [MinLength(30, ErrorMessage = "Bank Account should not be Less than 30 characters")]
        public string IBan { get; set; }

        [Required(ErrorMessage = "Companion Date entered is required"),
         DisplayName("Date In")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> DateIn { get; set; }
        public String DateInFormatted { get { return String.Format("{0:d}", DateIn); } }

        [DisplayName("Date Out")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> DateOut { get; set; }
        public String DateOutFormatted { get { return String.Format("{0:d}", DateOut); } }

        [Required(ErrorMessage = "Companion Is eithe active or inactive"),
         DisplayName("Active status")]
        public bool IsActive { get; set; } = true;

        [DisplayName("Is just Beneficiary?")]
        public Nullable<bool> JustBeneficiary { get; set; }

        [MaxLength(250, ErrorMessage = "Maximum characters allowed is 250 ")]
        public string Notes { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DisplayName("Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public System.DateTime CreatedDate { get; set; }
        public String CreatedDateFormatted { get { return String.Format("{0:d}", CreatedDate); } }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public String ModifiedDateFormatted { get { return String.Format("{0:d}", ModifiedDate); } }

        [Required(ErrorMessage = "Beneficiary field is required"),
         DisplayName("Is Beneficiary")]
        public bool IsBeneficiary { get; set; }

        [Required(ErrorMessage = "Companion Type field Is required"),
         DisplayName("Companion type")]
        public string CompanionType{ get; set; }

        //===============================
        public List<BankModel> Banks { get; set; }

        public List<CompanionTypeModel> CompanionTypes { get; set; }

        //=========================
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            if (IsBeneficiary == true)
            {
                if (String.IsNullOrEmpty(IBan) || String.IsNullOrEmpty(BankName))
                    errors.Add(new ValidationResult("Bank Information is required when the companion become beneficiary"));
            }
            if (!String.IsNullOrEmpty(IBan) || !String.IsNullOrEmpty(BankName))
            {
                if (IsBeneficiary == false)
                    errors.Add(new ValidationResult("Bank information are not needed since the companion is not beneficiary"));
            }
            if (IsActive == true && DateOut != null)
            {
                errors.Add(new ValidationResult("The companion has to be Inactive since the date out is set"));
            }
            if (DateOut == null && IsActive == false)
            {
                errors.Add(new ValidationResult("End Date out has to be set since the companion is no longer active"));
            }
            return errors;
        }
    }
}
