using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using Microsoft.Reporting.WebForms;

namespace Kwt.PatientsMgtApp.WebUI.Reports
{
    public partial class Patient : System.Web.UI.Page
    {
        readonly IPatientRepository _patientRepository = new PatientRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {

                GenerateReport();

            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {

            GenerateReport(PatientCid.Text,Doctor.Text,Speciality.Text,Hospital.Text,Status.Checked);
        }
        protected void Clear_Click(object sender, EventArgs e)
        {
            Speciality.Text = null;
            Doctor.Text = null;

            Status.Checked = false;

            Hospital.Text = null;
            PatientCid.Text = null;
            GenerateReport();
        }
        private void GenerateReport(string patientCid=null,string doctorTxt=null,string specialityTxt = null,string hospitalTxt = null, bool? statusCheck=null)
        {
            var patientId = string.IsNullOrEmpty(patientCid) ? null : patientCid;
            var doctor = string.IsNullOrEmpty(doctorTxt) ? null : doctorTxt;
            var speciality = string.IsNullOrEmpty(specialityTxt) ? null : specialityTxt;
            var hospital = string.IsNullOrEmpty(hospitalTxt) ? null : hospitalTxt;
            var status = statusCheck;//!statusCheck.HasValue || statusCheck.Value==false ? null : statusCheck;

            List<PatientReportModel> patients = _patientRepository.GetPatientsReport(patientId, hospital, doctor, status, speciality);

            
            if (patients==null||patients.Count == 0)
            {
                Message.Enabled = true;
                Message.Text = "There is no patient in our records to display in the report";
            }
            else
            {
                Message.Text = "";
                Message.Enabled = false;
            }
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/PatientReport.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rdc = new ReportDataSource("PatientList", patients);
            ReportViewer1.LocalReport.DataSources.Add(rdc);
            ReportViewer1.ShowPrintButton = true;
            ReportViewer1.PageCountMode = PageCountMode.Actual;
            ReportViewer1.Width = Unit.Percentage(100);
            ReportViewer1.Font.Bold=true;
            ReportViewer1.LocalReport.Refresh();
        }
    }
}