using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class SpecialityRepository: ISpecialityRepository
    {
        readonly private IDomainObjectRepository _domainObjectRepository;

        public SpecialityRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public SpecialtyModel GetSpeciality(int specialityid)
        {
            
            return null;
        }

        public List<SpecialtyModel> GetSpecialities()
        {
            var specialities = _domainObjectRepository.All<Specialty>();
            return specialities?.Select(s => new SpecialtyModel()
            {
                SpecialtyId = s.SpecialtyId,
                Speciality = s.Specialty1,
            }).ToList();
        }

        public void AddSpeciality(SpecialtyModel specialty)
        {
            if (specialty != null)
            {
                Specialty newSpecialty = new Specialty()
                {
                    Specialty1 = specialty.Speciality
                };
                _domainObjectRepository.Create<Specialty>(newSpecialty);
            }
        }
    }
}
