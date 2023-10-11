using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IDepositRepository
    {
        List<DepositAccountModel> GetDepositList();
        DepositAccountModel GetDeposit(int DepositId);
        void UpdateDeposit(DepositAccountModel Deposit);                
        void DeleteDeposit(int DepositId);       
        DepositAccountModel NewDeposit(DepositAccountModel Deposit);                       
        DepositAccountModel GetDepositObject();
        void CreateBudgetTransactions(DepositAccount dep = null, Payroll payroll = null);
        List<DepositReportModel> GetDepositReport(DateTime? startDate = null, DateTime? endDate = null, int? payeeId = null, int? reportType = null);
        List<DepositReportModel> GetDepositReportWithoutParms();
    }
}
