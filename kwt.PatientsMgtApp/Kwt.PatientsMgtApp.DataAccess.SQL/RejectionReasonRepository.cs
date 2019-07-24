using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class RejectionReasonRepository: IRejectionReasonRepository
    {

        private readonly IDomainObjectRepository _domainObjectRepository;
        public RejectionReasonRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        
        public  List<RejectionReasonModel> GetRejectionReasonList()
        {

            var rejectionReasons = _domainObjectRepository.All<RejectionReason>();

            return rejectionReasons.Select(p => new RejectionReasonModel()
            {
                RejectionReason1 = p.RejectionReason1,
                RejectionReasonID= p.RejectionReasonID
            }).ToList();
        }
       public void AddRejectionReason(RejectionReasonModel rejectionReason)
        {
            if (rejectionReason != null)
            {
                RejectionReason newRejectionReason = new RejectionReason()
                {
                    RejectionReason1 = rejectionReason.RejectionReason1,
                    RejectionReasonID = rejectionReason.RejectionReasonID
                };
                _domainObjectRepository.Create<RejectionReason>(newRejectionReason);
            }
        }
    }
}
