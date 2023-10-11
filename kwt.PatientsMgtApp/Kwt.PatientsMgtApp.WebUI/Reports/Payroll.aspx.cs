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
    public partial class Payroll : System.Web.UI.Page
    {
        readonly IPayrollRepository _payrollRepository = new PayrollRepository();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetPayeeDropDown();
                SetPaymentTypeDropDown();
                SetEmployeelistDropDown();
                SetPayrollStatuslistDropDown();
                GenerateReport();
            }
        }


        private void SetPayeeDropDown()
        {
            //var banks = _bankRepository.GetBanks();
            var payeeList = _payrollRepository.GetPayrollObject().PayeeList;
            Payees.DataSource = payeeList;
            Payees.DataTextField = "PayeeName";
            Payees.DataValueField = "PayeeID";
            Payees.DataBind();
            Payees.Items.Insert(0, "-----------Select Payee------------");
            Payees.SelectedIndex = 0;
        }
        private void SetPaymentTypeDropDown()
        {
            //var banks = _bankRepository.GetBanks();
            var paymentType = _payrollRepository.GetPayrollObject().PayrollModethodList;
            PaymentTypes.DataSource = paymentType;
            PaymentTypes.DataTextField = "PayrollMethodName";
            PaymentTypes.DataValueField = "PayrollMethodId";
            PaymentTypes.DataBind();
            PaymentTypes.Items.Insert(0, "-----------Select Payment Type------------");
            PaymentTypes.SelectedIndex = 0;
        }
        private void SetEmployeelistDropDown()
        {
            //var banks = _bankRepository.GetBanks();
            var paymentType = _payrollRepository.GetPayrollObject().EmployeeList;
            Employees.DataSource = paymentType;
            Employees.DataTextField = "EmployeeName";
            Employees.DataValueField = "EmployeeID";
            Employees.DataBind();
            Employees.Items.Insert(0, "-----------Select Employee------------");
            Employees.SelectedIndex = 0;
        }
        private void SetPayrollStatuslistDropDown()
        {
            //var banks = _bankRepository.GetBanks();
            var payrollStatus = _payrollRepository.GetPayrollObject().PayrollStatusList;
            PayrollStatus.DataSource = payrollStatus;
            PayrollStatus.DataTextField = "PayrollStatusName";
            PayrollStatus.DataValueField = "PayrollStatusID";
            PayrollStatus.DataBind();
            PayrollStatus.Items.Insert(0, "-----------Select Payroll Status------------");
            PayrollStatus.SelectedIndex = 0;
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
            SetPaymentTypeDropDown();
            SetPayrollStatuslistDropDown();
            SetEmployeelistDropDown();
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
            //var resultCount = 0;
            int paymentTypeId = 0;
            int? ptId = null;
            int? eId = null;
            int? pId = null;
            int? psId = null;
            DateTime? sDate = null;
            DateTime? eDate = null;
            string paymentType = PaymentTypes.SelectedItem.Value;
            if (Int32.TryParse(paymentType, out paymentTypeId))
            {
                ptId = paymentTypeId;
            }

            int employeeId = 0;
            string employee = Employees.SelectedItem.Value;
            if (Int32.TryParse(employee, out employeeId))
            {
                eId = employeeId;
            }
            int payeeId = 0;
            string payee = Payees.SelectedItem.Value;
            if (Int32.TryParse(payee, out payeeId))
            {
                pId = payeeId;
            }
            int payStatusId = 0;
            string payrollStatusId = PayrollStatus.SelectedItem.Value;
            if (Int32.TryParse(payrollStatusId, out payStatusId))
            {
                psId = payStatusId;
            }
            if (DateTime.TryParse(StartDate.Text, out startDate))
            {
                sDate = startDate;

            }
            if (DateTime.TryParse(EndDate.Text, out endDate))
            {
                eDate = endDate;
            }

            string reportName = "Payroll";
            string mapPath = "~/Reports/PayrollReport.rdlc";
            string dataSource = "PayrollDataSet";
            switch (reportType)
            {
                case 1:
                    reportName = "Outstanding Checks & Wires";
                    mapPath = "~/Reports/PayrollReportOutstandingChecks.rdlc";
                    dataSource = "PayrollDataSet";
                    psId = 4;// in this case we only wants the payroll status to be paid payrolls and not yet reconsiled
                    break;

                default:
                    reportName = "Payroll";
                    break;

            }
            List<PayrollReportModel> payroll = new List<PayrollReportModel>();
            payroll = _payrollRepository.GetPayrollReport(sDate, eDate, null, eId, ptId, pId, psId,reportType);
            
            
               



            if (payroll == null || payroll.Count == 0)
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
                ReportDataSource rdc = new ReportDataSource(dataSource, payroll);
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