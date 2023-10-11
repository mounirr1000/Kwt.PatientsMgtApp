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
    public partial class Deposit : System.Web.UI.Page
    {
        readonly IDepositRepository _depositRepository = new DepositRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPayeeDropDown();                
                GenerateReport();
            }
        }


        private void SetPayeeDropDown()
        {
            //var banks = _bankRepository.GetBanks();
            var payeeList = _depositRepository.GetDepositObject().PayeeList;
            Payees.DataSource = payeeList;
            Payees.DataTextField = "PayeeName";
            Payees.DataValueField = "PayeeID";
            Payees.DataBind();
            Payees.Items.Insert(0, "-----------Select Payee------------");
            Payees.SelectedIndex = 0;
        }
       
        protected void Search_Click(object sender, EventArgs e)
        {
            string selectedValue = ReportTypes.SelectedItem.Value;
            //   Response.Write(Banks.SelectedItem.Value);

            int reportType;
            Int32.TryParse(selectedValue, out reportType);
            GenerateReport(reportType);

        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            StartDate.Text = null;
            EndDate.Text = null;
           
            SetPayeeDropDown();
            GenerateReport();
        }
        protected void reportTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = ReportTypes.SelectedItem.Value;


            int reportType;
            Int32.TryParse(selectedValue, out reportType);
            GenerateReport(reportType);

        }
        private void GenerateReport(int? reportType = null)
        {

            var startDate = new DateTime();
            var endDate = new DateTime();
            int? pId = null;
            DateTime? sDate = null;
            DateTime? eDate = null;
            int payeeId = 0;
            string payee = Payees.SelectedItem.Value;
            if (Int32.TryParse(payee, out payeeId))
            {
                pId = payeeId;
            }

            if (DateTime.TryParse(StartDate.Text, out startDate))
            {
                sDate = startDate;

            }
            if (DateTime.TryParse(EndDate.Text, out endDate))
            {
                eDate = endDate;
            }

            string reportName = "Budget";
            string mapPath = "~/Reports/DepositReport.rdlc";
            string dataSource = "DepositDataSet";
            switch (reportType)
            {
                case 1:
                    reportName = "Outstanding Checks & Wires";
                    mapPath = "~/Reports/DepositDisbursementReport.rdlc";                   
                    break;

                default:
                    reportName = "Budget";
                    break;

            }
            List<DepositReportModel> deposit = new List<DepositReportModel>();

            deposit = _depositRepository.GetDepositReport(sDate, eDate, pId, reportType);// 1 for kfh bank id, 4 for da agency id



            if (deposit == null || deposit.Count == 0)
            {
                Message.Enabled = true;
                ErrorMessage.Visible = true;
                Message.Text = "There is no payment to display in the report in the provided criteria";
                ReportViewer1.Visible = false;
            }
            else
            {
                Message.Text = "";
                ErrorMessage.Visible = false;
                Message.Enabled = false;
                ReportViewer1.Visible = true;


                ReportViewer1.LocalReport.ReportPath = Server.MapPath(mapPath);
                ReportViewer1.LocalReport.DisplayName = reportName +
                                                        (startDate != null && endDate != null ? " From: " +
                                                         StartDate.Text + " To: " + EndDate.Text : "");
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rdc = new ReportDataSource(dataSource, deposit);
                //new 
                //ReportParameter stDate = new ReportParameter("StartDate", startDate?.ToString());
                //ReportParameter enDate = new ReportParameter("EndDate", endDate?.ToString());
                //ReportParameter rtType = new ReportParameter("ReportType", reportType?.ToString());
                // ReportParameter reason = new ReportParameter("DeductionReason", payments?.Select(r=>r.DeductionReasonText).ToString());

                //ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { stDate, enDate, rtType });
                // end new
                ReportViewer1.LocalReport.DataSources.Add(rdc);

                // ReportViewer1.EnableClientPrinting
                ReportViewer1.LocalReport.Refresh();

            }



            //  return payments;
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
    }
}