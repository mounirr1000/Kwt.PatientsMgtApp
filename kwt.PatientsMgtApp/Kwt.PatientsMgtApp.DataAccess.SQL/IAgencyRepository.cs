using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IAgencyRepository
    {

        AgencyModel GetAgency(int agencyid);

        List<AgencyModel> GetAgencies();

        void AddAgency(AgencyModel agency);

       void UpdateAgency(AgencyModel agency);

        bool DeleteAgency(AgencyModel agency);
    }
}
