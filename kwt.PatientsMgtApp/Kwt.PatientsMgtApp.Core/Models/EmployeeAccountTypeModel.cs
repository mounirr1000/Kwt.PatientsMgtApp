using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class EmployeeAccountTypeModel
    {
        public int Id { get; set; }
        public int AgencyAccountID { get; set; }
        public int SalaryAccountID { get; set; }
        public string SalaryAccountDesc { get; set; }
        public int BonusAccountID { get; set; }
        public string BonusAccountDesc { get; set; }
        public int OvertimeAccountID { get; set; }
        public string OvertimeAccountDesc { get; set; }
        public int InsuranceAccountID { get; set; }
        public string InsuranceAccountDesc { get; set; }
        public int EmployeeID { get; set; }
    }
}
