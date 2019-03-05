using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class AgencyRepository : IAgencyRepository
    {
        readonly private IDomainObjectRepository _domainObjectRepository;
        public AgencyRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public AgencyModel GetAgency(int agencyid)
        {
            var agency = _domainObjectRepository.Get<Agency>(a => a.AgencyID == agencyid);
            if (agency != null)
            {
                return new AgencyModel()
                {
                    AgencyID = agency.AgencyID,
                    AgencyName = agency.AgencyName
                };
            }
            return null;
        }

        public List<AgencyModel> GetAgencies()
        {
            var agency = _domainObjectRepository.All<Agency>();
            return agency?.Select(a => new AgencyModel()
            {
                AgencyID = a.AgencyID,
                AgencyName = a.AgencyName
            }).ToList();
        }

        public void AddAgency(AgencyModel agency)
        {
            if (agency != null)
            {
                Agency newAgency = new Agency()
                {
                    AgencyName = agency.AgencyName
                };
                _domainObjectRepository.Create<Agency>(newAgency);
            }
        }

        public void UpdateAgency(AgencyModel agency)
        {
            var agencyToUpdate = _domainObjectRepository.Get<Agency>(a => a.AgencyID == agency.AgencyID);
            if (agencyToUpdate != null)
            {
                agencyToUpdate.AgencyID = agency.AgencyID;
                agencyToUpdate.AgencyName = agency.AgencyName;
                _domainObjectRepository.Update<Agency>(agencyToUpdate);
            }
        }

        public bool DeleteAgency(AgencyModel agency)
        {
            var agencyToDelete = _domainObjectRepository.Get<Agency>(a => a.AgencyID == agency.AgencyID);
            if (agencyToDelete != null)
            {
                return _domainObjectRepository.Delete<Agency>(agencyToDelete)>0;
            }
            return false;
        }
    }
}
