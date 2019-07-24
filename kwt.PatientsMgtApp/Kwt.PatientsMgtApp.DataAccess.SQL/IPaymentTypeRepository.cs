using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IPaymentTypeRepository
    {
        List<PaymentTypeModel> GetPaymentTypeList();

        PaymentTypeModel GetPaymentType(int? paymentTypeId);
        void AddPaymentType(PaymentTypeModel paymentType);
    }
}
