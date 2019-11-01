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
        readonly IBankRepository _bankRepository = new BankRepository();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                GenerateReport();
                SetBanksDropDown();
                // uncomment this line to show banks dorpdown menu
                Banks.Visible = false;
            }
          //  SetBanksDropDown();
        }


        private void SetBanksDropDown()
        {
            //var banks = _bankRepository.GetBanks();
            Banks.DataSource = _bankRepository.GetBanks(); 
            Banks.DataTextField = "BankName";
            Banks.DataValueField = "BankID";
            Banks.DataBind();
            Banks.Items.Insert(0, "-----------Select Bank------------");
            Banks.SelectedIndex = 0;
        }
        protected void Search_Click(object sender, EventArgs e)
        {

            string selectedValue = ReportTypes.SelectedItem.Value;
         //   Response.Write(Banks.SelectedItem.Value);

            int reportType;
            Int32.TryParse(selectedValue, out reportType);
            //  ReportViewer1.Visible = false;
            GenerateReportType(reportType);
            // ReportViewer1.Visible = true;

        }

        protected void Clear_Click(object sender, EventArgs e)
        {
            StartDate.Text = null;
            EndDate.Text = null;
            PatientCid.Text = null;
            SetBanksDropDown();
            GenerateReport();
        }

        private void GenerateReport(string patientCid = null, DateTime? startDateTxt = null, DateTime? endDateTxt = null, int? reportType = 3, int?  bankId=null)
        {

            var patientId = string.IsNullOrEmpty(patientCid) ? null : patientCid;
            var startDate = startDateTxt ?? (patientId != null ? startDateTxt : DateTime.Now);
            var endDate = endDateTxt ?? (patientId != null ? endDateTxt : DateTime.Now); //endDateTxt ?? DateTime.Now; 
            var bankid = bankId == 0 ? null : bankId;
            string reportName = "Details";
            string mapPath = "~/Reports/PaymentReport.rdlc";
            string dataSource = "PaymentList";
            switch (reportType)
            {
                case 1:
                    reportName = "Bank";
                    mapPath = "~/Reports/BankPaymentReport.rdlc";
                    dataSource = "BankPaymentReportDataset";
                    break;
                case 2:
                    reportName = "Archive";
                    mapPath = "~/Reports/ArchivePaymentReport.rdlc";
                    dataSource = "ArchivePaymentReportDataset";
                    break;
                case 4:
                    reportName = "Ministry";
                    mapPath = "~/Reports/MinistryPaymentReport.rdlc";
                    dataSource = "MinistryPaymentReportDataset";
                    break;
                case 5:
                    reportName = "Statistical";
                    mapPath = "~/Reports/StatisticalPaymentReport.rdlc";
                    dataSource = "StatisticalPaymentReportDataset";
                    break;
                case 6:
                    reportName = "Statistical_2";
                    mapPath = "~/Reports/StatisticalPaymentReport_2.rdlc";
                    dataSource = "StatisticalPaymentDataset";
                    break;
                default:
                    reportName = "Details";
                    break;

            }
            List<PaymentReportModel> payments = new List<PaymentReportModel>();
            if (reportType == 6)
            {
                payments = _paymentRepository.GetStatisticalPaymentsReport( startDate, endDate, 1,4);// 1 for kfh bank id, 4 for da agency id
            }
            else if (reportType == 2)//archive report
            {
                payments = _paymentRepository.GetArchivePaymentsReport(patientId, startDate, endDate, bankid, reportType);//?.OrderByDescending(p => p.CreatedDate)?.ToList();    
            }
            else
            {
                payments = _paymentRepository.GetPaymentsReport(patientId, startDate, endDate, bankid);//?.OrderByDescending(p => p.CreatedDate)?.ToList();    
            }
            

            List<PaymentReportModel> returnedPayments = new List<PaymentReportModel>();
            if (payments == null || payments.Count == 0)
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
                returnedPayments = PaymentTypeForReportingType(payments, reportType);
                if (returnedPayments.Count == 0)
                {
                    Message.Enabled = true;
                    ErrorMessage.Visible = true;
                    Message.Text = "There is no payment to display in the report in the provided criteria";
                    ReportViewer1.Visible = false;
                }
                else
                {
                    //
                    if (reportType ==1)//only bank report
                    {
                        reportName = reportName + "[" + payments?.Select(p => p.SumTotalHash).FirstOrDefault()+"]";
                    }
                    //
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath(mapPath);
                    ReportViewer1.LocalReport.DisplayName = reportName +
                                                            (startDate != null && endDate != null ? " From: " +
                                                             StartDate.Text + " To: " + EndDate.Text : "");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rdc = new ReportDataSource(dataSource, returnedPayments);
                    //new 
                    ReportParameter stDate = new ReportParameter("StartDate", startDate?.ToString());
                    ReportParameter enDate = new ReportParameter("EndDate", endDate?.ToString());
                    ReportParameter rtType = new ReportParameter("ReportType", reportType?.ToString());
                    // ReportParameter reason = new ReportParameter("DeductionReason", payments?.Select(r=>r.DeductionReasonText).ToString());

                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { stDate, enDate, rtType });
                    // end new
                    ReportViewer1.LocalReport.DataSources.Add(rdc);

                    // ReportViewer1.EnableClientPrinting
                    ReportViewer1.LocalReport.Refresh();
                }
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

        protected void reportTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedValue = ReportTypes.SelectedItem.Value;


            int reportType;
            Int32.TryParse(selectedValue, out reportType);
            GenerateReportType(reportType);

        }

        private void GenerateReportType(int? reportType)
        {
            var startDate = new DateTime();
            var endDate = new DateTime();
            //var resultCount = 0;
            int bankId = 0;
            string selectedBankId = Banks.SelectedItem.Value;
            Int32.TryParse(selectedBankId, out bankId);

            if (DateTime.TryParse(StartDate.Text, out startDate) && DateTime.TryParse(EndDate.Text, out endDate))
            {
                GenerateReport(PatientCid.Text, startDate, endDate, reportType, bankId);
            }
            else
            {

                // resultCount = GenerateReport(PatientCid.Text, null, null, reportType).Count;
                GenerateReport(PatientCid.Text, null, null, reportType, bankId);
            }
        }

        private List<PaymentReportModel> PaymentTypeForReportingType(List<PaymentReportModel> payments, int? reportType = null)
        {
            List<PaymentReportModel> processedPayments = new List<PaymentReportModel>();
            if (reportType == (int)Enums.ReportType.Kuwait)// sent to kuwait = Bank Report
                processedPayments = CreateKuwaitPaymentReport(payments);
            else if (reportType == (int)Enums.ReportType.Archive)// saved in Archive
                processedPayments = CreateArchivePaymentReport(payments);
            else if (reportType == (int)Enums.ReportType.Details)
                processedPayments = CreateDetailsPaymentReport(payments);
            else if (reportType == (int)Enums.ReportType.Statistical)
                processedPayments = CreateDetailsPaymentReport(payments);
            else if (reportType == (int)Enums.ReportType.Statistical2)
                processedPayments = CreateStatisticalPaymentReport(payments);
            //

            else if (reportType == (int)Enums.ReportType.Ministry)
                processedPayments = CreateMinistryPaymentReport(payments);

            return processedPayments?.OrderByDescending(p => p.PaymentID).ToList();
        }

        private List<PaymentReportModel> CreateMinistryPaymentReport(List<PaymentReportModel> payments)
        {
            payments = CreateDetailsPaymentReport(payments);
            //new get the corrected payments, and search for it related rejected ids, add payments with the same info except the payment date
            var correctedPayments = payments?.Where(p => p.PaymentTypeId == (int)Enums.PaymentType.Correction).ToList();
            // to is correction payment 
            // in  this correctedPayments i have the ids of the payment that were rejected

            //var paymentList = _paymentRepository.GetPaymentsReport(null, DateTime.Parse("01/01/2019"),DateTime.Now);
            if (correctedPayments != null)
            {
                foreach (var cor in correctedPayments)
                {
                    var paymentList = _paymentRepository.GetPaymentsReportWithoutParms();
                    var newPayment = paymentList?.SingleOrDefault(py => py.PaymentID == cor.RejectedPaymentId);

                    if (newPayment != null)
                    {
                        if (payments.Select(p => p.PaymentID).Contains(newPayment.PaymentID))
                        {
                            var paymentsToRemove = payments.SingleOrDefault(p => p.PaymentID == newPayment.PaymentID);
                            if (paymentsToRemove != null)
                            {
                                paymentsToRemove.IsPaymentRejected = false;
                                payments.Remove(paymentsToRemove);
                            }

                            //payments.Remove(paymentsToRemove);
                            //payments.Remove(payments.SingleOrDefault(py => py.PaymentID == newPayment.PaymentID));
                        }
                        PaymentReportModel paymentToAdd = new PaymentReportModel();

                        // modifiy the paymentDate to be the same as the corrected payment
                        paymentToAdd.PaymentDate = cor.PaymentDate;
                        paymentToAdd.PaymentID = cor.PaymentID;
                        paymentToAdd.RowNumber = cor.RowNumber;
                        paymentToAdd.IsPaymentRejected = true;
                        paymentToAdd.Amount = newPayment.Amount * -1;
                        paymentToAdd.FinalAmount = newPayment.FinalAmount * -1 ?? newPayment.Amount * -1;
                        paymentToAdd.PatientCID = newPayment.PatientCID;
                        paymentToAdd.PatientName = newPayment.PatientName;
                        paymentToAdd.BeneficiaryCID = newPayment.PaymentBeneficiaryCID;// newPayment.BeneficiaryCID;
                        paymentToAdd.BeneficiaryName = newPayment.PaymentBeneficiaryName;// newPayment.BeneficiaryName;
                        paymentToAdd.AgencyName = newPayment.RejectionDateFormatted;// display rejected date insyead of agency name
                        paymentToAdd.Code = newPayment.PaymentBankCode;// newPayment.Code;
                        paymentToAdd.IBan = newPayment.RejectionReason;// they want the rejection reason to be displayed in Iban field
                        paymentToAdd.StartDate = newPayment.StartDate;
                        paymentToAdd.EndDate = newPayment.EndDate;
                        paymentToAdd.CreatedDate = newPayment.CreatedDate;
                        paymentToAdd.RejectionDate = newPayment.RejectionDate;
                        payments.Add(paymentToAdd);
                    }
                }
            }
            return payments?.Where(p => p.IsVoid != true).ToList();
        }

        private List<PaymentReportModel> CreateDetailsPaymentReport(List<PaymentReportModel> payments)
        {
            payments = payments?.Where(p => p.IsVoid != true).ToList();
            DeductionReasonRepository _deductionReasonRepository = new DeductionReasonRepository();
            List<PaymentReportModel> processedPayments = new List<PaymentReportModel>();
            processedPayments = payments?.Select(p => new PaymentReportModel()
            {
                AgencyName = p.AgencyName,
                PaymentID = p.PaymentID,
                FinalAmount = (p.IsPaymentRejected == true ? p.FinalAmount * -1 : p.FinalAmount) ?? (p.IsPaymentRejected == true ? p.Amount * -1 : p.Amount),
                PatientCID = p.PatientCID,
                CreatedDate = p.CreatedDate,
                //BeneficiaryCID = p.PaymentBeneficiaryCID,// p.BeneficiaryCID,
                EndDate = p.EndDate,
                StartDate = p.StartDate,
                Amount = p.IsPaymentRejected == true ? p.Amount * -1 : p.Amount,
             //   Bank = p.PaymentBankName,// p.Bank,
              //  BeneficiaryCID = p.PaymentBeneficiaryCID,// p.BeneficiaryCID,
              //  BeneficiaryName = p.PaymentBeneficiaryName,// p.BeneficiaryName,
             //   IBan = p.PaymentIban?.ToUpper(),// p.IBan?.ToUpper(),
             //   Code = p.PaymentBankCode,//p.Code,
                CompanionName = p.CompanionName,
                DeductedAmount = p.DeductedAmount ?? 0,
                //IBan = p.PaymentIban?.ToUpper(),// p.IBan?.ToUpper(),
                PatientName = p.PatientName,
                TotalPayments = payments.Where(pa => pa.IsVoid != true).ToList().Count,
                TotalPatients = payments.Where(pa => pa.IsVoid != true).Select(pat => pat.PatientCID).Distinct().Count(),
                TotalAmount = payments.Where(pa => pa.IsVoid != true).Sum(pay => pay.FinalAmount),
                TotalDeduction = p.TotalDeduction,
                DeductionNotes = p.DeductionNotes,
                DeductionReason = p.DeductionReason,
                PatientDeductionEndDate = p.PatientDeductionEndDate,
                PatientDeductionStartDate = p.PatientDeductionStartDate,
                CompanionDeductionEndDate = p.CompanionDeductionEndDate,
                CompanionDeductionStartDate = p.CompanionDeductionStartDate,
                DeductionReasonText = p.DeductionReasonText,// _deductionReasonRepository.GetDeductionReason(p.DeductionReason)?.Reason,
                PatientDeduction = p.PatientDeduction,
                CompanionDeduction = p.CompanionDeduction,
                AmountBeforeDeduction = p.AmountBeforeDeduction,
                PaymentTypeId = p.PaymentTypeId,
                RejectedPaymentId = p.RejectedPaymentId,
                IsPaymentRejected = p.IsPaymentRejected,
                PaymentDate = p.PaymentDate,
                RejectionReasonID = p.RejectionReasonID,
                RejectionNotes = p.RejectionNotes,
                RejectionDate = p.RejectionDate,
                RejectionReason = p.RejectionReason,
                RowNumber = p.RowNumber,
                AdjustmentReason = p.AdjustmentReason,
                AdjustmentReasonID = p.AdjustmentReasonID,
                IsDeleted = p.IsDeleted,
                IsVoid = p.IsVoid,
                Bank = p.PaymentBankName,// p.Bank,
                BeneficiaryCID = p.PaymentBeneficiaryCID,// p.BeneficiaryCID,
                BeneficiaryName = p.PaymentBeneficiaryName,// p.BeneficiaryName,
                IBan = p.PaymentIban?.ToUpper(),// p.IBan?.ToUpper(),
                Code = p.PaymentBankCode,//p.Code,
            }).ToList();
            // get only non void payment
            return processedPayments?.Where(p=>p.IsVoid!=true).ToList();
        }

        private List<PaymentReportModel> CreateArchivePaymentReport(List<PaymentReportModel> payments)
        {
            DeductionReasonRepository _deductionReasonRepository = new DeductionReasonRepository();
            payments = payments?.Select(p => new PaymentReportModel()
            {
                AgencyName = p.AgencyName,
                PaymentID = p.PaymentID,
                FinalAmount = p.IsVoid!=true? (p.IsPaymentRejected == true ? p.FinalAmount * -1 : p.FinalAmount) ?? (p.IsPaymentRejected == true ? p.Amount * -1 : p.Amount):0,
                PatientCID = p.PatientCID,
                CreatedDate = p.CreatedDate,
              //  BeneficiaryCID = p.BeneficiaryCID,
                EndDate = p.EndDate,
                StartDate = p.StartDate,
                //  Amount = p.Amount,
               // Bank = p.PaymentBankName,// p.Bank,
               // BeneficiaryName = p.BeneficiaryName,
               // Code = p.Code,
                CompanionName = p.CompanionName,
                //   DeductedAmount = p.DeductedAmount ?? 0,
               // IBan = p.IBan?.ToUpper(),
                PatientName = p.PatientName,
                //TotalPayments = payments.Count,
                //TotalPatients = payments.Select(pat => pat.PatientCID).Distinct().Count(),
                TotalAmount = payments.Where(pa => pa.IsVoid != true).Sum(pay => pay.FinalAmount ?? pay.Amount),
                TotalDeduction = p.TotalDeduction,
                DeductionNotes = p.DeductionNotes,
                DeductionReason = p.DeductionReason,
                PatientDeductionEndDate = p.PatientDeductionEndDate,
                PatientDeductionStartDate = p.PatientDeductionStartDate,
                CompanionDeductionEndDate = p.CompanionDeductionEndDate,
                CompanionDeductionStartDate = p.CompanionDeductionStartDate,
                DeductionReasonText = p.DeductionReasonText,//_deductionReasonRepository.GetDeductionReason(p.DeductionReason)?.Reason,
                PatientDeduction = p.PatientDeduction,
                CompanionDeduction = p.CompanionDeduction,
                AmountBeforeDeduction = p.AmountBeforeDeduction,
                PaymentTypeId = p.PaymentTypeId,
                RejectedPaymentId = p.RejectedPaymentId,
                IsPaymentRejected = p.IsPaymentRejected,
                PaymentDate = p.PaymentDate,
                RejectionReasonID = p.RejectionReasonID,
                RejectionNotes = p.RejectionNotes,
                RejectionDate = p.RejectionDate,
                RejectionReason = p.RejectionReason,
                RowNumber = p.RowNumber,
                IsDeleted = p.IsDeleted,
                IsVoid = p.IsVoid,
                Bank = p.PaymentBankName,// p.Bank,
                BeneficiaryCID = p.PaymentBeneficiaryCID,// p.BeneficiaryCID,
                BeneficiaryName = p.PaymentBeneficiaryName,// p.BeneficiaryName,
                IBan = p.PaymentIban?.ToUpper(),// p.IBan?.ToUpper(),
                Code = p.PaymentBankCode,//p.Code,
            }).ToList();
            return payments;
        }

        private List<PaymentReportModel> CreateKuwaitPaymentReport(List<PaymentReportModel> payments)
        {
            payments = payments?.Where(p => p.IsVoid != true).ToList();
            DeductionReasonRepository _deductionReasonRepository = new DeductionReasonRepository();
            payments = payments?.Select(p => new PaymentReportModel()
            {
                AgencyName = p.AgencyName,
                PaymentID = p.PaymentID,
                FinalAmount = (p.IsPaymentRejected == true ? p.FinalAmount * -1 : p.FinalAmount) ?? (p.IsPaymentRejected == true ? p.Amount * -1 : p.Amount),
                PatientCID = p.PatientCID,
                CreatedDate = p.CreatedDate,
               // BeneficiaryCID = p.BeneficiaryCID,
                EndDate = p.EndDate,
                StartDate = p.StartDate,
                //Amount = p.Amount,
              //  Bank = p.Bank,
              //  BeneficiaryName = p.BeneficiaryName,
              //  Code = p.Code,
                CompanionName = p.CompanionName,
                // DeductedAmount = p.DeductedAmount ?? 0,
              //  IBan = p.IBan?.ToUpper(),
                PatientName = p.PatientName,
                //TotalPayments = payments.Count,
                //TotalPatients = payments.Select(pat => pat.PatientCID).Distinct().Count(),
                //TotalAmount = payments.Sum(pay => pay.FinalAmount ?? pay.Amount)
                TotalDeduction = p.TotalDeduction,
                DeductionNotes = p.DeductionNotes,
                DeductionReason = p.DeductionReason,
                PatientDeductionEndDate = p.PatientDeductionEndDate,
                PatientDeductionStartDate = p.PatientDeductionStartDate,
                CompanionDeductionEndDate = p.CompanionDeductionEndDate,
                CompanionDeductionStartDate = p.CompanionDeductionStartDate,
                DeductionReasonText = p.DeductionReasonText,//_deductionReasonRepository.GetDeductionReason(p.DeductionReason)?.Reason,
                PatientDeduction = p.PatientDeduction,
                CompanionDeduction = p.CompanionDeduction,
                AmountBeforeDeduction = p.AmountBeforeDeduction,
                PaymentTypeId = p.PaymentTypeId,
                RejectedPaymentId = p.RejectedPaymentId,
                IsPaymentRejected = p.IsPaymentRejected,
                PaymentDate = p.PaymentDate,
                RejectionReasonID = p.RejectionReasonID,
                RejectionNotes = p.RejectionNotes,
                RejectionDate = p.RejectionDate,
                RejectionReason = p.RejectionReason,
                RowNumber = p.RowNumber,
                IbanHash = p.IbanHash,
                TotalHash = p.TotalHash,
                IsDeleted = p.IsDeleted,
                IsVoid = p.IsVoid,
                Bank = p.PaymentBankName,// p.Bank,
                BeneficiaryCID = p.PaymentBeneficiaryCID,// p.BeneficiaryCID,
                BeneficiaryName = p.PaymentBeneficiaryName,// p.BeneficiaryName,
                IBan = p.PaymentIban?.ToUpper(),// p.IBan?.ToUpper(),
                Code = p.PaymentBankCode,//p.Code,
                //new sum totalHash
                SumTotalHash =p.SumTotalHash
                // TotalHash = payments.Sum(pay => pay.TotalHash),

            }).ToList();
                // payments with final amount is zero no need to be dispalyed in the report
            return payments?.Where(p => p.IsVoid != true).Where(p=>p.FinalAmount!=0).ToList();
        }

        private List<PaymentReportModel> CreateStatisticalPaymentReport(List<PaymentReportModel> payments)
        {
            payments = payments?.Where(p => p.IsVoid != true).ToList();
            List<PaymentReportModel> processedPayments = new List<PaymentReportModel>();
            processedPayments = payments?.Select(p => new PaymentReportModel()
            {
                StatisticalPaymentDate = p.PaymentDate,
                PaymentDate = p.PaymentDate,
                KfhBank =p.KfhBank,
                OtherBank=p.OtherBank,
                SubTotal= p.SubTotal,
                Rejected= p.Rejected,
                DAAgency=p.DAAgency,
                OtherAgencies=p.OtherAgencies,
                StatisticalTotalDeduction=p.TotalDeduction,
                TotalDeduction = p.TotalDeduction,
                FinalTotal =p.FinalTotal,
                Bank = p.PaymentBankName,// p.Bank,
                BeneficiaryCID = p.PaymentBeneficiaryCID,// p.BeneficiaryCID,
                BeneficiaryName = p.PaymentBeneficiaryName,// p.BeneficiaryName,
                IBan = p.PaymentIban?.ToUpper(),// p.IBan?.ToUpper(),
                Code = p.PaymentBankCode,//p.Code,
            }).ToList();
            return processedPayments?.Where(p => p.IsVoid != true).ToList();
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
        //---------------------------------------
        //    ReportViewer rv = new ReportViewer();
        //    rv.ProcessingMode = ProcessingMode.Local;
        //    rv.LocalReport.ReportPath = Server.MapPath("Report2.rdlc"); // 
        //    //point to the new rdlc without page breaks
        //    rv.LocalReport.DataSources.Clear();
        //    rv.LocalReport.DataSources.Add(...); // your report data source
        //    Warning[] warnings;
        //    string[] streamids;
        //    string mimeType;
        //    string encoding;
        //    string extension;
        //    byte[] bytes = rv.LocalReport.Render("EXCEL", null, out mimeType,
        //    out encoding, out extension, out streamids, out warnings);

        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.ContentType =
        //    "application/vnd.ms-excel";
        //    HttpContext.Current.Response.AddHeader("Content-disposition",
        //    "attachment; filename=Report.xls");
        //    HttpContext.Current.Response.BinaryWrite(bytes);
        //    HttpContext.Current.Response.End();
        //}
    }
}