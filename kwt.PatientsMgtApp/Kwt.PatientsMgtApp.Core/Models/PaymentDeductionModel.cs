using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PaymentDeductionModel
    {
        public int DeductionID { get; set; }

        public int PaymentID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Deduction Date")]
        public Nullable<System.DateTime> DeductionDate { get; set; }
        public String DeductionDateFormatted { get { return String.Format("{0:d}", DeductionDate); } }

        //public bool IsActive { get; set; }

        
        [DisplayName("Patient Id")]
        public string PatientCID { get; set; }

        
        [DisplayName("Companion Id")]
        public string CompanionCID { get; set; }

        
        public int? BeneficiaryID { get; set; }


        [DisplayName("Deduction Start Date")]
        
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DeductionStartDate { get; set; }
        public String PaymentStartDateFormatted { get { return String.Format("{0:d}", DeductionStartDate); } }

        [DisplayName("Deduction End Date")]
        [DataType(DataType.Date)]
        
        public Nullable<System.DateTime> DeductionEndDate { get; set; }
        public String PaymentEndDateFormatted { get { return String.Format("{0:d}", DeductionEndDate); } }

        //[DisplayName("Payment Period")]
        //public Nullable<int> PaymentLengthPeriod { get; set; }

        [DisplayName("Patient Amount")]
        public Nullable<decimal> PatientAmount { get; set; }

        [DisplayName("Deduction Period")]
        public Nullable<int> DeductionPeriod { get; set; }


        [DisplayName("Companion Amount")]
        public Nullable<decimal> CompanionAmount { get; set; }

        [DisplayName("Total Deduction")]
        public Nullable<decimal> TotalDeduction { get; set; }

        [DisplayName("Amount Paid")]
        public Nullable<decimal> AmountPaid { get; set; }

        [DisplayName("Final Amount")]
        public Nullable<decimal> FinalAmount { get; set; }

        [DisplayName("Companion Deduction")]
        public Nullable<decimal> CompanionDeduction { get; set; }

        [DisplayName("Patient Deduction")]
        public Nullable<decimal> PatientDeduction { get; set; }

        [MaxLength(250, ErrorMessage = "Maximum characters allowed is 250 ")]
        public string Notes { get; set; }

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
        //================================
        public List<BeneficiaryModel> Beneficiaries { get; set; }
        public BeneficiaryModel Beneficiary { get; set; }

        public PaymentModel Payment { get; set; }
        public List<PayRateModel> PayRates { get; set; }

        // new February 28,2019
        public string PatientPhone { get; set; }
        //
        public List<PaymentModel> Payments { get; set; }
    }
}
