using Kwt.PatientsMgtApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IPayrollRepository
    {
        List<PayrollModel> GetPayrollList();
        PayrollModel GetPayroll(int payrollId);
        void UpdatePayroll(PayrollModel payroll);
        void UpdatePayrollStatus(PayrollModel payroll, int curentStatusId);
        void UpdatePayrollsStatus(int[] payroll, int curentStatusId, string updatedBy);
        void DeletePayroll(int payrollId);
        void ResetPayroll(PayrollModel payroll, int payrollStatusId);

        PayrollModel NewPayroll(PayrollModel payroll);
        AccountModel UpdatePayrollAccount(AccountModel account);
        List<AccountModel> GetPayrollAccountList(int payrollId);
        void DeletePayrollAccount(int accountId);
        PayrollModel GetPayrollObject();
        List<PayrollReportModel> GetPayrollReport(DateTime? startDate = null, DateTime? endDate = null,
                                int? transactionId = null, int? employeeId = null, int? PayrollMethodId = null,
                                int? payeeId = null, int? payrollStatus = null, int? reportType = null);
        List<PayrollReportModel> GetPayrollReportWithoutParms();
    }
}
