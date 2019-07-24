using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PaymentTypeRepository : IPaymentTypeRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;
        public PaymentTypeRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }


        public List<PaymentTypeModel> GetPaymentTypeList()
        {
            var newpaymentTypes = _domainObjectRepository.All<PaymentType>();

            return newpaymentTypes.Select(p => new PaymentTypeModel()
            {
                PaymentType1 = p.PaymentType1,
                PaymentTypeId = p.PaymentTypeId
            }).ToList();
        }

        public PaymentTypeModel GetPaymentType(int? paymentTypeId)
        {
            var newpaymentType = _domainObjectRepository.Get<PaymentType>(p => p.PaymentTypeId == paymentTypeId);

            if (newpaymentType != null)
                return new PaymentTypeModel()
                {
                    PaymentType1 = newpaymentType.PaymentType1,
                    PaymentTypeId = newpaymentType.PaymentTypeId
                };
            return null;
        }
        public void AddPaymentType(PaymentTypeModel paymentType)
        {
            if (paymentType != null)
            {
                PaymentType newpaymentType = new PaymentType()
                {
                    PaymentType1 = paymentType.PaymentType1,
                    PaymentTypeId = paymentType.PaymentTypeId
                };
                _domainObjectRepository.Create<PaymentType>(newpaymentType);
            }
        }
    }
}
