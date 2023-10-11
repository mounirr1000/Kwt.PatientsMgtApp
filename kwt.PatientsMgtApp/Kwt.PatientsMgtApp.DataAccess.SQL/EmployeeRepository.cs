using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System.Transactions;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

        public EmployeeRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }

        #region Employee Model CRUD Operation



        private void AddBonus(EmployeeModel emp, int employeeId) {
            Bonus bonus = new Bonus()
            {
                EmployeeID = employeeId,//employeeId.EmployeeID,
                BasedSalary = emp.Salary.Salary1,
                BonusTypeID = emp.Bonus?.BonusTypeID,
                BonusValue = emp.Bonus?.BonusValue,
                BonusFinalAmount =  emp.Bonus?.BonusFinalAmount,
                PayAccountID = emp.Bonus.BonusPayAccountID,
                                  // BonuseType =                         
            };
            var createdBonus = _domainObjectRepository.Create<Bonus>(bonus);
        }
        private void AddSalary(EmployeeModel emp, int employeeId) {
            var experienceSalary = CalculateSalary(emp, false, false);
            var experienceWithInsurance = CalculateSalary(emp, false, true);
            var experienceWithTax = CalculateSalary(emp, true, false);
            var totalSalary = CalculateSalary(emp);
            // add salary for this employee
            Salary salary = new Salary()
            {
                EmployeeID = employeeId,//employee.EmployeeID,
                FromDate = emp.Salary.FromDate,
                Salary1 = totalSalary ?? emp.Salary.Salary1,
                ExperienceSalary = experienceSalary,
                ExperienceWithInsuranceSalary = experienceWithInsurance,
                ExperienceWithTaxSalary = experienceWithTax,
                TotalSalary = totalSalary,
                ToDate = emp.Salary.ToDate,
                StartingSalary = emp.Salary.StartingSalary,
                TaxCategoryID = emp.Salary.TaxCategoryID,
                YearlySalaryIncrement = _domainObjectRepository.Get<TitleType>(t => t.TitleTypeId == emp.Title.TitleTypeId)?.TitleYearlyIncrease,// emp.Salary.YearlySalaryIncrement,
                PayAccountID = emp.Salary.SalaryPayAccountID,

            };
            var createdSalary = _domainObjectRepository.Create<Salary>(salary);
        }
        private void AddInsurances(EmployeeModel emp, int employeeId) {
            foreach (var e in emp.EmployeeInsurances)
            {
                EmployeeInsurance insurance = new EmployeeInsurance()
                {
                    EmployeeID = employeeId,// employee.EmployeeID,
                    BasedSalary = emp.Salary.Salary1,
                    InsuranceAmount = e.InsuranceAmount,
                    InsuranceOptionID = e.InsuranceOptionID,
                    InsuranceTypeID = e.InsuranceTypeID,
                    PayAccountID = emp.Insurance.InsurancePayAccountID
                };
                var createdInsurance = _domainObjectRepository.Create<EmployeeInsurance>(insurance);
            }   
        }
        private void AddOvertime(EmployeeModel emp, int employeeId) {
            Overtime overtime = new Overtime()
            {
                EmployeeID = employeeId,// employee.EmployeeID,
                BasedSalary = emp.Salary.Salary1,
                CalculatedOverTimeAmount = emp.Overtime.CalculatedOverTimeAmount,
                HourlyRate = emp.Overtime.HourlyRate,
                OvertimeHours = emp.Overtime.OvertimeHours,
                RegularHours = emp.Overtime.RegularHours,
                PayAccountID = emp.Overtime.OvertimePayAccountID,
            };
            var createdOvertime = _domainObjectRepository.Create<Overtime>(overtime);
           
        }
        private void AddTitle(EmployeeModel emp, int employeeId, Employee employee) {
            var titleName = _domainObjectRepository.Get<TitleType>(t => t.TitleTypeId == emp.Title.TitleTypeId)?.TitleTypeName;
            Title title = new Title()
            {
                EmployeeID = employeeId,// employee.EmployeeID,
                FromDate = employee.HireDate ?? DateTime.Today,
                TitleTypeId = emp.Title.TitleTypeId,
                Title1 = emp.Title.Title1 ?? titleName,

            };
            var createdTitle = _domainObjectRepository.Create<Title>(title);
            
        }
        private void AddAccount(EmployeeModel emp, int employeeId) {
           
            EmployeeAccountType account = new EmployeeAccountType()
            {
                EmployeeID = employeeId,// employee.EmployeeID,
                AgencyAccountID = emp.Account.AgencyAccountID,
                SalaryAccountID = emp.Account.SalaryAccountID,
                OvertimeAccountID = emp.Account.OvertimeAccountID,
                BonusAccountID = emp.Account.BonusAccountID,
                InsuranceAccountID = emp.Account.InsuranceAccountID,
            };
            _domainObjectRepository.Create<EmployeeAccountType>(account);
        }
        public void AddEmployee(EmployeeModel emp)
        {

            //throw new NotImplementedException();
            if (emp != null)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        
                        Employee newEmplyee = new Employee()
                        {
                            City = emp.City,
                            DateOfBirth = emp.DateOfBirth,
                            Education = emp.Education,
                            Email = emp.Email,
                            EmployeeFName = emp.EmployeeFName,
                            EmployeeLName = emp.EmployeeLName,
                            EmployeeMName = emp.EmployeeMName,
                            Employeephone = emp.Employeephone,
                            EndDate = emp.EndDate,
                            GENDER = emp.GENDER,
                            ImmigrationStatus = emp.ImmigrationStatus,
                            Nationality = emp.Nationality,
                            Notes = emp.Notes,
                            PhoneNumber = emp.PhoneNumber,
                            Qualification = emp.Qualification,
                            SocialStatus = emp.SocialStatus,
                            Photograph = emp.Photograph,
                            State = emp.State,
                            StreetAddress = emp.StreetAddress,
                            Training = emp.Training,
                            ZipCode = emp.ZipCode,
                            HireDate = emp.HireDate,
                            SocialStatusID = emp.SocialStatusID,

                        };
                        var employee = _domainObjectRepository.Create<Employee>(newEmplyee);
                        var employeeId = employee.EmployeeID;
                        AddBonus( emp,  employeeId);
                        AddSalary( emp,  employeeId);
                        AddInsurances( emp,  employeeId);
                        AddOvertime( emp,  employeeId);
                        AddTitle( emp,  employeeId,  employee);
                   //     AddAccount( emp,  employeeId);
                        scope.Complete();
                    }
                    finally
                    {
                        scope.Dispose();

                    }
                }
            }


        }

        public EmployeeModel GetEmployeeObject()
        {
            var employee = new EmployeeModel();
            var taxCategories = _domainObjectRepository.All<TaxCategory>();
            var bonustypes = _domainObjectRepository.All<BonuseType>();
            var InsuranceTypes = _domainObjectRepository.All<InsuranceType>();
            var InsuranceOptions = _domainObjectRepository.All<InsuranceOption>();
            var PayrollAccounts = _domainObjectRepository.All<PayrollAccount>();
            var SocialStauses = _domainObjectRepository.All<SocialStatus>();
            var TitleTypes = _domainObjectRepository.All<TitleType>();
            var taxes = taxCategories.Select(t => new TaxCategoryModel()
            {
                TaxCategory1 = t.TaxCategory1,
                TaxCategoryID = t.TaxCategoryID,
                TaxCategoryValue = t.TaxCategoryValue
            }).OrderBy(i => i.TaxCategory1).ToList();
            var bonusType = bonustypes.Select(b => new BonusTypeModel()
            {
                BonusType = b.BonusType,
                BonusTypeID = b.BonusTypeID
            }).OrderBy(i => i.BonusType).ToList();
            var InsurancesTypes = InsuranceTypes.Select(b => new InsuranceTypeModel()
            {
                InsuranceType1 = b.InsuranceType1,
                InsuranceTypeID = b.InsuranceTypeID
            }).OrderBy(i => i.InsuranceType1).ToList();
            var InsurancesOptions = InsuranceOptions.Select(b => new InsuranceOptionModel()
            {
                InsuranceOption1 = b.InsuranceOption1,
                InsuranceOptionID = b.InsuranceOptionID
            }).OrderBy(i => i.InsuranceOption1).ToList();
            var employeeInsurances = new List<EmployeeInsuranceModel>();
            employeeInsurances.Add(new EmployeeInsuranceModel()
            {

            });
            employeeInsurances.Add(new EmployeeInsuranceModel()
            {

            });
            var payAccounts = PayrollAccounts.Select(pa => new PayrollAccountModel()
            {
                PayAccountID = pa.PayAccountID,
                Description = pa.Description,
                Grouping = pa.Grouping,
                Parent = pa.Parent,
                Title = pa.Title
            }).OrderBy(i => i.Description).ToList();
            var socialStatus = SocialStauses.Select(ss => new SocialStatusModel()
            {
                SocialStatusID = ss.SocialStatusID,
                SocialStatusName = ss.SocialStatusName
            }).OrderByDescending(i => i.SocialStatusName).ToList();
            var titleTypes = TitleTypes.Select(t => new TitleTypesModel()
            {
                TitleTypeId = t.TitleTypeId,
                TitleTypeName = t.TitleTypeName,
                TitleYearlyIncrease = t.TitleYearlyIncrease
            }).OrderBy(i => i.TitleTypeName).ToList();

            employee.EmployeeInsurances = employeeInsurances;
            employee.TaxCategories = taxes;
            employee.BonusTypes = bonusType;
            employee.InsuranceOptions = InsurancesOptions;
            employee.InsuranceTypes = InsurancesTypes;
            employee.PayrollAccounts = payAccounts;
            employee.SocialStatuses = socialStatus;
            employee.TitleTypes = titleTypes;
            employee.Deduction = new SalaryDeductionModel();
            return employee;
        }

        public int DeleteEmployee(EmployeeModel employee)
        {
            throw new NotImplementedException();
        }

        public EmployeeModel GetEmployee(int employeeId)
        {
            var e = _domainObjectRepository.Get<Employee>(em => em.EmployeeID == employeeId, new string[] { "Payrolls", "DepartmentEmployees", "DepartmentManagers", "Salaries", "Bonuses", "EmployeeInsurances", "Overtimes", "SalaryDeductions", "EmployeeAccountTypes", "Titles" });
            var healthInsurance = e.EmployeeInsurances.ToList().Find(hi => hi.InsuranceTypeID == 1);
            var dentalInsurance = e.EmployeeInsurances.ToList().Find(hi => hi.InsuranceTypeID == 2);
            var otherInsurance = e.EmployeeInsurances.ToList().Find(hi => hi.InsuranceTypeID == 3);
            return new EmployeeModel()
            {
                City = e.City,
                DateOfBirth = e.DateOfBirth,
                Education = e.Education,
                Email = e.Email,
                EmployeeFName = e.EmployeeFName,
                EmployeeID = e.EmployeeID,
                EmployeeLName = e.EmployeeLName,
                EmployeeMName = e.EmployeeMName,
                Employeephone = e.Employeephone,
                EndDate = e.EndDate,
                GENDER = e.GENDER,
                HireDate = e.HireDate,
                ImmigrationStatus = e.ImmigrationStatus,
                Nationality = e.Nationality,
                Notes = e.Notes,
                PhoneNumber = e.PhoneNumber,
                Photograph = e.Photograph,
                Qualification = e.Qualification,
                SocialStatus = e.SocialStatus,
                State = e.State,
                StreetAddress = e.StreetAddress,
                Training = e.Training,
                ZipCode = e.ZipCode,
                SocialStatusID = e.SocialStatusID,

                Title = e.Titles?.Select(t => new TitleModel()
                {
                    EmployeeID = t.EmployeeID,
                    FromDate = t.FromDate,
                    Title1 = t.Title1,
                    TitleID = t.TitleID,
                    TitleTypeId = t.TitleTypeId,
                    ToDate = t.ToDate

                }).FirstOrDefault(),
                Bonus = e.Bonuses?.Select(b => new BonusModel()
                {
                    BasedSalary = b.BasedSalary,
                    BonusFinalAmount = b.BonusFinalAmount,
                    BonusID = b.BonusID,
                    BonusTypeID = b.BonusTypeID,
                    BonusValue = b.BonusValue,
                    EmployeeID = b.EmployeeID,
                    BonusPayAccountID = b.PayAccountID
                }).FirstOrDefault(),
                EmployeeInsurances = e.EmployeeInsurances?.Select(i => new EmployeeInsuranceModel()
                {
                    BasedSalary = i.BasedSalary,
                    EmployeeID = i.EmployeeID,
                    InsuranceAmount = i.InsuranceAmount,
                    InsuranceID = i.InsuranceID,
                    InsuranceOptionID = i.InsuranceOptionID,
                    InsurancePayAccountID = i.PayAccountID,
                    InsuranceTypeID = i.InsuranceTypeID
                }).ToList(),
                Insurance = healthInsurance != null ? new EmployeeInsuranceModel()
                {
                    BasedSalary = healthInsurance.BasedSalary,
                    EmployeeID = healthInsurance.EmployeeID,
                    InsurancePayAccountID = healthInsurance.PayAccountID,

                } : null,
                HealthInsurance = dentalInsurance != null ? new EmployeeInsuranceModel()
                {
                    BasedSalary = dentalInsurance.BasedSalary,
                    EmployeeID = dentalInsurance.EmployeeID,
                    InsuranceAmount = dentalInsurance.InsuranceAmount,
                    InsuranceID = dentalInsurance.InsuranceID,
                    InsuranceOptionID = dentalInsurance.InsuranceOptionID,
                    InsurancePayAccountID = dentalInsurance.PayAccountID,
                    InsuranceTypeID = dentalInsurance.InsuranceTypeID
                } : null,
                DentalInsurance = healthInsurance != null ? new EmployeeInsuranceModel()
                {
                    BasedSalary = healthInsurance.BasedSalary,
                    EmployeeID = healthInsurance.EmployeeID,
                    InsuranceAmount = healthInsurance.InsuranceAmount,
                    InsuranceID = healthInsurance.InsuranceID,
                    InsuranceOptionID = healthInsurance.InsuranceOptionID,
                    InsurancePayAccountID = healthInsurance.PayAccountID,
                    InsuranceTypeID = healthInsurance.InsuranceTypeID
                } : null,
                Overtime = e.Overtimes?.Select(o => new OvertimeModel()
                {
                    BasedSalary = o.BasedSalary,
                    CalculatedOverTimeAmount = o.CalculatedOverTimeAmount,
                    EmployeeID = o.EmployeeID,
                    HourlyRate = o.HourlyRate,
                    OvertimeHours = o.OvertimeHours,
                    OvertimeID = o.OvertimeID,
                    OvertimePayAccountID = o.PayAccountID,
                    RegularHours = o.RegularHours
                }).FirstOrDefault(),
                Salary = e.Salaries?.Select(s => new SalaryModel()
                {
                    EmployeeID = s.EmployeeID,
                    FromDate = s.FromDate,
                    Salary1 = Math.Round(s.Salary1 ?? 0, 2),//s.Salary1,
                    SalaryPayAccountID = s.PayAccountID,
                    StartingSalary = Math.Round(s.StartingSalary, 2),// s.StartingSalary,
                    TaxCategoryID = s.TaxCategoryID,
                    ToDate = s.ToDate,
                    YearlySalaryIncrement = Math.Round(s.YearlySalaryIncrement ?? 0, 2),
                    ExperienceSalary = s.ExperienceSalary,
                    TotalSalary = s.TotalSalary,
                    ExperienceWithTaxSalary = s.ExperienceWithTaxSalary,
                    ExperienceWithInsuranceSalary = s.ExperienceWithInsuranceSalary,
                    SalaryID = s.SalaryID
                }).FirstOrDefault(),
                Deduction = //new SalaryDeductionModel(),
                        e.SalaryDeductions.Count>0 ? e.SalaryDeductions.Select(d => new SalaryDeductionModel()
                        {
                            BasedSalary = d.BasedSalary,
                            DeductedDays = d.DeductedDays,
                            DeductedHours = d.DeductedHours,
                            DeductionAmount = d.DeductionAmount,
                            DeductionID = d.DeductionID,
                            DeductionPercentage = d.DeductionPercentage,
                            DeductionType= d.DeductedDays!=null?"Days": d.DeductedHours!=null?"Hours":null
                        }).ToList().FirstOrDefault() : new SalaryDeductionModel()
                        {

                        }
            };
        }

        public List<EmployeeModel> GetEmployees()
        {
            var employees = _domainObjectRepository.All<Employee>(new string[] { "Payrolls", "DepartmentEmployees", "DepartmentManagers",
                                                                                 "Salaries", "Bonuses", "EmployeeInsurances",
                                                                                 "Overtimes", "SalaryDeductions", "EmployeeAccountTypes", "Titles" });

            return employees.Select(e => new EmployeeModel()
            {
                City = e.City,
                DateOfBirth = e.DateOfBirth,
                Education = e.Education,
                Email = e.Email,
                EmployeeFName = e.EmployeeFName,
                EmployeeID = e.EmployeeID,
                EmployeeLName = e.EmployeeLName,
                EmployeeMName = e.EmployeeMName,
                Employeephone = e.Employeephone,
                EndDate = e.EndDate,
                GENDER = e.GENDER,
                HireDate = e.HireDate,
                ImmigrationStatus = e.ImmigrationStatus,
                Nationality = e.Nationality,
                Notes = e.Notes,
                PhoneNumber = e.PhoneNumber,
                Photograph = e.Photograph,
                Qualification = e.Qualification,
                SocialStatus = e.SocialStatus,
                State = e.State,
                StreetAddress = e.StreetAddress,
                Training = e.Training,
                ZipCode = e.ZipCode,

                Title = e.Titles.Select(t => new TitleModel()
                {
                    EmployeeID = t.EmployeeID,
                    FromDate = t.FromDate,
                    Title1 = t.Title1,
                    TitleID = t.TitleID,
                    TitleTypeId = t.TitleTypeId,
                    ToDate = t.ToDate

                }).FirstOrDefault(),
                Bonus = e.Bonuses.Select(b => new BonusModel()
                {
                    BasedSalary = b.BasedSalary,
                    BonusFinalAmount = b.BonusFinalAmount,
                    BonusID = b.BonusID,
                    BonusTypeID = b.BonusTypeID,
                    BonusValue = b.BonusValue,
                    EmployeeID = b.EmployeeID,
                    BonusPayAccountID = b.PayAccountID
                }).FirstOrDefault(),
                EmployeeInsurances = e.EmployeeInsurances.Select(i => new EmployeeInsuranceModel()
                {
                    BasedSalary = i.BasedSalary,
                    EmployeeID = i.EmployeeID,
                    InsuranceAmount = i.InsuranceAmount,
                    InsuranceID = i.InsuranceID,
                    InsuranceOptionID = i.InsuranceOptionID,
                    InsurancePayAccountID = i.PayAccountID,
                    InsuranceTypeID = i.InsuranceTypeID
                }).ToList(),
                HealthInsurance = e.EmployeeInsurances.Select(i => new EmployeeInsuranceModel()
                {
                    BasedSalary = i.BasedSalary,
                    EmployeeID = i.EmployeeID,
                    InsuranceAmount = i.InsuranceAmount,
                    InsuranceID = i.InsuranceID,
                    InsuranceOptionID = i.InsuranceOptionID,
                    InsurancePayAccountID = i.PayAccountID,
                    InsuranceTypeID = i.InsuranceTypeID
                }).ToList().FirstOrDefault(i => i.InsuranceTypeID == 1),
                Overtime = e.Overtimes.Select(o => new OvertimeModel()
                {
                    BasedSalary = o.BasedSalary,
                    CalculatedOverTimeAmount = o.CalculatedOverTimeAmount,
                    EmployeeID = o.EmployeeID,
                    HourlyRate = o.HourlyRate,
                    OvertimeHours = o.OvertimeHours,
                    OvertimeID = o.OvertimeID,
                    OvertimePayAccountID = o.PayAccountID,
                    RegularHours = o.RegularHours
                }).FirstOrDefault(),
                Salary = e.Salaries.Select(s => new SalaryModel()
                {
                    EmployeeID = s.EmployeeID,
                    FromDate = s.FromDate,
                    Salary1 = s.Salary1 ?? s.StartingSalary,
                    SalaryPayAccountID = s.PayAccountID,
                    StartingSalary = s.StartingSalary,
                    TaxCategoryID = s.TaxCategoryID,
                    ToDate = s.ToDate,
                    YearlySalaryIncrement = s.YearlySalaryIncrement,
                    ExperienceSalary = s.ExperienceSalary,
                    TotalSalary = s.TotalSalary,
                    ExperienceWithTaxSalary = s.ExperienceWithTaxSalary,
                    ExperienceWithInsuranceSalary = s.ExperienceWithInsuranceSalary,
                    SalaryID = s.SalaryID
                }).FirstOrDefault(),
                //Accounts = new List<AccountModel>()
                //{
                //    new AccountModel()
                //    {

                //    }
                //}
            }).ToList();
        }

        private void UpdateSalary(EmployeeModel emp, Employee dbEmp)
        {
            //test caculating salary based on experience

            var experienceSalary = CalculateSalary(emp, false, false);
            var experienceWithInsurance = CalculateSalary(emp, false, true);
            var experienceWithTax = CalculateSalary(emp, true, false);
            var totalSalary = CalculateSalary(emp);
            foreach (var s in dbEmp.Salaries)
            {
                // EmployeeID = dbEmp.EmployeeID,
                s.FromDate = emp.Salary.FromDate;
                s.Salary1 = totalSalary ?? emp.Salary.Salary1;
                s.ExperienceSalary = experienceSalary;
                s.ExperienceWithInsuranceSalary = experienceWithInsurance;
                s.ExperienceWithTaxSalary = experienceWithTax;
                s.TotalSalary = totalSalary;
                s.ToDate = emp.Salary.ToDate;
                s.StartingSalary = emp.Salary.StartingSalary;
                s.TaxCategoryID = emp.Salary.TaxCategoryID;
                s.YearlySalaryIncrement = _domainObjectRepository.Get<TitleType>(t => t.TitleTypeId == emp.Title.TitleTypeId)?.TitleYearlyIncrease;//emp.Salary.YearlySalaryIncrement;
                s.PayAccountID = emp.Salary.SalaryPayAccountID;
            }
        }
        private void UpdateBonus(EmployeeModel emp, Employee dbEmp)
        {
            foreach (var b in dbEmp.Bonuses)
            {
                // EmployeeID = dbEmp.EmployeeID,
                b.BasedSalary = emp.Salary.Salary1;
                b.BonusTypeID = emp.Bonus.BonusTypeID;
                b.BonusValue = emp.Bonus.BonusValue;
                b.BonusFinalAmount = emp.Bonus.BonusFinalAmount;
                b.PayAccountID = emp.Bonus.BonusPayAccountID;
            }
        }
        private void UpdateTitles(EmployeeModel emp, Employee dbEmp)
        {
            var titleName = _domainObjectRepository.Get<TitleType>(t => t.TitleTypeId == emp.Title.TitleTypeId)?.TitleTypeName;
            foreach (var t in dbEmp.Titles)
            {
                //    EmployeeID = dbEmp.EmployeeID,
                t.FromDate = dbEmp.HireDate ?? DateTime.Today;
                t.TitleTypeId = emp.Title.TitleTypeId;
                t.Title1 = emp.Title.Title1 ?? titleName;
            }
        }
        private void UpdateInsurances(EmployeeModel emp, Employee dbEmp)
        {
            foreach (var ins in dbEmp.EmployeeInsurances)
            {
                if (ins.InsuranceTypeID == 1)// health insurance
                {
                    ins.BasedSalary = emp.Salary.Salary1;
                    ins.InsuranceAmount = emp.HealthInsurance.InsuranceAmount;
                    ins.InsuranceOptionID = emp.HealthInsurance.InsuranceOptionID;
                    ins.InsuranceTypeID = emp.HealthInsurance.InsuranceTypeID;
                    ins.PayAccountID = emp.Insurance.InsurancePayAccountID;
                }
                else if (ins.InsuranceTypeID == 2)// dental insurance
                {
                    ins.BasedSalary = emp.Salary.Salary1;
                    ins.InsuranceAmount = emp.DentalInsurance.InsuranceAmount;
                    ins.InsuranceOptionID = emp.DentalInsurance.InsuranceOptionID;
                    ins.InsuranceTypeID = emp.DentalInsurance.InsuranceTypeID;
                    ins.PayAccountID = emp.Insurance.InsurancePayAccountID;
                }
                //else if (ins.InsuranceTypeID == 3)
                //{
                //    ins.BasedSalary = emp.Salary.Salary1;
                //    ins.InsuranceAmount = otherInsurance.InsuranceAmount;
                //    ins.InsuranceOptionID = otherInsurance.InsuranceOptionID;
                //    ins.InsuranceTypeID = otherInsurance.InsuranceTypeID;
                //    ins.PayAccountID = emp.Insurance.InsurancePayAccountID;
                //}
            }
            }
        private void UpdateOvertimes(EmployeeModel emp, Employee dbEmp)
        {
            foreach (var o in dbEmp.Overtimes)
            {
                // EmployeeID = dbEmp.EmployeeID,
                o.BasedSalary = emp.Salary.Salary1;
                o.CalculatedOverTimeAmount = emp.Overtime.CalculatedOverTimeAmount;
                o.HourlyRate = emp.Overtime.HourlyRate;
                o.OvertimeHours = emp.Overtime.OvertimeHours;
                o.RegularHours = emp.Overtime.RegularHours;
                o.PayAccountID = emp.Overtime.OvertimePayAccountID;
            }
            

        }
        private void UpdateDeductions(EmployeeModel emp, Employee dbEmp)
        {
            foreach (var d in dbEmp.SalaryDeductions)
            {
                d.BasedSalary = emp.Deduction.BasedSalary;
                d.DeductedDays = emp.Deduction.DeductedDays;
                d.DeductedHours = emp.Deduction.DeductedHours;
                d.DeductionAmount = emp.Deduction.DeductionAmount;
                d.DeductionPercentage = emp.Deduction.DeductionPercentage;
                d.EmployeeID = emp.EmployeeID;
                d.HourlyRate = emp.Deduction.HourlyRate;
            }
            //dbEmp.SalaryDeductions = new List<SalaryDeduction>()
            //{
            //    new SalaryDeduction()
            //    {
            //        BasedSalary=emp.Deduction.BasedSalary,
            //        DeductedDays=emp.Deduction.DeductedDays,
            //        DeductedHours=emp.Deduction.DeductedHours,
            //        DeductionAmount=emp.Deduction.DeductionAmount,
            //        DeductionPercentage=emp.Deduction.DeductionPercentage,
            //        //EmployeeID=emp.EmployeeID,
            //        HourlyRate=emp.Deduction.HourlyRate
            //    }
            //};
        }
        public EmployeeModel UpdateEmployee(EmployeeModel emp)
        {
            //var healthInsurance = emp.EmployeeInsurances.Find(hi => hi.InsuranceTypeID == 1);
            //var dentalInsurance = emp.EmployeeInsurances.Find(hi => hi.InsuranceTypeID == 2);
            //var otherInsurance = emp.EmployeeInsurances.Find(hi => hi.InsuranceTypeID == 3);
            var dbEmp = _domainObjectRepository.Get<Employee>(em => em.EmployeeID == emp.EmployeeID, new string[] { "Payrolls", "DepartmentEmployees", "DepartmentManagers", "Salaries", "Bonuses", "EmployeeInsurances", "Overtimes", "SalaryDeductions", "EmployeeAccountTypes", "Titles" });
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    try
                    {
                        dbEmp.City = emp.City;
                        dbEmp.DateOfBirth = emp.DateOfBirth;
                        dbEmp.Education = emp.Education;
                        dbEmp.Email = emp.Email;
                        dbEmp.EmployeeFName = emp.EmployeeFName;
                        dbEmp.EmployeeLName = emp.EmployeeLName;
                        dbEmp.EmployeeMName = emp.EmployeeMName;
                        dbEmp.Employeephone = emp.Employeephone;
                        dbEmp.EndDate = emp.EndDate;
                        dbEmp.GENDER = emp.GENDER;
                        dbEmp.ImmigrationStatus = emp.ImmigrationStatus;
                        dbEmp.Nationality = emp.Nationality;
                        dbEmp.Notes = emp.Notes;
                        dbEmp.PhoneNumber = emp.PhoneNumber;
                        dbEmp.Qualification = emp.Qualification;
                        dbEmp.SocialStatus = emp.SocialStatus;
                        dbEmp.Photograph = emp.Photograph;
                        dbEmp.State = emp.State;
                        dbEmp.StreetAddress = emp.StreetAddress;
                        dbEmp.Training = emp.Training;
                        dbEmp.ZipCode = emp.ZipCode;
                        dbEmp.HireDate = emp.HireDate;
                        dbEmp.SocialStatusID = emp.SocialStatusID;

                        UpdateSalary( emp, dbEmp);
                        UpdateBonus( emp, dbEmp);
                        UpdateTitles( emp, dbEmp);
                        UpdateInsurances( emp, dbEmp);
                        UpdateOvertimes( emp, dbEmp);
                        UpdateDeductions( emp, dbEmp);
                        var employee = _domainObjectRepository.Update<Employee>(dbEmp);
                        scope.Complete();
                    }
                    finally
                    {
                        scope.Dispose();

                    }
                }
            }
            return emp;
            //throw new NotImplementedException();
        }
        #endregion

        #region Employee Regular Payment
        public void CreateEmployeeRegularPayment(List<EmployeeModel> EmployeeList)
        {
            var wirePayrollMethodId = _domainObjectRepository.Get<PayrollMethod>(p => p.PayrollMethodName == "Wire").PayrollMethodId;
            var payrollStatusID = _domainObjectRepository.Get<PayrollStatu>(p => p.PayrollStatusName == "Entered").PayrollStatusID;
            var payrollTypeID = _domainObjectRepository.Get<PayrollType>(p => p.PayrollTypeName == "Sallary").PayrollTypeID;
            var payeeTypeID = _domainObjectRepository.Get<PayeesType>(p => p.PayeeType == "Employee").PayeeTypeID;
            var agencyID = _domainObjectRepository.Get<Agency>(p => p.AgencyName == "O").AgencyID;
            var pay = new Payroll();
            var accounts = CreateAccounts(EmployeeList);
            // decimal payrollAmount = 0;
            var payrollSalaryAccount = _domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "SALARY").PayAccountID;
            var payrollInsuranceAccount = _domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "HEALTH INSURANCE").PayAccountID;
            var payrollUsTaxesAccount = _domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "US TAXES").PayAccountID;
            // add accounts to each employee


            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var payrollList = EmployeeList.Select(p => new Payroll()
            {
                PayrollMethodId = wirePayrollMethodId,
                PayrollStatusID = payrollStatusID,
                PayrollTypeID = payrollTypeID,
                PayeeTypeID = payeeTypeID,
                // PayeeID= p.EmployeeID,// only when there is a payee other than employee
                EmployeeID = p.EmployeeID,
                PayeeName = p.EmployeeName,
                //Amount = p.PayrollAccounts.Sum(a=>a.) //payrollAmount,//p.Salary.Salary1,
                PaymentDueDate = DateTime.Now,// the end of the month of the curent month
                PaymentCreatedDate = DateTime.Now,
                PaymentCreatedBy = p.CreatedBy,
                PaymentEnteredBy = p.CreatedBy,
                PaymentEnteredDate = DateTime.Now,

                Accounts = new List<Account>() {//accounts, 
                    new Account()
                    {
                        PayrollAccountID = payrollSalaryAccount,//_domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "SALARY").PayAccountID,
                        AccountName = "SALARY",
                        Amount = p.Salary.Salary1,
                        Descriptions = "Employee Salary",
                        Discount = 0,
                    },
                    p.Salary?.TaxCategoryID == 2 ?new Account()
                    {
                        PayrollAccountID = payrollUsTaxesAccount,//_domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "US TAXES").PayAccountID,
                        AccountName = "US TAXES",
                        Amount = 30,// add $30 toward salary
                        Descriptions = "add $30 toward salary",
                        Discount = 0,
                    }:null,
                    p.HealthInsurance?.InsuranceOptionID==2 ? new Account()
                    {
                        PayrollAccountID = payrollInsuranceAccount,//_domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "HEALTH INSURANCE").PayAccountID,
                        AccountName = "HEALTH INSURANCE",
                        Amount = p.HealthInsurance.InsuranceAmount,
                        Descriptions = "Insurance Credited toward the salary",
                        Discount = 0,
                    }:null
                },
                AgencyID = agencyID,
                StartDate = startDate,
                EndDate = endDate
            }).ToList();
            //IList<Payroll> result =
            foreach (var p in payrollList)
            {
                p.Amount = p.Accounts != null ? p.Accounts.Sum(a => a != null ? (a.Amount - a.Discount) : 0) ?? 0 : 0;
            }
            _domainObjectRepository.CreateBulk<Payroll>(payrollList);
            //   return result.ToList();
            /*
                --	TransactionID int identity(1,1),
                --	TransactionIDFormat AS CONCAT('V', TransactionID),
                --	PayrollMethodId int not null,-- wire or check
                --	PayrollStatusID int not null, -- entered, approved, authorized, paid, reconciled
                --	PayrollTypeID int not null,-- Sallary, Overtime, Bonus, Contract, Other
                --	AgencyID int  null,-- refrences Agencies table id
                --	PayeeID Int  null,-- either employeeid or payeeid
                --	EmployeeID int  null, -- when the the payee is employee we get the Id otherwise in null
                --	AccountNumber int  null,-- refrences Account table
                --	PayeeName nvarchar(150) not null,-- either the employee name or other payee name
                --	PayeeTypeID int not null,-- employee, others
                --	Amount money not null, -- 
                --	PaymentDueDate Date not null,
                --	PaymentCreatedDate Date not null,

                --	WireNumber int  null ,-- WireNumber is created once the status is paid and payRollmethod is a wire
                --	CheckNumber int null,-- CheckNumber is created once the status is paid and payRollmethod is a check

                --	PaymentCreatedBy nvarchar(50) not null,
                --	PaymentEnteredBy nvarchar(50) not null,-- for entered status id
                --	PaymentEnteredDate Date not null,-- for entered status id
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

        private decimal? CalculateSalary(EmployeeModel employee, bool? includeTax = true, bool? includeInsurance = true)
        {           
            var salary = employee.Salary;
            //  var baseSalary = salary.StartingSalary ;
            decimal? calculatedSalary = salary.StartingSalary;
            //int taxCategoryId = salary.TaxCategoryID ?? 0;
            //var taxCaregoryValue = _domainObjectRepository.Get<TaxCategory>(t => t.TaxCategoryID == taxCategoryId).TaxCategoryValue;
            var increaseAmount = salary.YearlySalaryIncrement ?? _domainObjectRepository.Get<TitleType>(t => t.TitleTypeId == employee.Title.TitleTypeId)?.TitleYearlyIncrease;
            bool isBetweenJan1_June30 = false;
            bool isBetweenJuly1_Dec30 = false;
            bool isExperienceOverAYear = false;
            int increaseMonth = 0;
            int increaseYearStart = 0;
            DateTime increaseDateStart = new DateTime();
            var hireDate = employee.HireDate ?? DateTime.Now;// exp: 5/30/2019

            var hireMonth = hireDate.Month;
            var hireDay = hireDate.Day;
            var hireYear = hireDate.Year;
            var todayDate = DateTime.Now;
            var daysOfExperience = (todayDate - hireDate);
            var yearsOfexperience = daysOfExperience != null ? (daysOfExperience.Days / 365) : 0;
            DateTime Jan1 = new DateTime(hireYear, 1, 1);
            DateTime June30 = new DateTime(hireYear, 6, 30);
            DateTime July1 = new DateTime(hireYear, 7, 1);
            DateTime Dec31 = new DateTime(hireYear, 12, 31);

            if (hireDate <= June30 && hireDate >= Jan1)
            {
                isBetweenJan1_June30 = true;
            }
            else if (hireDate <= Dec31 && hireDate >= July1)
            {
                isBetweenJuly1_Dec30 = true;
            }

            if (yearsOfexperience >= 1)// over a year of experince
            {
                isExperienceOverAYear = true;
            }
            if (isExperienceOverAYear && isBetweenJan1_June30)
            {
                increaseMonth = 7;
                increaseYearStart = hireYear + 1;
                increaseDateStart = new DateTime(increaseYearStart, increaseMonth, 1);
            }
            if (isExperienceOverAYear && isBetweenJuly1_Dec30)
            {
                increaseMonth = 1;
                increaseYearStart = hireYear + 2;
                increaseDateStart = new DateTime(increaseYearStart, increaseMonth, 1);
            }
            var daysDiff = (todayDate - increaseDateStart);
            var numOfDays = daysDiff.Days;
            var yearsOfIncrease = numOfDays / 365;
            //var yearsOfIncrease = (todayDate - increaseDateStart).Days / 365;
            if (isExperienceOverAYear)
            {
                if (todayDate >= increaseDateStart)
                    for (int j = 0; j <= yearsOfIncrease; j++)// include the first year increase
                    {
                        calculatedSalary += increaseAmount ?? 0;
                    }
            }
            // add tax category
            if (includeTax == true)
                calculatedSalary = AddTaxPercentageToSalary(employee, calculatedSalary);
            // add insurance value
            if (includeInsurance == true)
                calculatedSalary = AddInsuranceToSalary(employee, calculatedSalary);

            return Math.Round(calculatedSalary ?? 0, 2, MidpointRounding.ToEven);// Decimal.Round(calculatedSalary??0) ;

        }

        private decimal? AddInsuranceToSalary(EmployeeModel employee, decimal? experienceSalary)
        {
            var healtInsurance = employee.HealthInsurance;//EmployeeInsurances[0]
            var dentalInsurance = employee.DentalInsurance;
            decimal? experiencePlusInsuranceSalary = experienceSalary;
            if (healtInsurance.InsuranceOptionID == 1)// debit
            {
                experiencePlusInsuranceSalary -= healtInsurance.InsuranceAmount;
            }
            else if (healtInsurance.InsuranceOptionID == 2)// credit
            {
                experiencePlusInsuranceSalary += healtInsurance.InsuranceAmount;
            }
            if (dentalInsurance.InsuranceOptionID == 1)// debit
            {
                experiencePlusInsuranceSalary -= dentalInsurance.InsuranceAmount;
            }
            else if (dentalInsurance.InsuranceOptionID == 2)// credit
            {
                experiencePlusInsuranceSalary += dentalInsurance.InsuranceAmount;
            }

            return experiencePlusInsuranceSalary;
        }

        private decimal? AddTaxPercentageToSalary(EmployeeModel employee, decimal? experienceSalary)
        {
            decimal? experiencePlusTaxSalary = experienceSalary;
            var salary = employee.Salary;
            int taxCategoryId = salary.TaxCategoryID ?? 0;
            var taxCaregoryValue = _domainObjectRepository.Get<TaxCategory>(t => t.TaxCategoryID == taxCategoryId)?.TaxCategoryValue;
            if (taxCategoryId != 0)
            {
                experiencePlusTaxSalary += (experiencePlusTaxSalary * taxCaregoryValue) / 100;
            }

            return experiencePlusTaxSalary;

        }

        private List<Account> CreateAccounts(List<EmployeeModel> EmployeeList)
        {
            List<Account> accounts = new List<Account>();
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                accounts.Add(new Account()
                {
                    PayrollAccountID = _domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "SALARY").PayAccountID,
                    AccountName = "SALARY",
                    Amount = EmployeeList[i].Salary.Salary1,
                    Descriptions = "Employee Salary",
                    Discount = 0,
                });
                for (int j = 0; j < EmployeeList[i].EmployeeInsurances.Count; j++)
                {
                    if (EmployeeList[i].EmployeeInsurances[j].InsuranceTypeID == 1)// Health Insurance
                    {
                        if (EmployeeList[i].EmployeeInsurances[j].InsuranceOptionID == 2)//Credit insurance, that is we pay the amount of insurance to be credited toward the amount
                        {
                            accounts.Add(new Account()
                            {
                                PayrollAccountID = _domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "HEALTH INSURANCE").PayAccountID,
                                AccountName = "HEALTH INSURANCE",
                                Amount = EmployeeList[i].EmployeeInsurances[j].InsuranceAmount,
                                Descriptions = "Insurance Credited toward the salary",
                                Discount = 0,
                            });
                        }
                    }
                }

                if (EmployeeList[i].Salary.TaxCategoryID == 2)// Tax category 30%
                {

                    accounts.Add(new Account()
                    {
                        PayrollAccountID = _domainObjectRepository.Get<PayrollAccount>(pa => pa.Description == "US TAXES").PayAccountID,
                        AccountName = "US TAXES",
                        Amount = 30,// add $30 toward salary
                        Descriptions = "add $30 toward salary",
                        Discount = 0,
                    });

                }
            }

            return accounts;
        }

        private decimal? CalculateSeveranceSalary(EmployeeModel  emp, int? vacationDays)
        {
            //Date of Hire End Date
            //1 - 5 years: ½ Current Salary (base + exp) x NY
            // 6 - 35 : Current Salary (base + exp) x NY
            //   + Vacation[60 days MAX]
            decimal? severanceSalary = 0;
               var endDate = emp.EndDate ?? DateTime.Now;
            var hireDate = emp.HireDate?? DateTime.Now;
            var daysOfExperience = (endDate - hireDate);
            var yearsOfexperience = daysOfExperience != null ? (daysOfExperience.Days / 365) : 0;
            var experienceSalary = emp.Salary.ExperienceSalary;
            const int minYearOfExperience = 1;
            const int midYearOfExperience = 5;
            const int maxYearOfExperience = 35;
            if (yearsOfexperience> minYearOfExperience && yearsOfexperience<= midYearOfExperience)//1 - 5 years
            {
                severanceSalary = ((experienceSalary / 2) * yearsOfexperience) + vacationDays??0;
            }
            else if ((yearsOfexperience > midYearOfExperience && yearsOfexperience <= maxYearOfExperience) || yearsOfexperience > maxYearOfExperience)// 6 - 35
            {
                severanceSalary = ((experienceSalary) * yearsOfexperience) + vacationDays ?? 0;
            }

            if (vacationDays!=null)
            {
                if (vacationDays>60)
                {
                    vacationDays = 60;
                }
                severanceSalary += vacationDays;
            }
            return severanceSalary;


        }

        private decimal? CalculateVacationSalary(EmployeeModel emp, int? numberOfDays)
        {
            // Current Salary(base + exp) x 12 / 365 x ND
            decimal? calculatedVacationSalary=0;
            var experienceSalary = emp.Salary.ExperienceSalary;
            var daySalary = (experienceSalary * 12) / 365;
            calculatedVacationSalary = daySalary * numberOfDays??0;
            return calculatedVacationSalary;
        }

        private decimal? CalculateResignationSalary(EmployeeModel emp, int? vacationDays)
        {
            //Date of Hire End Date
            //1 - 5 years: ½ Current Salary (base + exp) x NY
            // 6 - 35 : Current Salary (base + exp) x NY
            //   + Vacation[60 days MAX]
            decimal? severanceSalary = 0;
            var endDate = emp.EndDate ?? DateTime.Now;
            var hireDate = emp.HireDate ?? DateTime.Now;
            var daysOfExperience = (endDate - hireDate);
            var yearsOfexperience = daysOfExperience != null ? (daysOfExperience.Days / 365) : 0;
            var experienceSalary = emp.Salary.ExperienceSalary;
            const int minYearOfExperience = 1;
            const int midYearOfExperience = 5;
            const int maxYearOfExperience = 35;
            if (yearsOfexperience > minYearOfExperience && yearsOfexperience <= midYearOfExperience)//1 - 5 years
            {
                severanceSalary = ((experienceSalary / 2) * yearsOfexperience) + vacationDays ?? 0;
            }
            else if ((yearsOfexperience > midYearOfExperience && yearsOfexperience <= maxYearOfExperience) || yearsOfexperience > maxYearOfExperience)// 6 - 35
            {
                severanceSalary = ((experienceSalary) * yearsOfexperience) + vacationDays ?? 0;
            }

            if (vacationDays != null)
            {
                if (vacationDays > 60)
                {
                    vacationDays = 60;
                }
                severanceSalary += vacationDays;
            }
            return severanceSalary;


        }

        private decimal? CalculateFiredSalary(EmployeeModel emp)
        {
            //2 months of Current Salary + Insurance(Credit)
            decimal? severanceSalary = 0;
            var experienceSalary = emp.Salary.ExperienceSalary;
            var insuranceWithExperienceSalary = emp.Salary.ExperienceWithInsuranceSalary;
            var insuranceSalary = insuranceWithExperienceSalary - experienceSalary;
            if (insuranceSalary > 0)
            {
                severanceSalary = experienceSalary + insuranceSalary;
            }else
            {
                severanceSalary = experienceSalary;
            }
            return severanceSalary;


        }
        #endregion
    }
}
