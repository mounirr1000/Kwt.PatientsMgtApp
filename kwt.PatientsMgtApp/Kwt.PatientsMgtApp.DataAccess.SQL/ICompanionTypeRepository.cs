using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface ICompanionTypeRepository
    {

        List<CompanionTypeModel> GetCompanionTypes();

        CompanionTypeModel GetCompanionType(int companionTypeId);

        int DeleteCompanionType(int companionTypeId);

        void UpdateCompanionType(CompanionTypeModel companionType);

        CompanionTypeModel AddCompanionType(CompanionTypeModel companionType);

    }
}
