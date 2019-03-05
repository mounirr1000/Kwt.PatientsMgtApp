using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IPatientManagmentRepository
    {
        List<AgencyModel> GetAgencies();
        List<BankModel> GetBanks();
        List<HospitalModel> GetHospitals();
        List<DoctorModel> GetDoctors();
        List<SpecialtyModel> GetSpecialities();

    }
}
