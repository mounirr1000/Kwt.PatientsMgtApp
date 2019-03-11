using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IPaymentDeductionRepository
    {

        //List<PaymentDeductionModel> GetPaymentsWithPhone(DateTime? date);
        //

        List<PaymentDeductionModel> GetPaymentDeductions();

        PaymentDeductionModel GetPaymentDeductionById(int paymentDeductionId);

        PaymentDeductionModel GetPaymentDeductionByPaymentId(int paymentId);

        //List<PaymentDeductionModel> GetPaymentDeductionsByPatientCid(string pacientcid);

        //List<PaymentDeductionModel> GetPaymentDeductionsByCompanionCid(string companioncid);

        void AddPaymentDeduction(PaymentDeductionModel paymentDeduction);

        PaymentModel UpdatePaymentDeduction(PaymentDeductionModel paymentDeduction);

        int DeletePaymentDeduction(PaymentDeductionModel paymentDeduction);

        PaymentDeductionModel GetPaymentDeductionObject(string patientCid);

      //  List<PaymentDeductionModel> GetPaymentDeductionsReport(string paymentId, string patientCid = null, DateTime? startDate = null, DateTime? endDate = null);
    }
}
