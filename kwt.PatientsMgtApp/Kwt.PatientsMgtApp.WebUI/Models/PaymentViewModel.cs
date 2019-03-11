using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class PaymentViewModel
    {
        public PaymentModel Payment { get; set; }

        public PaymentDeductionModel PaymentDeduction
        {
            get { return Payment.PaymentDeductionObject; }
            set { Payment.PaymentDeductionObject = value; }
        }
    }
}