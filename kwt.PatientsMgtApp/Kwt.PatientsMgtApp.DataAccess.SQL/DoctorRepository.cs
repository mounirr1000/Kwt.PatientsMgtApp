using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class DoctorRepository: IDoctorRepository
    {

        readonly private IDomainObjectRepository _domainObjectRepository;

        public DoctorRepository()
        {
            _domainObjectRepository=new DomainObjectRepository();
        }
       public DoctorModel GetDoctor(int doctorid)
       {
           var doctor = _domainObjectRepository.Get<Doctor>(d => d.DoctorID == doctorid);
            if(doctor!=null)
                return new DoctorModel()
            {
                DoctorID = doctor.DoctorID,
                DoctorName = doctor.DoctorName,             
            };
            return null;
       }

        public List<DoctorModel> GetDoctors()
        {
            var doctors = _domainObjectRepository.All<Doctor>();
            return doctors?.Select(d => new DoctorModel()
            {
                DoctorID = d.DoctorID,
                DoctorName = d.DoctorName,
            }).ToList();
        }

        public void AddDorctor(DoctorModel doctor)
        {
            var doctorToAdd = new Doctor()
            {
                DoctorName = doctor.DoctorName,
            };
            if (!String.IsNullOrEmpty(doctorToAdd.DoctorName))
            {
                _domainObjectRepository.Create<Doctor>(doctorToAdd);
            }
        }

        public void UpdateDoctor(DoctorModel doctor)
        {
            var doctorToUpdate = _domainObjectRepository.Get<Doctor>(d => d.DoctorID == doctor.DoctorID);
            if (doctorToUpdate != null)
            {
                doctorToUpdate.DoctorName = doctor.DoctorName;
                _domainObjectRepository.Update<Doctor>(doctorToUpdate);
            }
        }

        public bool DeleteDoctor(int doctorid)
        {
            var doctorToDelete = _domainObjectRepository.Get<Doctor>(d => d.DoctorID == doctorid);
            if (doctorToDelete != null)
            {
              return  _domainObjectRepository.Delete<Doctor>(doctorToDelete)>0;
            }
            return false;
        }
    }
}
