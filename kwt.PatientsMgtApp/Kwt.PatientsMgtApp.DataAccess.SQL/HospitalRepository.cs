using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class HospitalRepository : IHospitalRepository
    {
        readonly private IDomainObjectRepository _domainObjectRepository;

        public HospitalRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public HospitalModel GetHospital(int hospitalid)
        {
            var hospital = _domainObjectRepository.Get<Hospital>(h => h.HospitalID == hospitalid);
            if (hospital != null)
                return new HospitalModel()
                {
                    HospitalName = hospital.HospitalName,
                    HospitalID = hospital.HospitalID,
                };
            return null;
        }

        public List<HospitalModel> GetHospitals()
        {
            var hospital = _domainObjectRepository.All<Hospital>();
            return hospital?.Select(h => new HospitalModel()
            {
                HospitalName = h.HospitalName,
                HospitalID = h.HospitalID,
            }).ToList();
        }

        public void AddHospital(HospitalModel hospital)
        {
            var hospitalToAdd = new Hospital()
            {
                HospitalName = hospital.HospitalName,
            };
            if (!String.IsNullOrEmpty(hospitalToAdd.HospitalName))
            {
                _domainObjectRepository.Create<Hospital>(hospitalToAdd);
            }
        }

        public void UpdateHospital(HospitalModel hospital)
        {
            var hospitalToUpdate = _domainObjectRepository.Get<Hospital>(d => d.HospitalID == hospital.HospitalID);
            if (hospitalToUpdate != null)
            {
                hospitalToUpdate.HospitalName = hospital.HospitalName;
                _domainObjectRepository.Update<Hospital>(hospitalToUpdate);
            }
        }

        public bool DeleteHospital(int hospitalid)
        {
            var hospitalToDelete = _domainObjectRepository.Get<Hospital>(h =>h.HospitalID  == hospitalid);
            if (hospitalToDelete != null)
            {
                return _domainObjectRepository.Delete<Hospital>(hospitalToDelete) > 0;
            }
            return false;
        }
    }
}
