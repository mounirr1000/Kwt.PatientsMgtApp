﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ICompanionRepository _companionRepository;
        private readonly IPayRateRepository _payRateRepository;


        public PaymentRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
            _beneficiaryRepository = new BeneficiaryRepository();
            _patientRepository = new PatientRepository();
            _companionRepository = new CompanionRepository();
            _payRateRepository = new PayRateRepository();
        }


        //new February 28, 2019
        public List<PaymentModel> GetPaymentsWithPhone(DateTime? date)
        {
            var payments = _domainObjectRepository.Filter<Payment>( p=>p.PaymentDate ==date,new[] {"Patient"});

            return payments.Select(p => p.Beneficiary != null ? new PaymentModel()
            {
                PatientPhone = p.Patient.KWTphone ?? p.Patient.USphone,
                PatientCID = p.PatientCID,
                PaymentStartDate = p.StartDate,
                PaymentEndDate = p.EndDate,
                TotalDue = p.TotalDue,

        } : null).OrderBy(p => p.CreatedDate).ToList();// returns null when the beneficiary is not set and the second null is returned when the companion is not set

        }
        //
        public List<PaymentReportModel> GetPaymentsReport(string patientCid = null, DateTime? startDate = null,
            DateTime? endDate = null)
        {

            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("pCid", patientCid);
            parms.Add("startDate", startDate);
            parms.Add("endDate", endDate);

            //pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter
            return _domainObjectRepository.ExecuteProcedure<PaymentReportModel>("GetPaymentListReport_SP", parms, false);
        }
        public List<PaymentModel> GetPayments()
        {
            var payments = _domainObjectRepository.All<Payment>(new[] { "Beneficiary", "Patient", "PayRate", "Companion" });

            return payments.Select(p => p.Beneficiary != null ? new PaymentModel()
            {
                PatientCID = p.PatientCID,
                Agency = p.Patient.Agency.AgencyName,
                CompanionAmount = p.CAmount,
                PatientFName = p.Patient.PatientFName,
                PatientLName = p.Patient.PatientLName,
                PatientMName = p.Patient.PatientMName,
                BeneficiaryFName = p.Beneficiary.BeneficiaryFName,
                BeneficiaryLName = p.Beneficiary.BeneficiaryLName,
                BeneficiaryMName = p.Beneficiary.BeneficiaryMName,
                //BeneficiaryBank = _domainObjectRepository.Get<Bank>(b=>b.BankName==p.Beneficiary.BankName).BankName,
                BeneficiaryIBan = p.Beneficiary.IBan,
                CompanionCID = p.CompanionCID,
                //CompanionFName = p.Companion.CompanionFName,
                //CompanionLName = p.Companion.CompanionLName,
                //CompanionMName = p.Companion.CompanionMName,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Notes = p.Notes,
                PaymentEndDate = p.EndDate,
                Hospital = p.Patient.Hospital.HospitalName,
                ModifiedBy = p.ModifiedBy,
                ModifiedDate = p.ModifiedDate,
                PatientAmount = p.PAmount,
                CompanionPayRate = p.PayRate.CompanionRate,
                PatientPayRate = p.PayRate.PatientRate,
                PaymentDate = p.PaymentDate ?? p.CreatedDate,
                PaymentLengthPeriod = p.Period,
                PaymentStartDate = p.StartDate,
                TotalDue = p.TotalDue,
                Id = p.PaymentID,
                BeneficiaryCID = p.Beneficiary.BeneficiaryCID,
                PayRateID =p.PayRateID,
                PatientPhone = p.Patient.KWTphone ?? p.Patient.USphone

            } : null).OrderBy(p => p.CreatedDate).ToList();// returns null when the beneficiary is not set and the second null is returned when the companion is not set

        }

        public PaymentModel GetPaymentObject(string patientCid)
        {
            PaymentModel payment = new PaymentModel();
            var ben = _beneficiaryRepository.GetBeneficiary(patientCid);
            var patient = _patientRepository.GetPatient(patientCid);
            var companion = _companionRepository.GetCompanion(ben?.CompanionCID);
            if (patient != null && ben != null)
            {
                payment.IsActive = patient.IsActive;
                payment.PatientCID = patientCid;
                payment.PatientFName = patient.PatientFName;
                payment.PatientLName = patient.PatientLName;
                payment.PatientMName = patient.PatientMName;
                payment.Agency = patient.Agency;
                payment.Hospital = patient.Hospital;
                payment.CompanionCID = ben.CompanionCID;
                payment.CompanionFName = companion?.CompanionFName;
                payment.CompanionLName = companion?.CompanionLName;
                payment.CompanionMName = companion?.CompanionMName;
                payment.BeneficiaryMName = ben.BeneficiaryFName;
                payment.BeneficiaryBank = ben.BankName;
                payment.BeneficiaryIBan = ben.IBan;
                payment.BeneficiaryCID = ben.BeneficiaryCID;
                payment.BeneficiaryFName = ben.BeneficiaryFName;
                payment.BeneficiaryLName = ben.BeneficiaryLName;

               
                //Todo: This should be done a better way
                payment.Payments = GetPaymentsByPatientCid(patientCid)?.OrderByDescending(p=>p.CreatedDate).ToList();
                payment.PayRates = _payRateRepository.GetPayRatesList();
                payment.PatientPayRate = _domainObjectRepository.Get<PayRate>(c => c.PayRateID == 1).PatientRate;
                payment.CompanionPayRate = !String.IsNullOrEmpty(ben.CompanionCID) ? _domainObjectRepository.Get<PayRate>(c => c.PayRateID == 1).CompanionRate : 0;
            }
            return payment;
        }
        public PaymentModel GetPaymentById(int paymentid)
        {
            var payment = _domainObjectRepository.Get<Payment>(p => p.PaymentID == paymentid,
                new[] { "Beneficiary", "Patient", "PayRate", "Companion" });
            PaymentModel pay = new PaymentModel();
            if (payment != null)
            {
                var pa = payment.Patient;
                var c = payment.Companion;
                var pr = payment.PayRate;
                var be = payment.Beneficiary;

                // PaymentModel pay = new PaymentModel();
                //{
                pay.PatientCID = payment.PatientCID;
                pay.Agency = payment.Patient?.AgencyID != null
                    ? _domainObjectRepository.Get<Agency>(a => a.AgencyID == payment.Patient.AgencyID).AgencyName
                    : null;
                pay.CompanionAmount = payment.CAmount;
                pay.PatientFName = payment?.Patient?.PatientFName;
                pay.PatientLName = payment?.Patient?.PatientLName;
                pay.PatientMName = payment?.Patient?.PatientMName;
                pay.BeneficiaryFName = payment.Beneficiary?.BeneficiaryFName;
                pay.BeneficiaryLName = payment.Beneficiary?.BeneficiaryLName;
                pay.BeneficiaryMName = payment.Beneficiary?.BeneficiaryMName;
                pay.BeneficiaryBank = payment.Beneficiary != null
                    ? _domainObjectRepository.Get<Bank>(b => b.BankID == payment.Beneficiary.BankID)?.BankName
                    : "";
                pay.BeneficiaryIBan = payment.Beneficiary?.IBan;
                pay.CompanionCID = payment.CompanionCID;
                pay.CreatedBy = payment.CreatedBy;
                pay.CreatedDate = payment.CreatedDate;
                pay.Notes = payment.Notes;
                pay.PaymentEndDate = payment.EndDate;
                pay.Hospital = payment.Patient?.HospitalID != null
                    ? _domainObjectRepository.Get<Hospital>(a => a.HospitalID == payment.Patient.HospitalID)
                        .HospitalName
                    : null;
                pay.ModifiedBy = payment.ModifiedBy;
                pay.ModifiedDate = payment.ModifiedDate;
                pay.PatientAmount = payment.PAmount;
                pay.CompanionPayRate = payment.PayRate?.CompanionRate;
                pay.PatientPayRate = payment.PayRate?.PatientRate;
                pay.PaymentDate = payment.PaymentDate??payment.CreatedDate;
                pay.PaymentLengthPeriod = payment.Period;
                pay.PaymentStartDate = payment.StartDate;
                pay.TotalDue = payment.TotalDue;
                pay.Id = payment.PaymentID;
                pay.BeneficiaryCID = payment.Beneficiary?.BeneficiaryCID;
                pay.CompanionFName = payment.Companion?.CompanionFName;
                pay.CompanionLName = payment.Companion?.CompanionLName;
                pay.CompanionMName = payment.Companion?.CompanionMName;
                //};
                return pay;
            }

            return null;
        }

        public List<PaymentModel> GetPaymentsByPatientCid(string pacientcid)
        {
            IPaymentRepository paymentRepo = new PaymentRepository();
            var paymentList = paymentRepo.GetPayments();
            List<PaymentModel> payments = new List<PaymentModel>();
            if (paymentList.Count > 0)
                payments = paymentList.Where(p => p?.PatientCID == pacientcid).OrderBy(p => p?.CreatedDate)?.ToList();//_domainObjectRepository.Get<Payment>(p => p.PatientCID == pacientcid, new[] { "Beneficiary", "Patient", "PayRate" });

            return payments;
        }

        public List<PaymentModel> GetPaymentsByCompanionCid(string companioncid)
        {
            IPaymentRepository paymentRepo = new PaymentRepository();
            var paymentList = paymentRepo.GetPayments();
            List<PaymentModel> payments = new List<PaymentModel>();
            if (paymentList.Count > 0)
                payments = paymentList.Where(p => p?.CompanionCID == companioncid).OrderBy(p => p?.CreatedDate)?.ToList();//_domainObjectRepository.Get<Payment>(p => p.PatientCID == pacientcid, new[] { "Beneficiary", "Patient", "PayRate" });

            return payments;
        }
        public void AddPayment(PaymentModel payment)
        {
            //start date and end date payment difference should be positive
            if (payment?.PaymentEndDate != null && payment?.PaymentStartDate != null)
            {

                var currentEndDate = (DateTime)payment?.PaymentEndDate?.Date;
                var currentStartDate = (DateTime)payment?.PaymentStartDate?.Date;
                // start date and end date has to be always in the future
                //if ((currentStartDate - DateTime.Now).Days < 0 || ((currentEndDate - DateTime.Now).Days<0))
                //{
                //    throw new PatientsMgtException(1, "error", "Add new Payment", "The payment end date " + currentEndDate + " and the payment start date " + currentStartDate+" should be in the future");
                //}
                if ((currentEndDate - currentStartDate).Days <= 0)
                {
                    throw new PatientsMgtException(1, "error", "Add new Payment", "The payment end date " + currentEndDate + " Should be greater than payment start date " + currentStartDate);
                }

                //Todo This should be removed if the payment can be before 15 days period
                //if ((currentEndDate - currentStartDate).Days < Constants.NUMBER_OF_DAYS_BEFORE_NEXT_PAYMENT)
                //{
                //    throw new PatientsMgtException(1, "error", "Add new Payment", 
                //                        "The payment end date should be on " 
                //                        + currentStartDate.AddDays(Constants.NUMBER_OF_DAYS_BEFORE_NEXT_PAYMENT));
                //}
            }
            //check last payment for the same patient if there is one
            IPaymentRepository paymentRepository = new PaymentRepository();

            var lastPayment = paymentRepository.
                            GetPaymentsByPatientCid(payment?.PatientCID)?
                            .OrderByDescending(p => p.PaymentDate).FirstOrDefault();
            // payment should be only in 15 days period
            if (lastPayment?.PaymentEndDate != null)
            {
                var lastEndDate = (DateTime)lastPayment?.PaymentEndDate?.Date;
                if (payment?.PaymentStartDate?.Date != null)
                {
                    var currentStartDate = (DateTime)payment?.PaymentStartDate?.Date;
                    TimeSpan dateDiff = currentStartDate - lastEndDate;
                    //the current start date should not be less than the last payment end date
                    if (dateDiff.Days <= 0
                        //Check if the last payemt total that was made on same date is different
                        && lastPayment.TotalDue == payment.TotalDue
                        )
                    {
                        throw new PatientsMgtException(1, "error", "Add new Payment", "Last Payment end date was on " + lastEndDate + " So payment start date should be after " + lastEndDate);
                    }

                }
            }


            Payment newPayment = new Payment()
            {
                PatientCID = payment.PatientCID,
                AgencyID = _domainObjectRepository.Get<Agency>(a => a.AgencyName == payment.Agency)?.AgencyID,
                CAmount = payment.CompanionAmount,
                BeneficiaryID = _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == payment.PatientCID)?.BeneficiaryID,
                CompanionCID = payment.CompanionCID,
                CreatedBy = payment.CreatedBy,

                Notes = payment.Notes,
                EndDate = payment.PaymentEndDate,
                HospitalID = _domainObjectRepository.Get<Hospital>(h => h.HospitalName == payment.Hospital)?.HospitalID,
                ModifiedBy = payment.ModifiedBy,

                PAmount = payment.PatientAmount,
                PayRateID = _domainObjectRepository.Get<PayRate>(pr => pr.CompanionRate == payment.CompanionPayRate
                                                && pr.PatientRate == payment.PatientPayRate)?.PayRateID,
                PaymentDate = payment.PaymentDate ?? DateTime.Now,
                Period = payment.PaymentLengthPeriod,
                StartDate = payment.PaymentStartDate,
                TotalDue = payment.TotalDue,

            };
            _domainObjectRepository.Create<Payment>(newPayment);

        }

        public PaymentModel UpdatePayment(PaymentModel payment)
        {
            var paymentToUpdate = _domainObjectRepository.Get<Payment>(p => p.PaymentID == payment.Id);
            if (paymentToUpdate != null)
            {
                Payment updatedPayment = new Payment()
                {
                    PatientCID = payment.PatientCID,
                    AgencyID = _domainObjectRepository.Get<Agency>(a => a.AgencyName == payment.Agency).AgencyID,
                    CAmount = payment.CompanionAmount,
                    BeneficiaryID = _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == payment.PatientCID).BeneficiaryID,
                    CompanionCID = payment.CompanionCID,
                    CreatedBy = payment.CreatedBy,
                    CreatedDate = payment.CreatedDate,
                    Notes = payment.Notes,
                    EndDate = payment.PaymentEndDate,
                    HospitalID = _domainObjectRepository.Get<Hospital>(h => h.HospitalName == payment.Hospital).HospitalID,
                    ModifiedBy = payment.ModifiedBy,

                    PAmount = payment.PatientAmount,
                    PayRateID = _domainObjectRepository.Get<PayRate>(pr => pr.CompanionRate == payment.CompanionPayRate
                                                    && pr.PatientRate == payment.PatientPayRate).PayRateID,
                    PaymentDate = payment.PaymentDate,
                    Period = payment.PaymentLengthPeriod,
                    StartDate = payment.PaymentStartDate,
                    TotalDue = payment.TotalDue,
                    Id = payment.Id,
                    PaymentID = payment.Id,
                    ModifiedDate = DateTime.Now
                };
                paymentToUpdate = updatedPayment;
                //Todo This is a temp fix for updating the Payment
                PatientsMgtEntities dbContext = new PatientsMgtEntities();
                var entry = dbContext.Entry(paymentToUpdate);
                dbContext.Set<Payment>().Attach(paymentToUpdate);
                entry.State = EntityState.Modified;
                dbContext.SaveChanges();
                //
                //_domainObjectRepository.Update<Payment>(paymentToUpdate);

            }
            return payment;
        }

        public int DeletePayment(PaymentModel payment)
        {

            var pay = _domainObjectRepository.Get<Payment>(p => p.PaymentID == payment.PaymentID);
            var index = 0;
            if (pay != null)
            {
   
                index = _domainObjectRepository.Delete<Payment>(pay);
            }
            return index;
            //throw new NotImplementedException();
        }
    }
}
