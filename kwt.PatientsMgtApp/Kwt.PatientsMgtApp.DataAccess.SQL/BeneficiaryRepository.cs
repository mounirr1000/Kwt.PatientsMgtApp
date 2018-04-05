using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class BeneficiaryRepository: IBeneficiaryRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;
        public BeneficiaryRepository()
        {
            _domainObjectRepository= new DomainObjectRepository();
        }
        public List<BeneficiaryModel> GetBeneficiaries()
        {
            var beneficiaries=  _domainObjectRepository.All<Beneficiary>(new[] {"Payments"}).ToList();

            return beneficiaries.Select(b => new BeneficiaryModel()
            {
                BankName = _domainObjectRepository.Get<Bank>(ba=>ba.BankID== b.BankID).BankName,
                BankCode = _domainObjectRepository.Get<Bank>(ba => ba.BankID == b.BankID).BankCode,
                PatientCID = b.PatientCID,
                CompanionCID = b.CompanionCID,
                BeneficiaryCID = b.BeneficiaryCID,
                BeneficiaryFName = b.BeneficiaryFName,
                BeneficiaryLName = b.BeneficiaryLName,
                BeneficiaryMName = b.BeneficiaryMName,
                IBan = b.IBan,
                Id = b.BeneficiaryID
            }).ToList();
        }

        public BeneficiaryModel GetBeneficiary(string patientCid)
        {
           var beneficiary= _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == patientCid, new [] {"Payments"});

            
            return new BeneficiaryModel()
            {
                BankName = _domainObjectRepository.Get<Bank>(ba => ba.BankID == beneficiary.BankID).BankName,
                BankCode = _domainObjectRepository.Get<Bank>(ba => ba.BankID == beneficiary.BankID).BankCode,
                PatientCID = beneficiary.PatientCID,
                CompanionCID = beneficiary.CompanionCID,
                BeneficiaryCID = beneficiary.BeneficiaryCID,
                BeneficiaryFName = beneficiary.BeneficiaryFName,
                BeneficiaryLName = beneficiary.BeneficiaryLName,
                BeneficiaryMName = beneficiary.BeneficiaryMName,
                IBan = beneficiary.IBan,
                Id = beneficiary.BeneficiaryID
                
            };
        }
    }
}
