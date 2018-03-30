using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtAppt.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

        public PatientRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<PatientModel> GetPatients()
        {
            var result = _domainObjectRepository.All<Patient>().ToList();
            //Map to returnable object
            return result.Select(m => new PatientModel()
            {
                Doctor = m.Doctor.DoctorName,
                BankName = m.Bank.BankName,
                Hospital = m.Hospital.HospitalName,
                Agency = m.Agency.AgencyName,
                BankCode = m.Bank.BankCode,
                Iban = m.Iban,
                IsActive = m.IsActive,
                EndTreatDate = m.EndTreatDate,
                FirstApptDAte = m.FirstApptDate,
                IsBeneficiary = m.IsBeneficiary,
                KWTPhone = m.KWTphone,
                Notes = m.Notes,
                PatientFName = m.PatientFName,
                PatientLName = m.PatientLName,
                PatientMName = m.PatientMName,
                PatientCID = m.PatientCID,
                USPhone = m.USphone

            }).ToList();
        }

        public PatientModel GetPatient(string patientcid)
        {
            
            var p = _domainObjectRepository.Get<Patient>(e => e.PatientCID == patientcid, 
                                            new [] {"Bank","Agency","Doctor","Hospital"});
            if (p != null)
            {
                return new PatientModel()
                {
                    Doctor = p.Doctor?.DoctorName,
                    BankName = p.Bank?.BankName,
                    Hospital = p.Hospital?.HospitalName,
                    Agency = p.Agency.AgencyName,
                    BankCode = p.Bank?.BankCode,
                    Iban = p.Iban,
                    IsActive = p.IsActive,
                    EndTreatDate = p.EndTreatDate,
                    FirstApptDAte = p.FirstApptDate,
                    IsBeneficiary = p.IsBeneficiary,
                    KWTPhone = p.KWTphone,
                    Notes = p.Notes,
                    PatientFName = p.PatientFName,
                    PatientLName = p.PatientLName,
                    PatientMName = p.PatientMName,
                    PatientCID = p.PatientCID,
                    USPhone = p.USphone
                };
            }

            else
                throw new Exception("There is no patients with this CID: "+ patientcid);
        }

        public List<CompanionModel> GetPatientCompanions(string patientcid)
        {
            
            var p = _domainObjectRepository.Get<Patient>(e => e.PatientCID == patientcid);
            List<CompanionModel> companionList = new List<CompanionModel>();
            if (p != null)
            {
                var companions =   _domainObjectRepository.Filter<Companion> (e => e.PatientCID == patientcid, new[] { "CompanionHistories" });
                companionList = companions.Select(m=>new CompanionModel()
                {
                 CompanionCID   = m.CompanionCID,
                 CompanionFName = m.CompanionFName,
                 CompanionLName = m.CompanionLName,
                 CompanionMName = m.CompanionMName,
                 IsActive = m.IsActive,
                 IsBeneficiary = m.IsBeneficiary,
                 CreatedBy = m.CreatedBy,
                 CreatedDate = m.CreatedDate,
                 DateIn = m.DateIn,
                 DateOut = m.DateOut
                }).ToList();
            }
            return companionList;
        }
    }
}
