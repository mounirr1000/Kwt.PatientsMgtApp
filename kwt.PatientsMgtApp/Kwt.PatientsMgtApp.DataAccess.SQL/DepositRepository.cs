using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class DepositRepository : IDepositRepository
    {
        DomainObjectRepository _domainObjectRepository;
        public DepositRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public void DeleteDeposit(int DepositId)
        {
            var deposit = _domainObjectRepository.Get<DepositAccount>(d => d.DepositID == DepositId);
            if (deposit != null)
            {
                var budget = _domainObjectRepository.Get<BudgetTransaction>(b => b.DepositAccountID == deposit.DepositID);
                if (budget != null)
                    _domainObjectRepository.Delete<BudgetTransaction>(budget);
                _domainObjectRepository.Delete<DepositAccount>(deposit);
            }
        }

        public DepositAccountModel GetDeposit(int DepositId)
        {
            var dep = _domainObjectRepository.Get<DepositAccount>(d => d.DepositID == DepositId, new string[] { "Agency", "BudgetTransactions", "PayrollAccount", "DepositType", "DepositDepartment", "Payee" });

            return new DepositAccountModel()
            {
                Agency = dep.Agency.AgencyName,
                AccountName = dep.PayrollAccount.Description,
                AmountDeposited = dep.AmountDeposited,
                CreatedBy = dep.CreatedBy,
                CreatedDate = dep.CreatedDate,
                DepositDate = dep.DepositDate,
                DepositDepartment = dep.DepositDepartment.DepositDepartment1,
                DepositTitle = dep.DepositTitle,
                DepositType = dep.DepositType.DepositType1,
                Descriptions = dep.Descriptions,
                EndDate = dep.EndDare,
                PayeeName = dep.Payee.PayeeName,
                StartDate = dep.StartDate,
                AccountID = dep.AccountID,
                AgencyID = dep.AgencyID,
                DepositDepartmentID = dep.DepositDepartmentID,
                DepositID = dep.DepositID,
                DepositTypeID = dep.DepositTypeID,
                PayeeID = dep.PayeeID,
                DepositTypes = _domainObjectRepository.All<DepositType>().Select(dt => new DepositTypeModel()
                {
                    DepositType1 = dt.DepositType1,
                    DepositTypeID = dt.DepositTypeID
                }).ToList(),
                DepositDepartments = _domainObjectRepository.All<DepositDepartment>().Select(dd => new DepositDepartmentModel()
                {
                    DepositDepartment = dd.DepositDepartment1,
                    DepositDepartmentID = dd.DepositDepartmentID
                }).ToList(),
                PayrollAccounts = _domainObjectRepository.All<PayrollAccount>().Select(pa => new PayrollAccountModel()
                {
                    AccountType = pa.AccountType,
                    Description = pa.Description,
                    Grouping = pa.Grouping,
                    Parent = pa.Parent,
                    PayAccountID = pa.PayAccountID,
                    Title = pa.Title
                }).ToList(),
                PayeeList = _domainObjectRepository.All<Payee>().Select(pa => new PayeeModel()
                {
                    PayeeBankAccount = pa.PayeeBankAccount,
                    PayeeBankName = pa.PayeeBankName,
                    PayeeBankRoutingNumber = pa.PayeeBankRoutingNumber,
                    PayeeCity = pa.PayeeCity,
                    PayeeEmail = pa.PayeeEmail,
                    PayeeID = pa.PayeeID,
                    PayeeName = pa.PayeeName,
                    PayeePhone = pa.PayeePhone,
                    PayeeState = pa.PayeeState,
                    PayeeStreetAddress = pa.PayeeStreetAddress,
                    PayeeTypeID = pa.PayeeTypeID,
                    PayeeZipcode = pa.PayeeZipcode
                }).ToList(),
                Agencies = _domainObjectRepository.All<Agency>().Select(a => new AgencyModel()
                {
                    AgencyID = a.AgencyID,
                    AgencyName = a.AgencyName,
                }).ToList(),
            };
        }

        public List<DepositAccountModel> GetDepositList()
        {
            var deposit = _domainObjectRepository.All<DepositAccount>(new string[] { "Agency", "BudgetTransactions", "PayrollAccount", "DepositType", "DepositDepartment", "Payee" });
            return deposit.Select(d => new DepositAccountModel()
            {
                AccountID = d.AccountID,
                AgencyID = d.AgencyID,
                AmountDeposited = d.AmountDeposited,
                CreatedBy = d.CreatedBy,
                CreatedDate = d.CreatedDate,
                DepositDate = d.DepositDate,
                DepositDepartmentID = d.DepositDepartmentID,
                DepositID = d.DepositID,
                DepositTitle = d.DepositTitle,
                DepositTypeID = d.DepositTypeID,
                Descriptions = d.Descriptions,
                EndDate = d.EndDare,
                PayeeID = d.PayeeID,
                PayeeName = d.PayeeName ?? d.Payee.PayeeName,
                StartDate = d.StartDate,
                DepositType = d.DepositType.DepositType1,
                Agency = d.Agency.AgencyName,
                DepositDepartment = d.DepositDepartment.DepositDepartment1,
                AccountName = d.PayrollAccount.Description,
            }).ToList();
        }

        public DepositAccountModel GetDepositObject()
        {
            return new DepositAccountModel()
            {
                DepositTypes = _domainObjectRepository.All<DepositType>().Select(dt => new DepositTypeModel()
                {
                    DepositType1 = dt.DepositType1,
                    DepositTypeID = dt.DepositTypeID
                }).ToList(),
                DepositDepartments = _domainObjectRepository.All<DepositDepartment>().Select(dd => new DepositDepartmentModel()
                {
                    DepositDepartment = dd.DepositDepartment1,
                    DepositDepartmentID = dd.DepositDepartmentID
                }).ToList(),
                PayrollAccounts = _domainObjectRepository.All<PayrollAccount>().Select(pa => new PayrollAccountModel()
                {
                    AccountType = pa.AccountType,
                    Description = pa.Description,
                    Grouping = pa.Grouping,
                    Parent = pa.Parent,
                    PayAccountID = pa.PayAccountID,
                    Title = pa.Title
                }).ToList(),
                PayeeList = _domainObjectRepository.All<Payee>().Select(pa => new PayeeModel()
                {
                    PayeeBankAccount = pa.PayeeBankAccount,
                    PayeeBankName = pa.PayeeBankName,
                    PayeeBankRoutingNumber = pa.PayeeBankRoutingNumber,
                    PayeeCity = pa.PayeeCity,
                    PayeeEmail = pa.PayeeEmail,
                    PayeeID = pa.PayeeID,
                    PayeeName = pa.PayeeName,
                    PayeePhone = pa.PayeePhone,
                    PayeeState = pa.PayeeState,
                    PayeeStreetAddress = pa.PayeeStreetAddress,
                    PayeeTypeID = pa.PayeeTypeID,
                    PayeeZipcode = pa.PayeeZipcode
                }).ToList(),
                Agencies = _domainObjectRepository.All<Agency>().Select(a => new AgencyModel()
                {
                    AgencyID = a.AgencyID,
                    AgencyName = a.AgencyName,
                }).ToList(),
            };


        }

        public DepositAccountModel NewDeposit(DepositAccountModel dep)
        {
            var deposit = new DepositAccount()
            {
                AgencyID = dep.AgencyID,
                AmountDeposited = dep.AmountDeposited,
                CreatedDate = DateTime.Now,
                DepositDate = DateTime.Now,
                DepositDepartmentID = dep.DepositDepartmentID,
                AccountID = dep.AccountID,
                DepositTitle = dep.DepositTitle,
                DepositTypeID = dep.DepositTypeID,
                Descriptions = dep.Descriptions,
                EndDare = dep.EndDate,
                PayeeID = dep.PayeeID,
                PayeeName = dep.PayeeName,
                StartDate = dep.StartDate,
                CreatedBy = dep.CreatedBy
            };
            var createdDeposit = _domainObjectRepository.Create<DepositAccount>(deposit);
            CreateBudgetTransactions(createdDeposit);
            return dep;
        }

        public void UpdateDeposit(DepositAccountModel dep)
        {
            var de = _domainObjectRepository.Get<DepositAccount>(d => d.DepositID == dep.DepositID, new string[] { "Agency", "BudgetTransactions", "PayrollAccount", "DepositType", "DepositDepartment", "Payee" });
            if (de != null)
            {

                de.AgencyID = dep.AgencyID;
                de.AmountDeposited = dep.AmountDeposited;
                de.CreatedDate = DateTime.Now;
                de.DepositDate = DateTime.Now;
                de.DepositDepartmentID = dep.DepositDepartmentID;
                de.AccountID = dep.AccountID;
                de.DepositTitle = dep.DepositTitle;
                de.DepositTypeID = dep.DepositTypeID;
                de.Descriptions = dep.Descriptions;
                de.EndDare = dep.EndDate;
                de.PayeeID = dep.PayeeID;
                de.PayeeName = dep.PayeeName;
                de.StartDate = dep.StartDate;
               

                _domainObjectRepository.Update<DepositAccount>(de);
                var budget = de.BudgetTransactions.FirstOrDefault();
                budget.BudgetAmount = dep.AmountDeposited;
                _domainObjectRepository.Update<BudgetTransaction>(budget);
            }
        }

        //when there is a new deposit we add the amount to the total budget, when there is a payroll transaction we substract the amount from the total budget
        public void CreateBudgetTransactions(DepositAccount dep = null, Payroll payroll = null)
        {
            // var totalBudget = _domainObjectRepository.All<BudgetTransaction>()?.Sum(b => b.BudgetAmount);
            //var budgetList = _domainObjectRepository.All<BudgetTransaction>().ToList();
            //var lastBudgetTransactionID = budgetList.Count > 0 ? budgetList.Max(b => b.ID) : 0;
            //var lastBudgetAmount = _domainObjectRepository.Get<BudgetTransaction>(b => b.ID == lastBudgetTransactionID)?.BudgetAmount;
            BudgetTransaction budget = new BudgetTransaction();
            if (dep != null)
            {
                budget = new BudgetTransaction()
                {
                    BudgetAmount = dep.AmountDeposited,//lastBudgetAmount != null ? lastBudgetAmount + dep.AmountDeposited : dep.AmountDeposited,
                    DepositAccountID = dep.DepositID,
                };
            }
            else if (payroll != null)
            {
                budget = new BudgetTransaction()
                {
                    BudgetAmount = payroll.Amount * (-1),//lastBudgetAmount != null ? lastBudgetAmount - payroll.Amount : payroll.Amount * (-1),
                    PayrollID = payroll.TransactionID,
                };
            }
            _domainObjectRepository.Create<BudgetTransaction>(budget);
        }

        public List<DepositReportModel> GetDepositReport(DateTime? startDate = null, DateTime? endDate = null, int? payeeId = null, int? reportType=null)
        {
           
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("startDate", startDate);
            parms.Add("endDate", endDate);            
            parms.Add("payeeId", payeeId);
            parms.Add("reportType", reportType);
            var report = _domainObjectRepository.ExecuteProcedure<DepositReportModel>("GetDepositReport_SP", parms, false);
            return report;
        }
        //
        public List<DepositReportModel> GetDepositReportWithoutParms()
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("startDate", null);
            parms.Add("endDate", null);            
            parms.Add("payeeId", null);

            var report = _domainObjectRepository.ExecuteProcedure<DepositReportModel>("GetDepositReport_SP", parms, false);
            return report;


        }
    }
}
