using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PaymentModel : BaseEntity
    {
        public int PaymentID { get; set; }

        public string PaymentIDString { get { return PaymentID != null ? PaymentID.ToString() : ""; } }

        [DataType(DataType.Date)]
        [DisplayName("Payment Date")]
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public String PaymentDateFormatted { get { return String.Format("{0:d}", PaymentDate); } }
        public String PaymentDateLongFormatted { get { return String.Format("{0:d-MMMM-yyyy}", PaymentDate); } }

        public bool IsActive { get; set; }

        public bool? IsPatientBlocked { get; set; }

        [Required]
        [DisplayName("Patient Id")]
        public string PatientCID { get; set; }

        [DisplayName("Patient First Name")]
        public string PatientFName { get; set; }

        [DisplayName("Patient Middle Name")]
        public string PatientMName { get; set; }

        [DisplayName("Patient Last Name")]
        public string PatientLName { get; set; }

        [DisplayName("Patient Name")]
        public string PatientFullName
        {
            get { return this.PatientFName + " " + this.PatientMName + " " + this.PatientLName; }
        }

        public bool? HasCompanion { get; set; }

        [DisplayName("Companion Id")]
        public string CompanionCID { get; set; }

        [DisplayName("Companion First Name")]
        public string CompanionFName { get; set; }

        [DisplayName("Companion Middle Name")]
        public string CompanionMName { get; set; }

        [DisplayName("Companion Last Name")]
        public string CompanionLName { get; set; }


        [DisplayName("Companion Name")]
        public string CompanionFullName
        {
            get { return this.CompanionFName + " " + this.CompanionMName + " " + this.CompanionLName; }
        }
        [DisplayName("Beneficiary Id")]
        public string BeneficiaryCID { get; set; }

        [DisplayName("Beneficiary First Name")]
        public string BeneficiaryFName { get; set; }

        [DisplayName("Beneficiary Middle Name")]
        public string BeneficiaryMName { get; set; }

        [DisplayName("Beneficiary Last Name")]
        public string BeneficiaryLName { get; set; }

        [DisplayName("Beneficiary Name")]
        public string BeneficiaryFullName
        {
            get { return this.BeneficiaryFName + " " + this.BeneficiaryMName + " " + this.BeneficiaryLName; }
        }
        [DisplayName("Beneficiary Bank Name")]
        public string BeneficiaryBank { get; set; }

        [DisplayName("IBan")]
        public string BeneficiaryIBan { get; set; }


        public string Hospital { get; set; }


        public string Agency { get; set; }

        [DisplayName("Comapnion Pay Rate")]
        public decimal? CompanionPayRate { get; set; }

        [DisplayName("Patient Pay Rate")]
        public decimal? PatientPayRate { get; set; } = 75;

        [DisplayName("Payment Start Date")]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PaymentStartDate { get; set; }
        public String PaymentStartDateFormatted { get { return String.Format("{0:d}", PaymentStartDate); } }
        public String PaymentStartDateLongFormatted { get { return String.Format("{0: d-MMMM-yyyy}", PaymentStartDate); } }
        [DisplayName("Payment End Date")]
        [DataType(DataType.Date)]
        [Required]
        public Nullable<System.DateTime> PaymentEndDate { get; set; }
        public String PaymentEndDateFormatted { get { return String.Format("{0:d}", PaymentEndDate); } }
        public String PaymentEndDateLongFormatted { get { return String.Format("{0:d-MMMM-yyyy}", PaymentEndDate); } }

        [DisplayName("Payment Period")]
        public Nullable<int> PaymentLengthPeriod { get; set; }

        [DisplayName("Patient Amount")]
        public Nullable<decimal> PatientAmount { get; set; }

        [DisplayName("Companion Amount")]
        public Nullable<decimal> CompanionAmount { get; set; }

        [DisplayName("Total")]
        public Nullable<decimal> TotalDue { get; set; }

        [DisplayName("Final Amount After Correction")]
        public decimal? FinalAmountAfterCorrection { get; set; }

        [DisplayName("Deducted Amount")]
        public decimal? DeductedAmount { get; set; }

        [MaxLength(250, ErrorMessage = "Maximum characters allowed is 250 ")]
        public string Notes { get; set; }

        public string DeductionNotes { get; set; }
        //new
        [DisplayName("Patient Notes")]
        public string PatientNotes { get; set; }        
        public string PaymentDeductionNotes { get { return PaymentDeductionObject!=null ? PaymentDeductionObject.Notes:""; }  }

        public decimal? TotalDeduction { get { return PaymentDeductionObject != null ? PaymentDeductionObject.TotalDeduction : 0; } }
        public decimal? FinalAmount { get { return PaymentDeductionObject != null ? PaymentDeductionObject.FinalAmount : TotalDue; } }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Created Date")]
        public System.DateTime CreatedDate { get; set; }
        public String CreatedDateFormatted { get { return String.Format("{0:d}", CreatedDate); } }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Modified Date")]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public String ModifiedDateFormatted { get { return String.Format("{0:d}", ModifiedDate); } }

        public int? PayRateID { get; set; }

        [DisplayName("Patient Deduction Rate")]
        public decimal? PatientDeductionRate { get; set; }

        [DisplayName("Comp Deduction Rate")]
        public decimal? CompanionDeductionRate { get; set; }
        //================================
        public List<BeneficiaryModel> Beneficiaries { get; set; }
        public BeneficiaryModel Beneficiary { get; set; }
        public List<PayRateModel> PayRates { get; set; }

        // new February 28,2019
        public string PatientPhone { get; set; }
        //
        public List<PaymentModel> Payments { get; set; }

        public List<PaymentHistoryModel> PaymentHistories { get; set; }

        public PaymentDeductionModel PaymentDeductionModel { get; set; }

        public PaymentDeductionModel PaymentDeductionObject { get; set; }
        public ICollection<PaymentDeductionModel> PaymentDeductions { get; set; }

        public IList<string> ActivePatientCidList { get; set; }

        // new 07/13/2019
        [DisplayName("Is Payment Rejected?")]
        public Nullable<bool> IsRejected { get; set; }

        public string IsRejectedText { get { return IsRejected == true ? "Yes" : "No"; } }
        [DisplayName("Select Payment Type to Make")]
        public Nullable<int> PaymentTypeId { get; set; }
        [DisplayName("Select The Rejected Payment to Correct")]
        public Nullable<int> RejectedPaymentId { get; set; }
        public RejectedPaymentModel RejectedPayment { get; set; }

        public bool? IsThisPaymentCorrected { get; set; }

        public int? CorrectedPaymentId { get; set; }
        public List<RejectionReasonModel> RejectionReasons { get; set; }
        public PaymentTypeModel PaymentType { get; set; }
        public List<RejectedPaymentModel> RejectedPayments { get; set; }

        public List<PaymentTypeModel> PaymentTypes { get; set; }
        public List<PaymentModel> RejectedPaymentList { get; set; }

        [DisplayName("Select Reason For The Adjsutment")]
        public Nullable<int> AdjustmentReasonID { get; set; }
        public AdjustmentReasonModel AdjustmentReason { get; set; }
        public List<AdjustmentReasonModel> AdjustmentReasons { get; set; }

        public Nullable<bool> IsApproved { get; set; }
        public Nullable<bool> IsVoid { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        //public string BeneficiaryFName { get; set; }
        //public string BeneficiaryMName { get; set; }
        //public string BeneficiaryLName { get; set; }
        //public string BeneficiaryCID { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string IBan { get; set; }
        public string PatientFormatedPhone { get { return FormatPhoneNumber(PatientPhone, ""); } }

        //new
        public PaymentDeductionModel PaymentDeductionObjectFromList => GetPaymentDeductionModelFromList(PaymentDeductions);

        public PaymentDeductionModel GetPaymentDeductionModelFromList(ICollection<PaymentDeductionModel> paymentDeductions)
        {
            if (paymentDeductions != null && paymentDeductions.Count > 0)
            {
                return paymentDeductions.FirstOrDefault();
            }
            return null;

        }
        public string FormatPhoneNumber(string phoneNum, string phoneFormat)
        {

            if (phoneFormat == "")
            {
                // If phone format is empty, code will use default format (###) ###-####
                phoneFormat = "##########";
            }
            if (!string.IsNullOrEmpty(phoneNum))
            {
                phoneNum = phoneNum.Replace("+1", "");
                // First, remove everything except of numbers
                Regex regexObj = new Regex(@"[^\d]");
                phoneNum = regexObj.Replace(phoneNum, "");
                // Second, format numbers to phone string 
                if (phoneNum.Length > 0)
                {
                    phoneNum = Convert.ToInt64(phoneNum).ToString(phoneFormat);
                }
            }
                

            

            return phoneNum;
        }
    }
}
