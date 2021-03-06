﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class PatientReportModel
    {
        public string PatientCID { get; set; }
        public string Name { get; set; }
        public string KWTphone { get; set; }
        public string USphone { get; set; }
        public Nullable<System.DateTime> FirstApptDate { get; set; }
        public Nullable<System.DateTime> EndTreatDate { get; set; }
        public string IsActive { get; set; }
        public string IsBeneficiary { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string Iban { get; set; }
        public string Agency { get; set; }
        public string Doctor { get; set; }
        public string Specialty { get; set; }
        public string Diagnosis { get; set; }
        public string Hospital { get; set; }
        public string Notes { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
