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
        List<PaymentModel> GetPayments();

        PaymentModel GetPaymentById(int paymentid);

        List<PaymentModel> GetPaymentsByPatientCid(string pacientcid);
        List<PaymentModel> GetPaymentsByCompanionCid(string companioncid);
        void AddPayment(PaymentModel payment);

        PaymentModel UpdatePayment(PaymentModel payment);

        int DeletePayment(PaymentModel payment);
    }
}
