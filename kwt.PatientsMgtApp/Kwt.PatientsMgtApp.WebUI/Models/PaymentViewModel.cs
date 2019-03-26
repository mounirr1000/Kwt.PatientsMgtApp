using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.WebUI.CustomFilter;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class PaymentViewModel
    {
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [GreaterDate(EarlierDateField = "StartDate", ErrorMessage = "End Date should be after Start Date")]
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime? PatientStartDate { get; set; }

        [GreaterDate(EarlierDateField = "PatientStartDate", ErrorMessage = "End Date should be after Start Date")]
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        public DateTime? PatientEndDate { get; set; }
        public PaymentModel Payment { get; set; }

        public PaymentDeductionModel PaymentDeduction
        {
            get { return Payment.PaymentDeductionObject; }
            set { Payment.PaymentDeductionObject = value; }
        }
    }
}