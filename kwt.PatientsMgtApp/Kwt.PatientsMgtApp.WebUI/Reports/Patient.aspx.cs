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
        readonly IDoctorRepository _doctorRepository = new DoctorRepository();
        readonly IHospitalRepository _hospitalRepository = new HospitalRepository();
        readonly ISpecialityRepository _specialityRepository = new SpecialityRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            
            

            //
            
            //
            
            if (!IsPostBack)
            {
                ListItem emptyItem = new ListItem("----Select----", "");
                DoctorList.DataSource = _doctorRepository.GetDoctors();
                DoctorList.DataBind();
                DoctorList.DataTextField = "DoctorName";
                DoctorList.DataValueField = "DoctorName";
                DoctorList.DataBind();
                DoctorList.Items.Insert(0, emptyItem);

                HospitalList.DataSource = _hospitalRepository.GetHospitals();
                HospitalList.DataBind();
                HospitalList.DataTextField = "HospitalName";
                HospitalList.DataValueField = "HospitalName";
                HospitalList.DataBind();
                HospitalList.Items.Insert(0, emptyItem);

                SpecialityList.DataSource = _specialityRepository.GetSpecialities();
                SpecialityList.DataBind();
                SpecialityList.DataTextField = "Speciality";
                SpecialityList.DataValueField = "Speciality";
                SpecialityList.DataBind();
                SpecialityList.Items.Insert(0, emptyItem);

                IDictionary<string,bool?> isActive = new Dictionary<string, bool?>();
                isActive.Add("----Select----", null);
                isActive.Add("Active",true);
                isActive.Add("Not Active", false);
                
                IsActiveList.DataSource = isActive.ToList();
                IsActiveList.DataBind();
                IsActiveList.DataTextField = "key";
                IsActiveList.DataValueField = "value";
                IsActiveList.DataBind();
                
                GenerateReport();
                
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            bool? selectedActive;
            if (string.IsNullOrEmpty(IsActiveList.SelectedValue))
            {
                selectedActive = null;
            }
            else
            {
                selectedActive = Convert.ToBoolean(IsActiveList.SelectedValue);
            }
            
            //GenerateReport(PatientCid.Text,Doctor.Text,Speciality.Text,Hospital.Text,Status.Checked);
            GenerateReport(PatientCid.Text, DoctorList.SelectedValue, SpecialityList.SelectedValue, HospitalList.SelectedValue, selectedActive);
        }
        protected void Clear_Click(object sender, EventArgs e)
        {
            SpecialityList.Text = null;
            DoctorList.Text = null;

            IsActiveList.SelectedValue = null;

            HospitalList.Text = null;
            PatientCid.Text = null;
            IsActiveList.Text = null;
            GenerateReport();
        }
        private void GenerateReport(string patientCid=null,string doctorTxt=null,
                                    string specialityTxt = null,string hospitalTxt = null,
                                    bool? statusCheck =null)
        {
            var patientId = string.IsNullOrEmpty(patientCid) ? null : patientCid;
            var doctor = string.IsNullOrEmpty(doctorTxt) ? null : doctorTxt;
            var speciality = string.IsNullOrEmpty(specialityTxt) ? null : specialityTxt;
            var hospital = string.IsNullOrEmpty(hospitalTxt) ? null : hospitalTxt;
            var status = statusCheck;//Convert.ToBoolean(statusCheck);//!statusCheck.HasValue || statusCheck.Value==false ? null : statusCheck;

            List<PatientReportModel> patients = _patientRepository.GetPatientsReport(patientId, hospital, doctor, status, speciality);

            
            if (patients==null||patients.Count == 0)
            {
                ErrorMessage.Visible = true;
                Message.Enabled = true;
                Message.Text = "There is no patient to display in the report!";
                ReportViewer1.Visible = false;
            }
            else
            {
                ReportViewer1.Visible = true;
                ErrorMessage.Visible = false;
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