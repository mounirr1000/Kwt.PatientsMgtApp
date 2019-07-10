using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PaymentDeductionRepository : IPaymentDeductionRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ICompanionRepository _companionRepository;
        private readonly IPayRateRepository _payRateRepository;

        public PaymentDeductionRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
            _beneficiaryRepository = new BeneficiaryRepository();
            _patientRepository = new PatientRepository();
            _companionRepository = new CompanionRepository();
            _payRateRepository = new PayRateRepository();
        }

        public List<PaymentDeductionModel> GetPaymentDeductions()
        {
            var deductions = _domainObjectRepository.All<PaymentDeduction>(new[] { "Beneficiary", "Patient", "PayRate", "Companion", "Payment" });

            return deductions.Select(p => p.Beneficiary != null ? new PaymentDeductionModel()
            {
                PatientCID = p.PatientCID,
                //Agency = p.Patient.Agency.AgencyName,
                ////CompanionAmount = p.CAmount,
                //PatientFName = p.Patient.PatientFName,
                //PatientLName = p.Patient.PatientLName,
                //PatientMName = p.Patient.PatientMName,
                //BeneficiaryFName = p.Beneficiary.BeneficiaryFName,
                //BeneficiaryLName = p.Beneficiary.BeneficiaryLName,
                //BeneficiaryMName = p.Beneficiary.BeneficiaryMName,
                //BeneficiaryBank = _domainObjectRepository.Get<Bank>(b=>b.BankName==p.Beneficiary.BankName).BankName,
                //BeneficiaryIBan = p.Beneficiary.IBan,
                //CompanionCID = p.CompanionCID,
                //CompanionFName = p.Companion.CompanionFName,
                //CompanionLName = p.Companion.CompanionLName,
                //CompanionMName = p.Companion.CompanionMName,
                CreatedBy = p.CreatedBy,
                CreatedDate = p.CreatedDate,
                Notes = p.Notes,
                DeductionEndDate = p.EndDate,
               // Hospital = p.Patient.Hospital.HospitalName,
                ModifiedBy = p.ModifiedBy,
                ModifiedDate = p.ModifiedDate,
                //PatientAmount = p.PAmount,
                //CompanionPayRate = p.PayRate.CompanionRate,
                //PatientPayRate = p.PayRate.PatientRate,
                //PaymentDate = p.PaymentDate ?? p.CreatedDate,
                //PaymentLengthPeriod = p.Period,
                DeductionStartDate = p.StartDate,
                //TotalDue = p.TotalDue,
                //Id = p.PaymentID,
               // BeneficiaryCID = p.Beneficiary.BeneficiaryCID,
                PayRateID = p.PayRateID,
                PatientPhone =  p.Patient.USphone ?? p.Patient.KWTphone,
                Payment = new PaymentModel()
                {
                    PaymentID = p.Payment.PaymentID,
                    CompanionAmount = p.Payment.CAmount,
                    PatientAmount = p.Payment.PAmount,
                    TotalDue = p.Payment.TotalDue,
                    PayRateID = p.Payment.PayRateID,


                }

            } : null).OrderBy(p => p.CreatedDate).ToList();// returns null when the beneficiary is not set and the second null is returned when the companion is not set

        }

        public PaymentDeductionModel GetPaymentDeductionById(int paymentDeductionId)
        {
            throw new NotImplementedException();
        }

        public PaymentDeductionModel GetPaymentDeductionByPaymentId(int paymentId)
        {
            var payDeduction = _domainObjectRepository.Get<PaymentDeduction>(pd => pd.PaymentID == paymentId);

            if (payDeduction != null)
                return new PaymentDeductionModel()
                {
                    PaymentID = payDeduction.PaymentID,
                    FinalAmount = payDeduction.FinalAmount,
                    DeductionDate = payDeduction.DeductionDate,
                    PayRateID = payDeduction.PayRateID,
                    TotalDeduction = payDeduction.TotalDeduction,
                    CompanionAmount = payDeduction.CDeduction,
                    //BeneficiaryCID = payDeduction.,
                    Notes = payDeduction.Notes,
                    CreatedDate = payDeduction.CreatedDate,
                    //CompanionCID = payDeduction.CompanionCID,
                    PatientDeduction = payDeduction.PDeduction,
                    PatientCID = payDeduction.PatientCID
                };
            return null;
        }
        public void AddPaymentDeduction(PaymentDeductionModel paymentDeduction)
        {
            PaymentDeduction newPayment = new PaymentDeduction()
            {
                PaymentID = paymentDeduction.PaymentID,
                DeductionDate = DateTime.Now,
                PatientCID = paymentDeduction.PatientCID,
               // CompanionCID = paymentDeduction.CompanionCID,
                BeneficiaryID = _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == paymentDeduction.PatientCID)?.BeneficiaryID,
                PayRateID = paymentDeduction.PayRateID,
                //_domainObjectRepository.Get<PayRate>(pr => pr.CompanionRate == paymentDeduction.CompanionPayRate
                //                      && pr.PatientRate == paymentDeduction.PatientPayRate)?.PayRateID,
                StartDate = paymentDeduction.DeductionStartDate,
                EndDate = paymentDeduction.DeductionEndDate,
                CDeduction = paymentDeduction.CompanionDeduction,
                PDeduction = paymentDeduction.PatientDeduction,
                TotalDeduction = paymentDeduction.CompanionDeduction + paymentDeduction.PatientDeduction,
                FinalAmount = paymentDeduction.FinalAmount,
                CreatedBy = paymentDeduction.CreatedBy,
                Notes = paymentDeduction.Notes,
                ModifiedBy = paymentDeduction.ModifiedBy,
                CreatedDate = DateTime.Now,

            };
            _domainObjectRepository.Create<PaymentDeduction>(newPayment);
        }

        public PaymentModel UpdatePaymentDeduction(PaymentDeductionModel paymentDeduction)
        {
            throw new NotImplementedException();
        }

        public int DeletePaymentDeduction(PaymentDeductionModel paymentDeduction)
        {
            throw new NotImplementedException();
        }

        public PaymentDeductionModel GetPaymentDeductionObject(string patientCid)
        {
            throw new NotImplementedException();
        }
    }
}
