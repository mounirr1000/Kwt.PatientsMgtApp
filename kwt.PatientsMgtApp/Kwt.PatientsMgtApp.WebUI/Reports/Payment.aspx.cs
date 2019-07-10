using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.DataAccess.SQL;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using Microsoft.Reporting.WebForms;
using Kwt.PatientsMgtApp.Core;
namespace Kwt.PatientsMgtApp.WebUI.Reports
{
    public partial class Payment : System.Web.UI.Page
    {
        readonly IPaymentRepository _paymentRepository = new PaymentRepository();
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                GenerateReport();

            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            
            string selectedValue = ReportTypes.SelectedItem.Value;


            int reportType;
            Int32.TryParse(selectedValue, out reportType);
            GenerateReportType(reportType);

        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            StartDate.Text = null;
            EndDate.Text = null;
            PatientCid.Text = null;
            //ResultMessage.Enabled = false;
            //ResultMessage.Text = "";
            GenerateReport();
        }

        private List<PaymentReportModel> GenerateReport(string patientCid = null, DateTime? startDateTxt = null, DateTime? endDateTxt = null, int? reportType = null)
        {
            var patientId = string.IsNullOrEmpty(patientCid) ? null : patientCid;
            var startDate = startDateTxt;
            var endDate = endDateTxt;


            List<PaymentReportModel> payments = _paymentRepository.GetPaymentsReport(patientId, startDate, endDate)?.OrderByDescending(p => p.CreatedDate)?.ToList();


            if (payments == null || payments.Count == 0)
            {
                Message.Enabled = true;
                ErrorMessage.Visible = true;
                Message.Text = "There is no payment to display in the report";
            }
            else
            {
                Message.Text = "";
                ErrorMessage.Visible = false;
                Message.Enabled = false;
               
                payments = PaymentTypeForReportingType(payments, reportType);
            }
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reports/PaymentReport.rdlc");
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rdc = new ReportDataSource("PaymentList", payments);
            //new 
            ReportParameter stDate = new ReportParameter("StartDate", startDate?.ToString());
            ReportParameter enDate = new ReportParameter("EndDate", endDate?.ToString());
            ReportParameter rtType = new ReportParameter("ReportType", reportType?.ToString());

            ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { stDate, enDate, rtType });
            // end new
            ReportViewer1.LocalReport.DataSources.Add(rdc);
            // ReportViewer1.EnableClientPrinting
            ReportViewer1.LocalReport.Refresh();


            return payments;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType,
                           out encoding, out extension, out streamids, out warnings);

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("output.pdf"),
            FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            //Open existing PDF
            Document document = new Document(PageSize.LETTER);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("output.pdf"));
            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath("Print.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            //Add Page to new document
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }

            //Attach javascript to the document
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            frmPrint.Attributes["src"] = "Print.pdf";
        }

        protected void reportTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = ReportTypes.SelectedItem.Value;


            int reportType;
            Int32.TryParse(selectedValue, out reportType);
            GenerateReportType(reportType);
            //switch (reportType)
            //{
            //    case 1:
            //        GenerateReportType(reportType);
            //        break;
            //    case 2:
            //        GenerateReportType(reportType);
            //        break;
            //    case 3:
            //        GenerateReportType(reportType);
            //        break;
            //    default:
            //        GenerateReportType(3);
            //        break;
            //}
        }

        private void GenerateReportType(int? reportType)
        {
            var startDate = new DateTime();
            var endDate = new DateTime();
            var resultCount = 0;

            if (DateTime.TryParse(StartDate.Text, out startDate) && DateTime.TryParse(EndDate.Text, out endDate))
            {
                GenerateReport(PatientCid.Text, startDate, endDate, reportType);
            }
            else
            {

                resultCount = GenerateReport(PatientCid.Text, null, null, reportType).Count;
            }
        }

        private List<PaymentReportModel> PaymentTypeForReportingType(List<PaymentReportModel> payments, int? reportType = null)
        {
            if (reportType == (int)Enums.ReportType.Kuwait)// sent to kuwait
                payments = payments?.Select(p => new PaymentReportModel()
                {
                    AgencyName = p.AgencyName,
                    PaymentID = p.PaymentID,
                    FinalAmount = p.FinalAmount ?? p.Amount,
                    PatientCID = p.PatientCID,
                    CreatedDate = p.CreatedDate,
                    BeneficiaryCID = p.BeneficiaryCID,
                    EndDate = p.EndDate,
                    StartDate = p.StartDate,
                    //Amount = p.Amount,
                    Bank = p.Bank,
                    BeneficiaryName = p.BeneficiaryName,
                    Code = p.Code,
                    CompanionName = p.CompanionName,
                    // DeductedAmount = p.DeductedAmount ?? 0,
                    IBan = p.IBan,
                    PatientName = p.PatientName,
                    //TotalPayments = payments.Count,
                    //TotalPatients = payments.Select(pat => pat.PatientCID).Distinct().Count(),
                    //TotalAmount = payments.Sum(pay => pay.FinalAmount ?? pay.Amount)
                }).ToList();
           else  if (reportType == (int)Enums.ReportType.Archive)// saved in Archive
                 payments = payments?.Select(p => new PaymentReportModel()
                {
                    AgencyName = p.AgencyName,
                    PaymentID = p.PaymentID,
                    FinalAmount = p.FinalAmount ?? p.Amount,
                    PatientCID = p.PatientCID,
                    CreatedDate = p.CreatedDate,
                    BeneficiaryCID = p.BeneficiaryCID,
                    EndDate = p.EndDate,
                    StartDate = p.StartDate,
                  //  Amount = p.Amount,
                    Bank = p.Bank,
                    BeneficiaryName = p.BeneficiaryName,
                    Code = p.Code,
                    CompanionName = p.CompanionName,
                 //   DeductedAmount = p.DeductedAmount ?? 0,
                    IBan = p.IBan,
                    PatientName = p.PatientName,
                     //TotalPayments = payments.Count,
                     //TotalPatients = payments.Select(pat => pat.PatientCID).Distinct().Count(),
                     TotalAmount = payments.Sum(pay => pay.FinalAmount ?? pay.Amount)
                 }).ToList();
            else
                payments = payments?.Select(p => new PaymentReportModel()
                {
                    AgencyName = p.AgencyName,
                    PaymentID = p.PaymentID,
                    FinalAmount = p.FinalAmount ?? p.Amount,
                    PatientCID = p.PatientCID,
                    CreatedDate = p.CreatedDate,
                    BeneficiaryCID = p.BeneficiaryCID,
                    EndDate = p.EndDate,
                    StartDate = p.StartDate,
                    Amount = p.Amount,
                    Bank = p.Bank,
                    BeneficiaryName = p.BeneficiaryName,
                    Code = p.Code,
                    CompanionName = p.CompanionName,
                    DeductedAmount = p.DeductedAmount ?? 0,
                    IBan = p.IBan,
                    PatientName = p.PatientName,
                    TotalPayments = payments.Count,
                    TotalPatients = payments.Select(pat=>pat.PatientCID).Distinct().Count(),
                    TotalAmount = payments.Sum(pay=>pay.FinalAmount ?? pay.Amount)
                }).ToList();
            //

            // new 
            // payments?.Select(p => new PaymentReportModel()
            //{
            //    TotalPayments = payments.Count,
            //    TotalPatients = payments.Select(pat=>pat.PatientCID).Distinct().Count(),
            //    TotalAmount = payments.Sum(pay=>pay.FinalAmount)
            //}).ToList();

            return payments;
        }

        //protected void Export(object sender, EventArgs e)
        //{
        //    Warning[] warnings;
        //    string[] streamIds;
        //    string contentType;
        //    string encoding;
        //    string extension;

        //    //Export the RDLC Report to Byte Array.
        //    byte[] bytes = ReportViewer1.LocalReport.Render(rbFormat.SelectedItem.Value, null, out contentType, out encoding, out extension, out streamIds, out warnings);

        //    //Download the RDLC Report in Word, Excel, PDF and Image formats.
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.Charset = "";
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = contentType;
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=RDLC." + extension);
        //    Response.BinaryWrite(bytes);
        //    Response.Flush();
        //    Response.End();
        //}
    }
}