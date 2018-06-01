using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PatientHistoryRepository: IPatientHistoryRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

        public PatientHistoryRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<PatientHistoryModel> GetPatientsHistory()
        {
            var history = _domainObjectRepository.All<PatientHistory>();

            return history.Select(h => new PatientHistoryModel()
            {
                CreatedDate = h.CreatedDate,
                EndTreatDate = h.EndTreatDate,
                Iban = h.Iban,
                FirstApptDate = h.FirstApptDate,
                IsActive = h.IsActive,
                IsBeneficiary = h.IsBeneficiary,
                KWTphone = h.KWTphone,
                Notes = h.Notes,
                PatientCID = h.PatientCID,
                OldCreatedDate = h.OldCreatedDate,
                PatientFName = h.PatientFName,
                PatientMName = h.PatientMName,
                PatientLName = h.PatientLName,
                PrimaryCompanionCid = h.PrimaryCompanionCid,
                USphone = h.USphone,
                BankID = h.BankID,
                //Agency = h.AgencyID==0? _domainObjectRepository.Get<Agency>(a => a.AgencyID == h.AgencyID).AgencyName:"",
                
                //Hospital = _domainObjectRepository.Get<Hospital>(c => c.HospitalID == h.HospitalID).HospitalName,
                
                //Doctor = _domainObjectRepository.Get<Doctor>(d => d.DoctorID == h.DoctorID).DoctorName,
                

            }).ToList();

        }
    }
}
