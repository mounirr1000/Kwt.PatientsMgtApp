using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class HomeViewModels
    {
        public int? TotalActivePatients { get; set; }

        public int? TotalPayments { get; set; }

        public DateTime TodayDateTime { get; set; }

    }
}