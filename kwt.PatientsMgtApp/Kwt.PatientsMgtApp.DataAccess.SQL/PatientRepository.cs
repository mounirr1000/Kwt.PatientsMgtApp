using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kwt.PatientsMgtApp.Utilities.Errors;
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
            var result = _domainObjectRepository.All<Patient>(new[] { "Doctor", "Bank", "Agency", "Hospital" }).ToList();
            //Map to returnable object
            return result.Select(m => new PatientModel()
            {
                Doctor = m.Doctor?.DoctorName,
                BankName = m.Bank?.BankName,
                Hospital = m.Hospital?.HospitalName,
                Agency = m.Agency?.AgencyName,
                BankCode = m.Bank?.BankCode,
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
                USPhone = m.USphone,
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                ModifiedDate = m.ModifiedDate

            }).ToList();
        }

        public PatientModel GetPatient(string patientcid)
        {

            var p = _domainObjectRepository.Get<Patient>(e => e.PatientCID == patientcid,
                                            new[] { "Bank", "Agency", "Doctor", "Hospital" });
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
                throw new PatientsMgtException(1, "error", "Getting Patient with CID", "There is no patients with this CID: " + patientcid);
        }

        public List<CompanionModel> GetPatientCompanions(string patientcid)
        {

            var p = _domainObjectRepository.Get<Patient>(e => e.PatientCID == patientcid);
            List<CompanionModel> companionList = new List<CompanionModel>();
            if (p != null)
            {
                var companions = _domainObjectRepository.Filter<Companion>(e => e.PatientCID == patientcid, new[] { "CompanionHistories" });
                companionList = companions.Select(m => new CompanionModel()
                {
                    CompanionCID = m.CompanionCID,
                    CompanionFName = m.CompanionFName,
                    CompanionLName = m.CompanionLName,
                    CompanionMName = m.CompanionMName,
                    IsActive = m.IsActive,//==true?"Yes":"No",
                    IsBeneficiary = m.IsBeneficiary,// == true ? "Yes" : "No",
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    DateIn = m.DateIn,
                    DateOut = m.DateOut
                }).ToList();
            }
            return companionList;
        }

        public void AddPatient(PatientModel patient)
        {
            var pa = _domainObjectRepository.Get<Patient>(tp => tp.PatientCID == patient.PatientCID);

            if (pa == null)
            {
                Patient p = new Patient()
                {
                    AgencyID = _domainObjectRepository.Get<Agency>(a => a.AgencyName == patient.Agency).AgencyID,
                    USphone = patient.USPhone,
                    HospitalID =
                        _domainObjectRepository.Get<Hospital>(a => a.HospitalName == patient.Hospital).HospitalID,
                    BankID = _domainObjectRepository.Get<Bank>(a => a.BankName == patient.BankName).BankID,
                    IsBeneficiary = patient.IsBeneficiary,
                    IsActive = patient.IsActive,
                    KWTphone = patient.KWTPhone,
                    Iban = patient.Iban,
                    DoctorID = _domainObjectRepository.Get<Doctor>(a => a.DoctorName == patient.Doctor).DoctorID,
                    Notes = patient.Notes,
                    CreatedBy = patient.CreatedBy,
                    PatientCID = patient.PatientCID,
                    EndTreatDate = patient.EndTreatDate,
                    PatientFName = patient.PatientFName,
                    PatientLName = patient.PatientLName,
                    PatientMName = patient.PatientMName,

                };

                _domainObjectRepository.Create<Patient>(p);
                //Do an insert into Beneficiary table when the patient is declared as beneficiary
                var ben = _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == p.PatientCID);
                if (ben == null)
                {
                    Beneficiary beneficiary;
                    
                    if (p?.IsBeneficiary == true)
                    {

                         beneficiary = new Beneficiary()
                        {
                            PatientCID = p.PatientCID,
                            BankID = p.BankID,
                            IBan = p.Iban,
                            BeneficiaryCID = p.PatientCID,
                            BeneficiaryFName = p.PatientFName,
                            BeneficiaryLName = p.PatientLName,
                            BeneficiaryMName = p.PatientMName,
                        };
                    }
                    else
                    {
                         beneficiary = new Beneficiary()
                        {
                            PatientCID = p.PatientCID,                           
                        };
                    }
                    _domainObjectRepository.Create<Beneficiary>(beneficiary);
                }


            }
            else throw new PatientsMgtException(1, "error", "Creating new Patient", String.Format("There is a Patient with the same Patient Civil ID '{0}' already in our records!", patient.PatientCID));
        }

        public PatientModel UpdatePatient(PatientModel patient)
        {
            var p = _domainObjectRepository.Get<Patient>(m => m.PatientCID == patient.PatientCID);
            p.FirstApptDate = patient.FirstApptDAte;
            p.IsActive = patient.IsActive;
            p.Notes = patient.Notes;
            p.IsBeneficiary = patient.IsBeneficiary;
            p.KWTphone = patient.KWTPhone;
            p.PatientFName = patient.PatientFName;
            p.PatientLName = patient.PatientLName;
            p.PatientMName = patient.PatientMName;
            p.Iban = patient.Iban;
            p.EndTreatDate = patient.EndTreatDate;
            p.AgencyID = _domainObjectRepository.Get<Agency>(a => a.AgencyName == patient.Agency).AgencyID;
            p.BankID = _domainObjectRepository.Get<Bank>(a => a.BankName == patient.BankName).BankID;
            p.DoctorID = _domainObjectRepository.Get<Doctor>(a => a.DoctorName == patient.Doctor).DoctorID;
            p.HospitalID = _domainObjectRepository.Get<Hospital>(a => a.HospitalName == patient.Hospital).HospitalID;
            p.USphone = patient.USPhone;
            _domainObjectRepository.Update<Patient>(p);
            return patient;
        }

        public int DeletePatient(PatientModel patient)
        {
            var p = _domainObjectRepository.Get<Patient>(pa => pa.PatientCID == patient.PatientCID);
            if (p != null)
            {
                //Delete all related tables to the patient
                //delete Beneficiary record
                _domainObjectRepository.Delete<Beneficiary>(b => b.PatientCID == patient.PatientCID);
                _domainObjectRepository.Delete<CompanionHistory>(ch => ch.PatientCID == patient.PatientCID);
                _domainObjectRepository.Delete<Companion>(co => co.PatientCID == patient.PatientCID);
                //_domainObjectRepository.Delete<Payment>(co => co.PatientCID == patient.PatientCID);
                return _domainObjectRepository.Delete<Patient>(p);
            }
            else
                return 0;
        }
    }

}
