using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class DeductionReasonRepository: IDeductionReasonRepository
    {
        readonly private IDomainObjectRepository _domainObjectRepository;

        public DeductionReasonRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<DeductionReasonModel> GetDeductionReasons()
        {
            var specialities = _domainObjectRepository.All<DeductionReason>();
            return specialities?.Select(s => new DeductionReasonModel()
            {
                ReasonId = s.ReasonId,
                Reason = s.Reason,
            }).ToList();
        }

        public DeductionReasonModel GetDeductionReason(int reasonId)
        {
            return null;
        }

        public void AddDeductionReason(DeductionReasonModel reason)
        {
            if (reason != null)
            {
                DeductionReason newReason = new DeductionReason()
                {
                    Reason = reason.Reason
                };
                _domainObjectRepository.Create<DeductionReason>(newReason);
            }
        }
    }
}
