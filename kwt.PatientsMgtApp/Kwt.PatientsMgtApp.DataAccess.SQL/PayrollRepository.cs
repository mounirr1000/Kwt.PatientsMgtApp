using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System.Data.SqlClient;
using System.Data;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PayrollRepository : IPayrollRepository
    {

        private readonly IDomainObjectRepository _domainObjectRepository;
        private IDepositRepository _depositRepository;
        public PayrollRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
            _depositRepository = new DepositRepository();
        }
        public List<PayrollModel> GetPayrollList()
        {
            var payroll = _domainObjectRepository.All<Payroll>(new string[] { "CheckPayrolls", "WirePayrolls", "PayrollType", "Employee", "Account", "Agency", "Payee", "PayeesType", "PayrollMethod", "PayrollStatu" });

            return payroll.Select(p => new PayrollModel()
            {
                AccountNumber = p.AccountNumber,
                AgencyID = p.AgencyID,
                Amount = p.Amount != 0 ? p.Amount : p.Accounts.Sum(a => a.Amount - a.Discount) ?? 0,// p.Amount,
                CheckNumber = p.CheckNumber,
                Descriptions = p.Descriptions,
                EmployeeID = p.EmployeeID,
                PayeeID = p.PayeeID,
                PayeeName = p.PayeeName,
                PayeeTypeID = p.PayeeTypeID,
                PaymentApprovedBy = p.PaymentApprovedBy,
                PaymentApprovedDate = p.PaymentApprovedDate,
                PaymentAuthorizedBy = p.PaymentAuthorizedBy,
                PaymentAuthorizedDate = p.PaymentAuthorizedDate,
                PaymentCreatedBy = p.PaymentCreatedBy,
                PaymentCreatedDate = p.PaymentCreatedDate,
                PaymentDueDate = p.PaymentDueDate,
                PaymentEnteredBy = p.PaymentEnteredBy,
                PaymentEnteredDate = p.PaymentEnteredDate,
                PaymentModifiedBy = p.PaymentModifiedBy,
                PaymentModifiedDate = p.PaymentModifiedDate,
                PaymentPaidBy = p.PaymentPaidBy,
                PaymentPaidDate = p.PaymentPaidDate,
                PaymentReconciledBy = p.PaymentReconciledBy,
                PaymentReconciledDate = p.PaymentReconciledDate,
                PayrollMethodId = p.PayrollMethodId,
                PayrollStatusID = p.PayrollStatusID,
                PayrollTypeID = p.PayrollTypeID,
                TransactionID = p.TransactionID,
                TransactionIDFormat = p.TransactionIDFormat,
                WireNumber = p.WireNumber,
                //Account = new AccountModel()
                //{
                //    AccountName = p.Account?.AccountName,
                //    AccountNumber = p.Account.AccountNumber ,
                //    Amount = p.Account?.Amount,
                //    Descriptions = p.Account?.Descriptions,
                //    Discount = p.Account?.Discount
                //},
                //Agency = new AgencyModel()
                //{
                //    AgencyID = p.Agency.AgencyID,
                //    AgencyName = p.Agency.AgencyName,
                //},
                //Payee = new PayeeModel()
                //{
                //    PayeeBankAccount = p.Payee.PayeeBankAccount,
                //    PayeeBankName = p.Payee.PayeeBankName,
                //    PayeeBankRoutingNumber = p.Payee.PayeeBankRoutingNumber,
                //    PayeeCity = p.Payee.PayeeCity,
                //    PayeeEmail = p.Payee.PayeeEmail,
                //    PayeeID = p.Payee.PayeeID,
                //    PayeeName = p.Payee.PayeeName,
                //    PayeePhone = p.Payee.PayeePhone,
                //    PayeeState = p.Payee.PayeeState,
                //    PayeeStreetAddress = p.Payee.PayeeStreetAddress,
                //    PayeeTypeID = p.Payee.PayeeTypeID,
                //    PayeeZipcode = p.Payee.PayeeZipcode
                //},
                PayeesType = new PayeesTypeModel()
                {
                    PayeeType = p.PayeesType.PayeeType,
                    PayeeTypeID = p.PayeesType.PayeeTypeID
                },
                PayrollStatu = new PayrollStatusModel()
                {
                    PayrollStatusID = p.PayrollStatu.PayrollStatusID,
                    PayrollStatusName = p.PayrollStatu.PayrollStatusName,

                },
                PayrollMethod = new PayrollMethodModel()
                {
                    PayrollMethodId = p.PayrollMethod.PayrollMethodId,
                    PayrollMethodName = p.PayrollMethod.PayrollMethodName,

                },
                PayrollType = new PayrollTypeModel()
                {
                    PayrollTypeID = p.PayrollType.PayrollTypeID,
                    PayrollTypeName = p.PayrollType.PayrollTypeName,

                },

            }).OrderByDescending(pa => pa.TransactionID).ToList();
        }
        public void DeletePayroll(int payrollId)
        {
            var pay = _domainObjectRepository.Get<Payroll>(p => p.TransactionID == payrollId, new string[] { "WirePayrolls", "CheckPayrolls" });
            if (pay != null)
            {
                if (pay.WirePayrolls?.Count > 0)
                {
                    var wirePay = _domainObjectRepository.Get<WirePayroll>(c => c.PayrollID == pay.TransactionID);
                    if (wirePay != null)
                    {
                        _domainObjectRepository.Delete<WirePayroll>(wirePay);
                        ReseedIdentityColumnInTable("WirePayroll", "WireNumber");
                    }


                }
                if (pay.CheckPayrolls?.Count > 0)
                {
                    var checkPay = _domainObjectRepository.Get<CheckPayroll>(c => c.PayrollID == pay.TransactionID);
                    if (checkPay != null)
                    {
                        _domainObjectRepository.Delete<CheckPayroll>(checkPay);
                        ReseedIdentityColumnInTable("CheckPayrolls", "CheckNumber");
                    }

                }
                var accounts = _domainObjectRepository.All<Account>().Where(p => p.PayrollID == pay.TransactionID).ToList();
                foreach (var account in accounts)
                {
                    _domainObjectRepository.Delete<Account>(account);
                }

                _domainObjectRepository.Delete<Payroll>(pay);
                ReseedIdentityColumnInTable("Payrolls", "TransactionID");

            }
        }

        private PayrollModel CreatePayrollMethod(PayrollModel payroll)
        {
            var payMethodId = _domainObjectRepository.Get<PayrollMethod>(p => p.PayrollMethodName == "Wire").PayrollMethodId;
            var payrollObejct = _domainObjectRepository.Get<Payroll>(p => p.TransactionID == payroll.TransactionID);
            if (payroll.PayrollMethodId == payMethodId)//wire
            {
                // create wire id in wirepayment Table
                var wire = new WirePayroll()
                {
                    PayrollID = payroll.TransactionID,
                };
                var pay = _domainObjectRepository.Create<WirePayroll>(wire);
                payroll.WireNumber = pay.WireNumber;
                payroll.WireNumberFormat = pay.WireNumberFormat;
                // we create a budget transaction since we paid the payee we need to substract the amount from the budget
                _depositRepository.CreateBudgetTransactions(null, payrollObejct);
            }
            else
            {
                // create wire id in checkpayment Table
                var check = new CheckPayroll()
                {
                    PayrollID = payroll.TransactionID,
                };
                _domainObjectRepository.Create<CheckPayroll>(check);
                payroll.CheckNumber = check.CheckNumber;
                payroll.CheckNumberFormat = check.CheckNumberFormat;
                // we create a budget transaction since we paid the payee we need to substract the amount from the budget
                _depositRepository.CreateBudgetTransactions(null, payrollObejct);

            }
            return payroll;
        }
        public void UpdatePayrollStatus(PayrollModel payroll, int curentStatusId)
        {
            var pay = _domainObjectRepository.Get<Payroll>(p => p.TransactionID == payroll.TransactionID);
            //  --PayrollStatusID int not null, --entered, approved, authorized, paid, reconciled
            // -- WireNumber int  null ,--WireNumber is created once the status is paid and payRollmethod is a wire
            //--  CheckNumber int null,--CheckNumber is created once the status is paid and payRollmethod is a check
            switch (curentStatusId)
            {
                case 1:// curentStatus
                    pay.PayrollStatusID = 2;// approved
                    pay.PaymentApprovedBy = payroll.UpdatedBy;
                    pay.PaymentApprovedDate = DateTime.Now;
                    break;
                case 2:
                    pay.PayrollStatusID = 3;// Authorized
                    pay.PaymentAuthorizedBy = payroll.UpdatedBy;
                    pay.PaymentAuthorizedDate = DateTime.Now;
                    break;
                case 3:
                    pay.PayrollStatusID = 4;// Paid
                    pay.PaymentPaidBy = payroll.UpdatedBy;
                    pay.PaymentPaidDate = DateTime.Now;

                    // create payroll method
                    var payRoll = CreatePayrollMethod(payroll);
                    pay.CheckNumber = payRoll.CheckNumber;
                    pay.WireNumber = payRoll.WireNumber;
                    break;
                case 4:
                    pay.PayrollStatusID = 5;// Reconciled
                    pay.PaymentReconciledBy = payroll.UpdatedBy;
                    pay.PaymentReconciledDate = DateTime.Now;
                    break;
            }
            _domainObjectRepository.Update<Payroll>(pay);
            /*
            --	PayrollStatusID int not null, -- entered, approved, authorized, paid, reconciled
            --	WireNumber int  null ,-- WireNumber is created once the status is paid and payRollmethod is a wire
            --	CheckNumber int null,-- CheckNumber is created once the status is paid and payRollmethod is a check

            --	PaymentApprovedBy nvarchar(50)  null,-- for approved status id
            --	PaymentApprovedDate Date  null,-- for approved status id
            --	PaymentAuthorizedBy nvarchar(50)  null,-- for authorized status id
            --	PaymentAuthorizedDate Date  null,-- for authorized status id
            --	PaymentPaidBy nvarchar(50)  null,-- for paid status id
            --	PaymentPaidDate Date  null,-- for paid status id
            --	PaymentReconciledBy nvarchar(50)  null,-- for reconciled status id
            --	PaymentReconciledDate Date  null,-- for reconciled status id

            --	PaymentModifiedDate Date  null,
            --	PaymentModifiedBy nvarchar(50)  null,

            --	Descriptions nvarchar(max)  null,

         */

        }

        private Payroll CreatePayrollWireCheckMethod(Payroll payroll)
        {
            var payMethodId = _domainObjectRepository.Get<PayrollMethod>(p => p.PayrollMethodName == "Wire").PayrollMethodId;
            if (payroll.PayrollMethodId == payMethodId)//wire
            {
                // create wire id in wirepayment Table
                var wire = new WirePayroll()
                {
                    PayrollID = payroll.TransactionID,
                };
                var pay = _domainObjectRepository.Create<WirePayroll>(wire);
                payroll.WireNumber = pay.WireNumber;
                // payroll.WireNumberFormat = pay.WireNumberFormat;
                // we create a budget transaction since we paid the payee we need to substract the amount from the budget
                _depositRepository.CreateBudgetTransactions(null, payroll);
            }
            else
            {
                // create wire id in checkpayment Table
                var check = new CheckPayroll()
                {
                    PayrollID = payroll.TransactionID,
                };
                _domainObjectRepository.Create<CheckPayroll>(check);
                payroll.CheckNumber = check.CheckNumber;
                // we create a budget transaction since we paid the payee we need to substract the amount from the budget
                _depositRepository.CreateBudgetTransactions(null, payroll);
                
                // payroll.CheckNumberFormat = check.CheckNumberFormat;
            }
            return payroll;
        }
        public void UpdatePayrollsStatus(int[] payroll, int curentStatusId, string updatedBy)
        {
            //.Where(a => listOfSearchedIds.Contains(a.Id)).Any()).ToList()
            var pays = _domainObjectRepository.All<Payroll>().Where(p => payroll.Contains(p.TransactionID)).ToList();//payroll.Contains(p.TransactionID) ).ToList();
            //  --PayrollStatusID int not null, --entered, approved, authorized, paid, reconciled
            // -- WireNumber int  null ,--WireNumber is created once the status is paid and payRollmethod is a wire
            //--  CheckNumber int null,--CheckNumber is created once the status is paid and payRollmethod is a check
            foreach (var pay in pays)
            {
                switch (curentStatusId)
                {
                    case 1:// curentStatus
                        pay.PayrollStatusID = 2;// approved
                        pay.PaymentApprovedBy = updatedBy;
                        pay.PaymentApprovedDate = DateTime.Now;
                        break;
                    case 2:
                        pay.PayrollStatusID = 3;// Authorized
                        pay.PaymentAuthorizedBy = updatedBy;
                        pay.PaymentAuthorizedDate = DateTime.Now;
                        break;
                    case 3:
                        pay.PayrollStatusID = 4;// Paid
                        pay.PaymentPaidBy = updatedBy;
                        pay.PaymentPaidDate = DateTime.Now;

                        // create payroll method
                        var payRoll = CreatePayrollWireCheckMethod(pay);
                        pay.CheckNumber = payRoll.CheckNumber;
                        pay.WireNumber = payRoll.WireNumber;
                        break;
                    case 4:
                        pay.PayrollStatusID = 5;// Reconciled
                        pay.PaymentReconciledBy = updatedBy;
                        pay.PaymentReconciledDate = DateTime.Now;
                        break;
                }
            }

            _domainObjectRepository.UpdateBulk<Payroll>(pays);
        }

        public void ResetPayroll(PayrollModel payroll, int payrollStatusId)
        {
            var pay = _domainObjectRepository.Get<Payroll>(p => p.TransactionID == payroll.TransactionID);

            switch (payrollStatusId)
            {
                case 2:// reset approved to entered
                    pay.PayrollStatusID = 1;// approved
                    pay.PaymentApprovedBy = null;
                    pay.PaymentApprovedDate = null;
                    break;
                case 3:// reset Authorized to approved
                    pay.PayrollStatusID = 2;// approved
                    pay.PaymentAuthorizedBy = null;
                    pay.PaymentAuthorizedDate = null;
                    break;

            }

            _domainObjectRepository.Update<Payroll>(pay);
        }
        public PayrollModel GetPayroll(int payrollId)
        {
            var payroll = _domainObjectRepository.Get<Payroll>(p => p.TransactionID == payrollId, new string[] { "CheckPayrolls", "WirePayrolls", "PayrollType", "Employee", "Account", "Accounts", "Agency", "Payee", "PayeesType", "PayrollMethod", "PayrollStatu" });
            if (payroll == null)
                return null;
            // get the employee object when the payroll is for employee
            var employee = _domainObjectRepository.Get<Employee>(e => e.EmployeeID == payroll.EmployeeID);

            PayrollModel pay = new PayrollModel()
            {

                PayrollMethodId = payroll.PayrollMethodId,
                PayrollStatusID = payroll.PayrollStatusID,
                PayrollTypeID = payroll.PayrollTypeID,
                PayeeTypeID = payroll.PayeeTypeID,
                // PayeeID= p.EmployeeID,// only when there is a payee other than employee
                EmployeeID = payroll.EmployeeID,
                PayeeName = payroll.PayeeName,
                Amount = payroll.Amount,
                PaymentDueDate = payroll.PaymentDueDate,
                PaymentCreatedDate = payroll.PaymentCreatedDate,
                PaymentCreatedBy = payroll.PaymentCreatedBy,
                PaymentEnteredBy = payroll.PaymentEnteredBy,
                PaymentEnteredDate = payroll.PaymentEnteredDate,
                AccountNumber = payroll.AccountNumber,
                AgencyID = payroll.AgencyID,
                CheckNumber = payroll.CheckNumber,
                Descriptions = payroll.Descriptions,
                PayeeID = payroll.PayeeID,
                PaymentApprovedBy = payroll.PaymentApprovedBy,
                PaymentApprovedDate = payroll.PaymentApprovedDate,
                PaymentAuthorizedBy = payroll.PaymentAuthorizedBy,
                PaymentAuthorizedDate = payroll.PaymentAuthorizedDate,
                PaymentModifiedBy = payroll.PaymentModifiedBy,
                PaymentModifiedDate = payroll.PaymentModifiedDate,
                PaymentPaidBy = payroll.PaymentPaidBy,
                PaymentPaidDate = payroll.PaymentPaidDate,
                PaymentReconciledBy = payroll.PaymentReconciledBy,
                PaymentReconciledDate = payroll.PaymentReconciledDate,
                TransactionID = payroll.TransactionID,
                TransactionIDFormat = payroll.TransactionIDFormat,
                WireNumber = payroll.WireNumber,
                StartDate = payroll.StartDate,
                EndDate = payroll.EndDate,
                Accounts = payroll.Accounts?.Select(a => new AccountModel()
                {
                    AccountName = a.AccountName,
                    AccountNumber = a.AccountNumber,
                    Amount = a.Amount,
                    Descriptions = a.Descriptions,
                    Discount = a.Discount,
                    PayrollAccountID = a.PayrollAccountID,
                    PayrollID = a.PayrollID,
                    PayrollAccounts = _domainObjectRepository.All<PayrollAccount>().Select(pa => new PayrollAccountModel()
                    {
                        AccountType = pa.AccountType,
                        Description = pa.Description,
                        Grouping = pa.Grouping,
                        Parent = pa.Parent,
                        PayAccountID = pa.PayAccountID,
                        Title = pa.Title
                    }).ToList(),
                    DiscountTypeId = a.DiscountTypeId,
                    AmountAfterDiscount = a.AmountAfterDiscount,
                    DiscountTypes = _domainObjectRepository.All<DiscountType>().Select(d => new DiscountTypeModel()
                    {
                        DiscountType1 = d.DiscountType1,
                        DiscountTypeId = d.DiscountTypeId
                    }).ToList(),
                    DiscountTypeName = _domainObjectRepository.Get<DiscountType>(d => d.DiscountTypeId == a.DiscountTypeId)?.DiscountType1
                }).ToList(),
                //Agency = new AgencyModel()
                //{
                //    AgencyID= payroll.Agency.AgencyID,
                //    AgencyName= payroll.Agency.AgencyName,
                //},
                //Payee = new PayeeModel()
                //{
                //    PayeeBankAccount = payroll.Payee.PayeeBankAccount,
                //    PayeeBankName= payroll.Payee.PayeeBankName,
                //    PayeeBankRoutingNumber= payroll.Payee.PayeeBankRoutingNumber,
                //    PayeeCity = payroll.Payee.PayeeCity,
                //    PayeeEmail= payroll.Payee.PayeeEmail,
                //    PayeeID= payroll.Payee.PayeeID,
                //    PayeeName= payroll.Payee.PayeeName,
                //    PayeePhone= payroll.Payee.PayeePhone,
                //    PayeeState= payroll.Payee.PayeeState,
                //    PayeeStreetAddress= payroll.Payee.PayeeStreetAddress,
                //    PayeeTypeID= payroll.Payee.PayeeTypeID,
                //    PayeeZipcode= payroll.Payee.PayeeZipcode
                //},
                PayeesType = new PayeesTypeModel()
                {
                    PayeeType = payroll.PayeesType.PayeeType,
                    PayeeTypeID = payroll.PayeesType.PayeeTypeID
                },
                PayrollStatu = new PayrollStatusModel()
                {
                    PayrollStatusID = payroll.PayrollStatu.PayrollStatusID,
                    PayrollStatusName = payroll.PayrollStatu.PayrollStatusName,

                },
                PayrollMethod = new PayrollMethodModel()
                {
                    PayrollMethodId = payroll.PayrollMethod.PayrollMethodId,
                    PayrollMethodName = payroll.PayrollMethod.PayrollMethodName,

                },
                PayrollType = new PayrollTypeModel()
                {
                    PayrollTypeID = payroll.PayrollType.PayrollTypeID,
                    PayrollTypeName = payroll.PayrollType.PayrollTypeName,

                },
                Payee = payroll.PayeeTypeID == 2 && payroll.Payee != null ? new PayeeModel()
                {// this is other type of payee that are not employee
                    PayeeBankAccount = payroll.Payee.PayeeBankAccount,
                    PayeeBankName = payroll.Payee.PayeeBankName,
                    PayeeBankRoutingNumber = payroll.Payee.PayeeBankRoutingNumber,
                    PayeeCity = payroll.Payee.PayeeCity,
                    PayeeEmail = payroll.Payee.PayeeEmail,
                    PayeeID = payroll.Payee.PayeeID,
                    PayeeName = payroll.Payee.PayeeName,
                    PayeePhone = payroll.Payee.PayeePhone,
                    PayeeState = payroll.Payee.PayeeState,
                    PayeeStreetAddress = payroll.Payee.PayeeStreetAddress,
                    PayeeTypeID = payroll.Payee.PayeeTypeID,
                    PayeeZipcode = payroll.Payee.PayeeZipcode
                } : payroll.PayeeTypeID == 1 && employee != null ? new PayeeModel()// this is an employee
                {
                    //PayeeBankAccount = payroll.Payee.PayeeBankAccount,
                    //PayeeBankName = payroll.Payee.PayeeBankName,
                    //PayeeBankRoutingNumber = payroll.Payee.PayeeBankRoutingNumber,
                    PayeeCity = employee.City,
                    PayeeEmail = employee.Email,
                    PayeeID = employee.EmployeeID,
                    PayeeName = employee.EmployeeFName + " " + employee.EmployeeMName + " " + employee.EmployeeLName,
                    PayeePhone = employee.PhoneNumber,
                    PayeeState = employee.State,
                    PayeeStreetAddress = employee.StreetAddress,
                    PayeeTypeID = payroll.PayeeTypeID,
                    PayeeZipcode = employee.ZipCode
                } : null,
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
                WirePayrolls = payroll.WirePayrolls?.Select(w => new WirePayrollModel()
                {
                    PayrollID = w.PayrollID,
                    WireNumber = w.WireNumber,
                    WireNumberFormat = w.WireNumberFormat
                }).ToList(),
                CheckPayrolls = payroll.CheckPayrolls?.Select(c => new CheckPayrollModel()
                {
                    CheckNumberFormat = c.CheckNumberFormat,
                    CheckNumber = c.CheckNumber,
                    PayrollID = c.PayrollID
                }).ToList(),
                Agency = new AgencyModel()
                {
                    AgencyID = payroll.Agency != null ? payroll.Agency.AgencyID : 0,
                    AgencyName = payroll.Agency != null ? payroll.Agency.AgencyName : ""//payroll.Agency?.AgencyName,                   
                },
                PayrollModethodList = _domainObjectRepository.All<PayrollMethod>().Select(pm => new PayrollMethodModel()
                {
                    PayrollMethodId = pm.PayrollMethodId,
                    PayrollMethodName = pm.PayrollMethodName
                }).ToList(),
                Agencies = _domainObjectRepository.All<Agency>().Select(a => new AgencyModel()
                {
                    AgencyID = a.AgencyID,
                    AgencyName = a.AgencyName,
                }).ToList(),
                PayrollStatusList = _domainObjectRepository.All<PayrollStatu>().Select(ps => new PayrollStatusModel()
                {
                    PayrollStatusID = ps.PayrollStatusID,
                    PayrollStatusName = ps.PayrollStatusName
                }).ToList()
            };
            pay.CheckPayroll = pay.CheckPayrolls?.SingleOrDefault();
            pay.WirePayroll = pay.WirePayrolls?.SingleOrDefault();
            return pay;
        }

        public AccountModel UpdatePayrollAccount(AccountModel account)
        {
            var updatedAccount = _domainObjectRepository.Get<Account>(a => a.AccountNumber == account.AccountNumber);
            updatedAccount.AccountName = _domainObjectRepository.Get<PayrollAccount>(ac => ac.PayAccountID == account.PayrollAccountID)?.Description;
            updatedAccount.Amount = account.Amount;
            updatedAccount.Descriptions = account.Descriptions;
            updatedAccount.PayrollAccountID = account.PayrollAccountID;
            updatedAccount.Discount = account.Discount;
            updatedAccount.DiscountTypeId = account.DiscountTypeId;
            updatedAccount.AmountAfterDiscount = account.Amount - account.Discount;

            _domainObjectRepository.Update<Account>(updatedAccount);

            // we  need to update the payroll amount
            decimal? accumulatedAmount = 0;
            var accounts = _domainObjectRepository.All<Account>().Where(p => p.PayrollID == updatedAccount.PayrollID).ToList();
            foreach (var ac in accounts)
            {
                if (ac.DiscountTypeId == 1)// percentage accumulatedAmount += (ac.AmountAfterDiscount ?? 0);
                {
                    ac.AmountAfterDiscount = ac.Amount - ((ac.Amount * ac.Discount) / 100);
                    accumulatedAmount += (ac.AmountAfterDiscount ?? 0);
                }
                else if (ac.DiscountTypeId == 2)// dollar value
                {
                    ac.AmountAfterDiscount = (ac.Amount - ac.Discount);
                    accumulatedAmount += ac.AmountAfterDiscount ?? 0;
                }
                else
                {
                    accumulatedAmount += ac.Amount ?? 0;
                }
                //amount += ac.Amount - ac.Discount;
            }
            var payroll = _domainObjectRepository.Get<Payroll>(pa => pa.TransactionID == account.PayrollID);
            payroll.Amount = accumulatedAmount ?? payroll.Amount;
            _domainObjectRepository.Update<Payroll>(payroll);
            account.TotalAmount = payroll.Amount;
            return account;
        }
        public void DeletePayrollAccount(int accountId)
        {
            var account = _domainObjectRepository.Get<Account>(a => a.AccountNumber == accountId);
            if (account != null)
            {
                var payroll = _domainObjectRepository.Get<Payroll>(pa => pa.TransactionID == account.PayrollID);
                payroll.Amount -= account.Amount ?? 0;
                payroll.AccountNumber = null;
                _domainObjectRepository.Update<Payroll>(payroll);
                _domainObjectRepository.Delete<Account>(account);
            }
        }
        public List<AccountModel> GetPayrollAccountList(int payrollId)
        {
            var payrollAccounts = _domainObjectRepository.All<Account>().Where(p => p.PayrollID == payrollId).ToList();
            return payrollAccounts.Select(a => new AccountModel()
            {
                AccountName = a.AccountName,
                AccountNumber = a.AccountNumber,
                Amount = a.Amount,
                Descriptions = a.Descriptions,
                Discount = a.Discount,
                PayrollAccountID = a.PayrollAccountID,
                PayrollID = a.PayrollID,
                DiscountTypeId = a.DiscountTypeId,
                AmountAfterDiscount = a.AmountAfterDiscount,
                DiscountTypeName = _domainObjectRepository.Get<DiscountType>(d => d.DiscountTypeId == a.DiscountTypeId)?.DiscountType1,
                DiscountTypes = _domainObjectRepository.All<DiscountType>().Select(d => new DiscountTypeModel()
                {
                    DiscountType1 = d.DiscountType1,
                    DiscountTypeId = d.DiscountTypeId
                }).ToList()
            }).ToList();
        }
        public PayrollModel NewPayroll(PayrollModel p)
        {
            // create an account in account table
            //var account = new Account()
            //{
            //    AccountName = p.Account.AccountName,
            //    AccountNumber = p.Account.AccountNumber,
            //    Amount = p.Account.Amount,
            //    Descriptions = p.Account.Descriptions,
            //    Discount = p.Account.Discount,
            //    PayrollID=p.TransactionID,
            //};
            // var createdAccount = _domainObjectRepository.Create<Account>(account);
            //var wirePayrollMethodId = _domainObjectRepository.Get<PayrollMethod>(p => p.PayrollMethodName == "Wire").PayrollMethodId;
            var payrollStatusID = _domainObjectRepository.Get<PayrollStatu>(pa => pa.PayrollStatusName == "Entered").PayrollStatusID;
            var payrollTypeID = _domainObjectRepository.Get<PayrollType>(pa => pa.PayrollTypeName == "Sallary").PayrollTypeID;
            var payeeTypeID = _domainObjectRepository.Get<PayeesType>(pa => pa.PayeeType == "Others").PayeeTypeID;
            decimal accumulatedAmount = 0;
            foreach (var ac in p.Accounts)
            {
                ac.AccountName = _domainObjectRepository.Get<PayrollAccount>(a => a.PayAccountID == ac.PayrollAccountID).Description;
                if (ac.DiscountTypeId == 1)// percentage
                {
                    ac.AmountAfterDiscount = ac.Amount - ((ac.Amount * ac.Discount) / 100);
                    accumulatedAmount += (ac.AmountAfterDiscount ?? 0);
                }
                else if (ac.DiscountTypeId == 2)// dollar value
                {
                    ac.AmountAfterDiscount = (ac.Amount - ac.Discount);
                    accumulatedAmount += ac.AmountAfterDiscount ?? 0;
                }
                else
                {
                    accumulatedAmount += ac.Amount ?? 0;
                }

            }
            var newPay = new Payroll()
            {
                Amount = accumulatedAmount,// required                
                Descriptions = p.Descriptions,
                PaymentCreatedBy = p.PaymentCreatedBy,// required
                PaymentCreatedDate = DateTime.Now,// required
                PaymentDueDate = p.PaymentDueDate,// required
                PaymentEnteredBy = p.PaymentEnteredBy,// required
                PaymentEnteredDate = DateTime.Now,// required
                PayrollMethodId = p.PayrollMethodId,// required
                PayrollStatusID = payrollStatusID,// required
                PayrollTypeID = payrollTypeID,// required
                PayeeTypeID = payeeTypeID,// required
                PayeeName = p.PayeeName,// required
                PayeeID = p.PayeeID,
                AgencyID = p.AgencyID,
                EmployeeID = p.EmployeeID,
                StartDate = p.StartDate,
                EndDate = p.EndDate,

                //AccountNumber = createdAccount.AccountNumber,
                //Accounts=p.Accounts.Select(ac=> new Account() {
                //    AccountName = ac.AccountName,
                //    AccountNumber = ac.AccountNumber,
                //    Amount = ac.Amount,
                //    Descriptions = ac.Descriptions,
                //    Discount = ac.Discount,
                //    PayrollID = p.TransactionID,
                //}).ToList()
            };
           var createdPayroll= _domainObjectRepository.Create<Payroll>(newPay);

            //_depositRepository.CreateBudgetTransactions(null, createdPayroll);
            newPay.Accounts = p.Accounts.Select(ac => new Account()
            {
                AccountName = ac.AccountName,
                AccountNumber = ac.AccountNumber,
                Amount = ac.Amount,
                Descriptions = ac.Descriptions,
                Discount = ac.Discount,
                PayrollID = newPay.TransactionID,
                DiscountTypeId = ac.DiscountTypeId,
                AmountAfterDiscount = ac.AmountAfterDiscount,
                PayrollAccountID = ac.PayrollAccountID,

            }).ToList();
          _domainObjectRepository.CreateBulk<Account>(newPay.Accounts.ToList());
            //throw new NotImplementedException();
            
            return p;
        }

        public PayrollModel GetPayrollObject()
        {
            var PayrollModel = new PayrollModel()
            {
                Account = new AccountModel()
                {
                    PayrollAccounts = _domainObjectRepository.All<PayrollAccount>().Select(a => new PayrollAccountModel()
                    {
                        AccountType = a.AccountType,
                        Description = a.Description,
                        Grouping = a.Grouping,
                        Parent = a.Parent,
                        PayAccountID = a.PayAccountID,
                        Title = a.Title
                    }).ToList(),
                    DiscountTypes = _domainObjectRepository.All<DiscountType>().Select(a => new DiscountTypeModel()
                    {

                        DiscountType1 = a.DiscountType1,
                        DiscountTypeId = a.DiscountTypeId
                    }).ToList()
                },
                Accounts = new List<AccountModel>() {
                    new AccountModel()
                {
                    PayrollAccounts = _domainObjectRepository.All<PayrollAccount>().Select(a => new PayrollAccountModel()
                    {
                        AccountType = a.AccountType,
                        Description = a.Description,
                        Grouping = a.Grouping,
                        Parent = a.Parent,
                        PayAccountID = a.PayAccountID,
                        Title = a.Title
                    }).ToList(),
                    DiscountTypes = _domainObjectRepository.All<DiscountType>().Select(a =>new DiscountTypeModel() {

                        DiscountType1=a.DiscountType1,
                        DiscountTypeId=a.DiscountTypeId
                    }).ToList()
                },

                },
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
                EmployeeList = _domainObjectRepository.All<Employee>().Select(em=> new EmployeeModel() {
                    EmployeeFName=em.EmployeeFName,
                    EmployeeMName = em.EmployeeMName,
                    EmployeeLName=em.EmployeeLName,
                    EmployeeID=em.EmployeeID
                }).ToList(),
                Agencies = _domainObjectRepository.All<Agency>().Select(a => new AgencyModel()
                {
                    AgencyID = a.AgencyID,
                    AgencyName = a.AgencyName,
                }).ToList(),
                PayrollModethodList = _domainObjectRepository.All<PayrollMethod>().Select(pm => new PayrollMethodModel()
                {
                    PayrollMethodId = pm.PayrollMethodId,
                    PayrollMethodName = pm.PayrollMethodName
                }).ToList(),
                PayrollStatusList = _domainObjectRepository.All<PayrollStatu>().Select(ps => new PayrollStatusModel()
                {
                   PayrollStatusID=ps.PayrollStatusID,
                   PayrollStatusName=ps.PayrollStatusName
                }).ToList(),
            };

            return PayrollModel;
        }
        public void UpdatePayroll(PayrollModel payroll)
        {

            var p = _domainObjectRepository.Get<Payroll>(pa => pa.TransactionID == payroll.TransactionID, new string[] { "CheckPayrolls", "WirePayrolls", "PayrollType", "Employee", "Account", "Agency", "Payee", "PayeesType", "PayrollMethod", "PayrollStatu" });
            if (p != null)
            {

                p.PaymentDueDate = payroll.PaymentDueDate;
                p.StartDate = payroll.StartDate;
                p.EndDate = payroll.EndDate;
                p.Descriptions = payroll.Descriptions;
                p.PayeeID = payroll.PayeeID;

                //if payroll status is paid or reconciled, then we should update the check or wire table accordingly
                if (p.PayrollStatusID >= 4)
                    UpdateCheckOrWirePayroll(payroll, p);
                p.PayrollMethodId = payroll.PayrollMethodId;

                UpdatePayrollStatus(payroll,p);
               // p.PayrollStatusID = payroll.PayrollStatu.PayrollStatusID;
                p.PayeeName = payroll.PayeeName;
                p.AgencyID = payroll.AgencyID;
                p.PaymentModifiedBy = payroll.PaymentModifiedBy;
                p.PaymentModifiedDate = payroll.PaymentModifiedDate;

                _domainObjectRepository.Update<Payroll>(p);
            }
            //throw new NotImplementedException();
        }

        private void UpdateCheckOrWirePayroll(PayrollModel payroll, Payroll p)
        {
            if (p.PayrollMethod.PayrollMethodId != payroll.PayrollMethodId)
            {

                if (p.PayrollMethod.PayrollMethodId == 1)// wire for old payroll before the updates,
                {
                    // we delete this insert into from the table
                    var wirePayroll = _domainObjectRepository.Get<WirePayroll>(w => w.PayrollID == p.TransactionID);
                    if (wirePayroll != null)
                    {
                        _domainObjectRepository.Delete<WirePayroll>(wirePayroll);
                        ReseedIdentityColumnInTable("WirePayrolls", "WireNumber");

                        var updatepayroll = CreatePayrollMethod(payroll);
                        payroll.WireNumber = null;
                        p.WireNumber = null;
                        payroll.CheckNumber = updatepayroll.CheckNumber;
                        p.CheckNumber = updatepayroll.CheckNumber;
                    }

                }
                else if (p.PayrollMethod.PayrollMethodId == 2)// check for old payroll before the updates,
                {
                    // we delete this insert into from the table
                    var checkPayroll = _domainObjectRepository.Get<CheckPayroll>(w => w.PayrollID == p.TransactionID);
                    if (checkPayroll != null)
                    {
                        _domainObjectRepository.Delete<CheckPayroll>(checkPayroll);
                        ReseedIdentityColumnInTable("CheckPayrolls", "CheckNumber");
                        var updatepayroll = CreatePayrollMethod(payroll);
                        payroll.CheckNumber = null;
                        p.CheckNumber = null;
                        payroll.WireNumber = updatepayroll.WireNumber;
                        p.WireNumber = updatepayroll.WireNumber;
                    }

                }

            }
        }
        private void ReseedIdentityColumnInTable(string tableName, string IdentityColumn)
        {
            var parms = new SqlParameter[] {
                new SqlParameter {ParameterName= "@TableName",Value=tableName,Direction=ParameterDirection.Input },
                new SqlParameter { ParameterName= "@IdentityColumn",Value=IdentityColumn,Direction=ParameterDirection.Input },
            };
            _domainObjectRepository.ExecuteProcedure("ReseedIdentity_SP @TableName, @IdentityColumn", parms);
        }

        private void UpdatePayrollStatus(PayrollModel payroll, Payroll pay)
        {
            var currentStatusId = pay.PayrollStatusID;
            var newStatusId = payroll.PayrollStatu.PayrollStatusID;
            if (newStatusId != currentStatusId)
                if (newStatusId <= 3 && !(currentStatusId==4 || currentStatusId==5))// entered, approved, autorized
                {
                    switch (newStatusId)
                    {
                        case 1:// entered
                            pay.PayrollStatusID = newStatusId;// entered
                            pay.PaymentEnteredBy = payroll.PaymentModifiedBy;
                            pay.PaymentEnteredDate = DateTime.Now;
                            pay.PaymentApprovedBy = null;
                            pay.PaymentApprovedDate = null;
                            pay.PaymentAuthorizedBy = null;
                            pay.PaymentAuthorizedDate = null;
                            break;
                        case 2:
                            pay.PayrollStatusID = newStatusId;// approved
                            pay.PaymentApprovedBy = payroll.PaymentModifiedBy;
                            pay.PaymentApprovedDate = DateTime.Now;
                            pay.PaymentEnteredBy = payroll.PaymentEnteredBy ?? payroll.PaymentModifiedBy;
                            pay.PaymentEnteredDate = payroll.PaymentEnteredDate;
                            pay.PaymentAuthorizedBy = null;
                            pay.PaymentAuthorizedDate = null;
                            break;
                        case 3:
                            pay.PayrollStatusID = newStatusId;// Authorized
                            pay.PaymentAuthorizedBy = payroll.PaymentModifiedBy;
                            pay.PaymentAuthorizedDate = DateTime.Now;
                            pay.PaymentApprovedBy = payroll.PaymentEnteredBy ?? payroll.PaymentModifiedBy;
                            pay.PaymentApprovedDate = payroll.PaymentApprovedDate?? DateTime.Now;
                            pay.PaymentEnteredBy = payroll.PaymentEnteredBy ?? payroll.PaymentModifiedBy;
                            pay.PaymentEnteredDate = payroll.PaymentEnteredDate;
                            break;
                    }
                }

        }


        public List<PayrollReportModel> GetPayrollReport(DateTime? startDate = null,DateTime? endDate = null,
                                int? transactionId = null, int? employeeId = null, int? PayrollMethodId = null, int? payeeId = null,
                                int? payrollStatus =null, int? reportType = null)
        {
            /*
             @startDate Date =null,
		     @endDate Date = null,
		     @transactionId int =null,
		     @employeeId int = null,
		     @PayrollMethodId int = null,
		     @payeeId int = null
             */

            Dictionary<string, object> parms = new Dictionary<string, object>();            
            parms.Add("startDate", startDate);
            parms.Add("endDate", endDate);
            parms.Add("transactionId", transactionId);
            parms.Add("employeeId", employeeId);
            parms.Add("PayrollMethodId", PayrollMethodId);
            parms.Add("payeeId", payeeId);
            parms.Add("payrollStatus", payrollStatus);
            parms.Add("reportType", reportType);


            //pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter
            var report = _domainObjectRepository.ExecuteProcedure<PayrollReportModel>("GetPayrollReport_SP", parms, false);
            return report;
        }
        //
        public List<PayrollReportModel> GetPayrollReportWithoutParms()
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("startDate", null);
            parms.Add("endDate", null);
            parms.Add("transactionId", null);
            parms.Add("employeeId", null);
            parms.Add("PayrollMethodId", null);
            parms.Add("payeeId", null);
            parms.Add("reportType", null);
            //pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter
            var report = _domainObjectRepository.ExecuteProcedure<PayrollReportModel>("GetPayrollReport_SP", parms, false);
            return report;


        }
    }
}
