using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IDoctorRepository
    {
        DoctorModel GetDoctor(int doctorid);
        List<DoctorModel> GetDoctors();

        void AddDorctor(DoctorModel doctor);

        void UpdateDoctor(DoctorModel doctor);

        bool DeleteDoctor(int doctorid);

    }
}
