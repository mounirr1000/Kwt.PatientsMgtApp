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

        [DisplayName("Has Companion")]
        [Required]
        public bool HasCompanion { get; set; } 

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

        [DisplayName("IBan")]
        [MaxLength(30, ErrorMessage = "IBan should not be more than 30 characters")]
        [MinLength(30, ErrorMessage = "IBan should not be Less than 30 characters")]
        //[RequiredIf("IsBeneficiary", true, ErrorMessage = "Bank Account is required since the patient is Beneficiary")]
        public string Iban { get; set; }

        [Required]
        [DisplayName("First Appointment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> FirstApptDAte { get; set; }

        [DisplayName("Appointment Date")]
        public String FirstApptDAteFormatted { get { return String.Format("{0:d}", FirstApptDAte); } }

        public String FirstApptDAteLongFormatted { get { return String.Format("{0:d-MMMM-yyyy}", FirstApptDAte); } }
        public String FirstApptDAteLongArabicFormatted { get { return String.Format(new System.Globalization.CultureInfo("ar-KW"), "{0:yyyy, MMMM dd}", FirstApptDAte); } }//   yyyy ,MMMM dd
        [DisplayName("End Treatment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[RequiredIf("IsActive", true,ErrorMessage = "End Treatment Date is required since the patient is not active")]
        public Nullable<System.DateTime> EndTreatDate { get; set; }
        //public Nullable<decimal> PatientRate { get; set; }
        public String EndTreatDateFormatted { get { return String.Format("{0:d}", EndTreatDate); } }
    
        public String EndTreatDateLongFormatted { get { return String.Format("{0:d-MMMM-yyyy}", EndTreatDate); } }
        public String EndTreatDateLongArabicFormatted { get { return String.Format(new System.Globalization.CultureInfo("ar-KW"), "{0:yyyy, dd MMMM}", EndTreatDate); } }
        [DisplayName("Authorized Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> AuthorizedDate { get; set; }
        public String AuthorizedDateFormatted { get { return String.Format("{0:d}", AuthorizedDate); } }
        public String AuthorizedDateLongFormatted { get { return String.Format("{0:d-MMMM-yyyy}", AuthorizedDate); } }
        public String AuthorizedDateLongArabicFormatted { get { return String.Format(new System.Globalization.CultureInfo("ar-KW"), "{0:yyyy, dd MMMM}", AuthorizedDate); } }
        [Required(ErrorMessage = "If the patient is not Beneficiary, select No")]
        [DisplayName("Is Beneficiary?")]
        public bool IsBeneficiary { get; set; } = true;

        [DisplayName("Is Active?")]
        [Required(ErrorMessage = "The patient is either active or inactive, select one")]
        public bool IsActive { get; set; } = true;

        [DisplayName("Is Blocked?")]
        public Nullable<bool> IsBlocked { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public String CreatedDateFormatted { get { return String.Format("{0:d}", CreatedDate); } }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Modified Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public String ModifiedDateFormatted { get { return String.Format("{0:d}", ModifiedDate); } }
        //========================
        public List<AgencyModel> Agencies { get; set; }
        public List<BankModel> Banks { get; set; }
        public List<DoctorModel> Doctors { get; set; }
        public List<HospitalModel> Hospitals { get; set; }
        public List<SpecialtyModel> Sepcialities { get; set; }
        public List<PaymentModel> Payments { get; set; }
        public List<CompanionModel> Companions { get; set; }

        public CompanionModel PrimaryCompanion { get { return Companions!=null? Companions.Where(c=>c.CompanionType=="Primary" && c.JustBeneficiary!=true)?.SingleOrDefault():null; } }
        public BeneficiaryModel Beneficiary { get; set; }

        public  List<CompanionHistoryModel> CompanionHistories { get; set; }

        public List<PatientHistoryModel> PatientHistories { get; set; }
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

        public List<BookTypeModel> BookTypes { get; set; }
        [DisplayName("Select Book Type")]
        public BookTypeModel BookType { get; set; }

        [DisplayName("Treatment Period")]
        public int? TreatmentPeriod { get { return (DateTime.Now.Date - (FirstApptDAte != null ? FirstApptDAte.Value.Date : DateTime.Now.Date)).Days ; } }
    }
}
