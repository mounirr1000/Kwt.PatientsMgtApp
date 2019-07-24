using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class AdjustmentReasonRepository: IAdjustmentReasonRepository
    {

        private readonly IDomainObjectRepository _domainObjectRepository;
        public AdjustmentReasonRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<AdjustmentReasonModel> GetAdjustmentReasonList()
        {
            var newpaymentTypes = _domainObjectRepository.All<AdjustmentReason>();

            return newpaymentTypes.Select(p => new AdjustmentReasonModel()
            {
                AdjustmentReasonID = p.AdjustmentReasonID,
                AdjustmentReason = p.AdjustmentReason1
            }).ToList();
        }

       public void AddAdjustmentReason(AdjustmentReasonModel adjustmentReason)
        {
            if (adjustmentReason != null)
            {
                AdjustmentReason adjustmentReasonType = new AdjustmentReason()
                {
                  //  AdjustmentReasonID = adjustmentReason.AdjustmentReasonID,
                    AdjustmentReason1 = adjustmentReason.AdjustmentReason
                };
                _domainObjectRepository.Create<AdjustmentReason>(adjustmentReasonType);
            }
        }
        
    }
}
