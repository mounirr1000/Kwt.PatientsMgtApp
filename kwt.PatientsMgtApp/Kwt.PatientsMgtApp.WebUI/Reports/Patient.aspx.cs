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

                //new isdead
                IDictionary<string, bool?> isDead = new Dictionary<string, bool?>();
                isDead.Add("----Select----", null);
                isDead.Add("Dead", true);
                isDead.Add("Not Dead", false);

                IsDeadList.DataSource = isDead.ToList();
                IsDeadList.DataBind();
                IsDeadList.DataTextField = "key";
                IsDeadList.DataValueField = "value";
                IsDeadList.DataBind();

                GenerateReport();
                
            }
        }
        
        protected void Search_Click(object sender, EventArgs e)
        {
            bool? selectedActive;
            bool? selectedDeatStatus;
            if (string.IsNullOrEmpty(IsActiveList.SelectedValue))
            {
                selectedActive = null;
            }
            else
            {
                selectedActive = Convert.ToBoolean(IsActiveList.SelectedValue);
            }
            if (string.IsNullOrEmpty(IsDeadList.SelectedValue))
            {
                selectedDeatStatus = null;
            }
            else
            {
                selectedDeatStatus = Convert.ToBoolean(IsDeadList.SelectedValue);
            }

            //GenerateReport(PatientCid.Text,Doctor.Text,Speciality.Text,Hospital.Text,Status.Checked);
            GenerateReport(PatientCid.Text, DoctorList.SelectedValue, SpecialityList.SelectedValue, HospitalList.SelectedValue, selectedActive,null,null, selectedDeatStatus);
        }
        protected void Clear_Click(object sender, EventArgs e)
        {
            SpecialityList.Text = null;
            DoctorList.Text = null;

            IsActiveList.SelectedValue = null;
            IsDeadList.SelectedValue = null;
            HospitalList.Text = null;
            PatientCid.Text = null;
            IsActiveList.Text = null;
            IsDeadList.Text = null;
            GenerateReport();
        }
        private void GenerateReport(string patientCid=null,string doctorTxt=null,
                                    string specialityTxt = null,string hospitalTxt = null,
                                    bool? statusCheck =null, DateTime? startDateTxt = null, DateTime? endDateTxt = null, bool? isDead = null)
        {
            var patientId = string.IsNullOrEmpty(patientCid) ? null : patientCid;
            var doctor = string.IsNullOrEmpty(doctorTxt) ? null : doctorTxt;
            var speciality = string.IsNullOrEmpty(specialityTxt) ? null : specialityTxt;
            var hospital = string.IsNullOrEmpty(hospitalTxt) ? null : hospitalTxt;
            var status = statusCheck;//Convert.ToBoolean(statusCheck);//!statusCheck.HasValue || statusCheck.Value==false ? null : statusCheck;
            var isDeadValue = isDead;//(isDead==false ||isDead==null)? false:true;

            var startDate = new DateTime();
            var endDate = new DateTime();
            List<PatientReportModel> patients = new List<PatientReportModel>();
            if (DateTime.TryParse(StartDate.Text, out startDate) && DateTime.TryParse(EndDate.Text, out endDate))
            {
                patients = _patientRepository.GetPatientsReport(patientId, hospital, doctor, status, speciality,
                    startDate, endDate, isDeadValue);
            }
            // esle get last 30 days entered patients
            else
            {
                startDate = DateTime.Now.AddDays(-30).Date;
                endDate = DateTime.Now.Date;
                if (patientId!=null || hospital!=null || doctor!=null || status!=null || speciality!=null|| isDeadValue!=null)
                {
                    patients = _patientRepository.GetPatientsReport(patientId, hospital, doctor, status, speciality, null, null, isDeadValue);
                }
                else patients = _patientRepository.GetPatientsReport(patientId, hospital, doctor, status, speciality, startDate, endDate, isDeadValue);
            }

            Message.Text = null;
            //ErrorMessage.Visible = true;
            //Message.Enabled = true;
            if (patients==null||patients.Count == 0)
            {
                ErrorMessage.Visible = true;
                Message.Enabled = true;
                if (startDate != null && endDate != null)
                    Message.Text = "There is no patient to display in the selected search!";
                if (startDate == null || endDate == null)
                    Message.Text = "There is no patient in the last 30 days to display in the report!";
                ReportViewer1.Visible = false;
            }
            else
            {
                ReportViewer1.Visible = true;
                ErrorMessage.Visible = false;
                Message.Text = "Total Number of patients: "+ patients.Count;
                Message.Enabled = false;
            }
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/PatientReport.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rdc = new ReportDataSource("PatientList", patients);

            //
            //ReportParameter isActive = new ReportParameter("IsActive", status.ToString());
            ReportParameter selectedPatientCid = new ReportParameter("PatientCid", patientId);
            ReportParameter selectedDoctor = new ReportParameter("Doctor", doctor);
            ReportParameter selectedHospital = new ReportParameter("Hospital", hospital);
            ReportParameter selectedSpecialty = new ReportParameter("Speciality", speciality);

            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { selectedPatientCid, selectedDoctor, selectedHospital, selectedSpecialty });
            //
            ReportViewer1.LocalReport.DataSources.Add(rdc);
            ReportViewer1.ShowPrintButton = true;
            ReportViewer1.PageCountMode = PageCountMode.Actual;
            ReportViewer1.Width = Unit.Percentage(100);
            ReportViewer1.Font.Bold=true;
            ReportViewer1.LocalReport.Refresh();
        }


        // export report to specified format
        protected void Export(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string contentType;
            string encoding;
            string extension;

            //Export the RDLC Report to Byte Array.
            byte[] bytes = ReportViewer1.LocalReport.Render("PDF"//rbFormat.SelectedItem.Value
                                                        , null, out contentType, out encoding, out extension, out streamIds, out warnings);

            //Download the RDLC Report in Word, Excel, PDF and Image formats.
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=RDLC." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }


    }
}