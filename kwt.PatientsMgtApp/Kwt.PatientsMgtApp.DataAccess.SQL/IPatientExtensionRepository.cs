using Kwt.PatientsMgtApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IPatientExtensionRepository
    {
        PatientExtensionModel GetExtension(int extensionId);
        List<PatientExtensionModel> GetExtensionList();
        void AddExtension(PatientExtensionModel extensionModel);
        PatientExtensionModel EditExtension(PatientExtensionModel extensionModel);
        bool DeleteExtension(int extensionId);
        List<PatientExtensionModel> GetOpenExtensionList();
        PatientExtensionModel GetPatientExtensionByCID(string patientCid);
        PatientExtensionModel UpdatePatientExtension(PatientExtensionModel patExtension);


    }
}
