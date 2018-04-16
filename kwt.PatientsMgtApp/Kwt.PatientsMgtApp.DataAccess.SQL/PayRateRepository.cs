using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PayRateRepository : IPayRateRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository ;
        public PayRateRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<PayRateModel> GetPayRatesList()
        {
            var payRates = _domainObjectRepository.All<PayRate>();

            return payRates.Select(p => new PayRateModel()
            {
                CompanionRate = p.CompanionRate,
                PatientRate = p.PatientRate,
                PayRateID = p.PayRateID
            }).ToList();
            
        }
    }
}
