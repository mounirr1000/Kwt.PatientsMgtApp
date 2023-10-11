using Kwt.PatientsMgtApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kwt.PatientsMgtApp.WebUI.Models
{
    public class EmployeesPaymentViewModel
    {
        public List<EmployeeModel> EmployeeList { get; set; }
        public string EmployeeSelecedIds { get; set; }

    }
}