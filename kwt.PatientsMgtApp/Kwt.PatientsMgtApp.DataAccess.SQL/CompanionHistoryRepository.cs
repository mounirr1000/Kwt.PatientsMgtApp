using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class CompanionHistoryRepository: ICompanionHistoryRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

        public CompanionHistoryRepository()
        {
            _domainObjectRepository= new DomainObjectRepository();
        }
        public List<CompanionHistoryModel> GetCompanionsHistory()
        {
            var history = _domainObjectRepository.All<CompanionHistory>();

            return history.Select(h => new CompanionHistoryModel()
            {
                 CompanionCID = h.CompanionCID,
                 CreatedDate = h.CreatedDate,
                 CompanionType = h.CompanionType,
                 CreatedBy = h.CreatedBy,
                 DateIn = h.DateIn,
                 DateOut = h.DateOut,
                 HistoryID = h.HistoryID,
                 IsActive = h.IsActive,
                 IsBeneficiary = h.IsBeneficiary,
                 ModifiedBy = h.ModifiedBy,
                 ModifiedDate = h.ModifiedDate,
                 Name = h.Name,
                 Notes = h.Notes,
                 PatientCID = h.PatientCID,
                
            }).ToList();
            
        }
    }
}
