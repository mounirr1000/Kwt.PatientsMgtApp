﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PaymentModel : BaseEntity
    {

        public int PaymentID { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Payment Date")]
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public String PaymentDateFormatted { get { return String.Format("{0:d}", PaymentDate); } }

        public bool IsActive { get; set; }

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

        [DisplayName("Companion Name")]
        public string BeneficiaryFullName
        {
            get { return this.BeneficiaryFName + " " + this.BeneficiaryMName + " " + this.BeneficiaryLName; }
        }
        [DisplayName("Beneficiary Bank Name")]
        public string BeneficiaryBank { get; set; }

        [DisplayName("Beneficiary Bank Account")]
        public string BeneficiaryIBan { get; set; }


        public string Hospital { get; set; }


        public string Agency { get; set; }

        [DisplayName("Comapnion Pay Rate")]
        public decimal? CompanionPayRate { get; set; }

        [DisplayName("Patient Pay Rate")]
        public decimal? PatientPayRate { get; set; } = 75;

        [DisplayName("Payment Start Date")]
        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> PaymentStartDate { get; set; }
        public String PaymentStartDateFormatted { get { return String.Format("{0:d}", PaymentStartDate); } }

        [DisplayName("Payment End Date")]
        [DataType(DataType.Date)]
        [Required]
        public Nullable<System.DateTime> PaymentEndDate { get; set; }
        public String PaymentEndDateFormatted { get { return String.Format("{0:d}", PaymentEndDate); } }

        [DisplayName("Payment Period")]
        public Nullable<int> PaymentLengthPeriod { get; set; }

        [DisplayName("Patient Amount")]
        public Nullable<decimal> PatientAmount { get; set; }

        [DisplayName("Companion Amount")]
        public Nullable<decimal> CompanionAmount { get; set; }

        [DisplayName("Total")]
        public Nullable<decimal> TotalDue { get; set; }

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
        public List<PayRateModel> PayRates { get; set; }

        // new February 28,2019
        public string PatientPhone { get; set; }
        //
        public List<PaymentModel> Payments { get; set; }
    }
}
