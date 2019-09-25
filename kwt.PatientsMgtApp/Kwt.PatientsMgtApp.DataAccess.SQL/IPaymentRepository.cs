using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IPaymentRepository
    {

        //new February 28, 2019
        List<PaymentModel> GetPaymentsWithPhone(DateTime? date);
        //
        //new 07/12/2019
        List<PaymentReportModel> GetPaymentsReportWithoutParms();
            //
        List<PaymentModel> GetPayments();

        PaymentModel GetPaymentById(int paymentid);

        List<PaymentModel> GetPaymentsByPatientCid(string pacientcid);
        List<PaymentModel> GetPaymentsByCompanionCid(string companioncid);
        void AddPayment(PaymentModel payment);

        PaymentModel UpdatePayment(PaymentModel payment);

        PaymentModel UpdateApprovedPayment(PaymentModel payment);

        int DeletePayment(PaymentModel payment);

        PaymentModel GetPaymentObject(string patientCid);

        PaymentModel GetPaymentObject();

        List<PaymentReportModel> GetPaymentsReport(string patientCid = null, DateTime? startDate = null, DateTime? endDate = null, int? bankId = null);

        List<PaymentModel> GetNextPatientPayment(int? numberOfDays = null);

        List<PaymentReportModel> GetStatisticalPaymentsReport(DateTime? startDate = null,
            DateTime? endDate = null, int? bankId = null, int? agencyId = null);
    }
}
