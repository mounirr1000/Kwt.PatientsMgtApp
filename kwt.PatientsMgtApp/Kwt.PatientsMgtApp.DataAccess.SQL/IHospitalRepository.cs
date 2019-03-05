using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IHospitalRepository
    {
        HospitalModel GetHospital(int hospitalid);
        List<HospitalModel> GetHospitals();
        void AddHospital(HospitalModel hospital);
        void UpdateHospital(HospitalModel hospital);
        bool DeleteHospital(int hospitalid);
    }
}
