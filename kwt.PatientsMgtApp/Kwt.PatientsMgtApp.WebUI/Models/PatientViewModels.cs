using Kwt.PatientsMgtApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class PatientViewModels
    {

        public PatientModel Patient { get; set; }
        public List<AgencyModel> Agencies { get; set; }
        public List<BankModel> Banks { get; set; }
        public List<DoctorModel> Doctors { get; set; }
        public List<HospitalModel> Hospitals { get; set; }

    }
}