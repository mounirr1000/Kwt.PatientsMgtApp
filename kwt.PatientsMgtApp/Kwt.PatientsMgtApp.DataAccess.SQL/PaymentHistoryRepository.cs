using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PaymentHistoryRepository : IPaymentHistoryRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

        public PaymentHistoryRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<PaymentHistoryModel> GetPaymentHistory()
        {
            var history = _domainObjectRepository.All<PaymentHistory>();

            return history.Select(ph => new PaymentHistoryModel()
            {
                PaymentHistoryID = ph.PaymentHistoryID,
                PaymentID = ph.PaymentID,
                CompanionCID = ph.CompanionCID,
                PatientCID = ph.PatientCID,
                PayRateID = ph.PayRateID,
                PAmount = ph.PAmount,
                PaymentDate = ph.PaymentDate,
                IsRejected = ph.IsRejected,
                Period = ph.Period,
                PaymentTypeId = ph.PaymentTypeId,
                RejectedPaymentId = ph.RejectedPaymentId,
                StartDate = ph.StartDate,
                EndDate = ph.EndDate,
                TotalCorrection = ph.TotalCorrection,
                TotalDue = ph.TotalDue,
                ModifiedBy = ph.ModifiedBy,
                ModifiedDate = ph.ModifiedDate,
                Notes = ph.Notes,
                CAmount = ph.CAmount,
                HospitalID = ph.HospitalID,
                FinalAmountAfterCorrection = ph.FinalAmountAfterCorrection,
                BeneficiaryID = ph.BeneficiaryID,
                AgencyID = ph.AgencyID,
                AdjustmentReasonID = ph.AdjustmentReasonID,
                CreatedBy = ph.CreatedBy,
                CreatedDate = ph.CreatedDate,
                BeneficiaryBankId = ph.BeneficiaryBankId,
                BeneficiaryFName = ph.BeneficiaryFName,
                BeneficiaryMName = ph.BeneficiaryMName,
                BeneficiaryLName = ph.BeneficiaryLName,
                BeneficiaryBankIban = ph.BeneficiaryBankIban,
                BeneficiaryCID = ph.BeneficiaryCID,
                BeneficiaryCompanionCid = ph.BeneficiaryCompanionCid,
                BeneficiaryPatientCid = ph.BeneficiaryPatientCid,
                BeneficiaryBankName = ph.BeneficiaryBankName,
                BeneficiaryBankCode = ph.BeneficiaryBankCode,
                AgencyName = ph.AgencyName,
                PaymentType = ph.PaymentType,
                AdjustmentReason = ph.AdjustmentReason,
                HospitalName = ph.HospitalName,
                CompanionRate = ph.CompanionRate,
                PatientRate = ph.PatientRate,

            }).ToList();
        }
    }
}
