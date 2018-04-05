using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PatientManagmentRepository: IPatientManagmentRepository
    {
        private readonly IAgencyRepository _agencyRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IBankRepository _bankRepository;


        public PatientManagmentRepository()
        {
            _agencyRepository = new AgencyRepository();
            _doctorRepository = new DoctorRepository();
            _hospitalRepository= new HospitalRepository();
            _bankRepository = new BankRepository();

        }
        public List<AgencyModel> GetAgencies()
        {
            return _agencyRepository.GetAgencies();
        }

        public List<BankModel> GetBanks()
        {
            return _bankRepository.GetBanks();
        }

        public List<HospitalModel> GetHospitals()
        {
            return _hospitalRepository.GetHospitals();
        }

        public List<DoctorModel> GetDoctors()
        {
            return _doctorRepository.GetDoctors();
        }
    }
}
