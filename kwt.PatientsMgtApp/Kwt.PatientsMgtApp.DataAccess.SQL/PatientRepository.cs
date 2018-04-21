using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

        public PatientRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }

        public List<PatientReportModel> GetPatientsReport(string patientCid =null, string hospital = null, string doctor = null, Nullable<bool> status = null, string speciality = null)
        {
            Dictionary<string,object> parms = new Dictionary<string, object>();
            parms.Add("pCid", patientCid);
            parms.Add("hospital", hospital);
            parms.Add("doctor", doctor);
            parms.Add("status", status);
            parms.Add("specialty", speciality);
           
            //pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter
            return _domainObjectRepository.ExecuteProcedure<PatientReportModel>("GetPatientListReport_SP", parms, false);
            
        }
        public List<PatientModel> GetPatients()
        {
            var result = _domainObjectRepository.All<Patient>(new[] { "Doctor", "Bank", "Agency", "Hospital", "Specialty" }).ToList();
            //Map to returnable object
            return result.Select(m => new PatientModel()
            {
                Doctor = m.Doctor?.DoctorName,
                BankName = m.Bank?.BankName,
                Hospital = m.Hospital?.HospitalName,
                Agency = m.Agency?.AgencyName,
                BankCode = m.Bank?.BankCode,
                Iban = m.Iban,
                IsActive = m.IsActive == true ? true : false,
                EndTreatDate = m.EndTreatDate,
                FirstApptDAte = m.FirstApptDate,
                IsBeneficiary = m.IsBeneficiary == true ? true : false,
                KWTPhone = m.KWTphone,
                Notes = m.Notes,
                PatientFName = m.PatientFName,
                PatientLName = m.PatientLName,
                PatientMName = m.PatientMName,
                PatientCID = m.PatientCID,
                USPhone = m.USphone,
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                ModifiedDate = m.ModifiedDate,
                Diagnosis = m.Diagnosis,
                Specialty = m.Specialty?.Specialty1

            }).ToList();
        }

        public List<PatientModel> GetActivePatients()
        {
            IPatientRepository patientRepo= new PatientRepository();
            var activePatients = patientRepo.GetPatients().Where(p => p.IsActive == true).ToList();

            return activePatients;
            throw new NotImplementedException();
        }

        public List<PatientModel> GetHistoryPatients()
        {
            var patientHistory = _domainObjectRepository.All<PatientHistory>();
            return patientHistory.Select(m => new PatientModel()
            {
                //Doctor = _domainObjectRepository.Get<Doctor>(d=>d.DoctorID==m.DoctorID).DoctorName,
                //BankName = _domainObjectRepository.Get<Bank>(d => d.BankID == m.BankID).BankName,
                //Hospital = _domainObjectRepository.Get<Hospital>(d => d.HospitalID == m.HospitalID).HospitalName,
                //Agency = _domainObjectRepository.Get<Agency>(d => d.AgencyID == m.DoctorID).AgencyName,
                //BankCode = _domainObjectRepository.Get<Bank>(d => d.BankID == m.BankID).BankCode,
                Iban = m.Iban,
                IsActive = m.IsActive==true? true:false,
                EndTreatDate = m.EndTreatDate,
                FirstApptDAte = m.FirstApptDate,
                IsBeneficiary = m.IsBeneficiary == true ? true : false,
                KWTPhone = m.KWTphone,
                Notes = m.Notes,
                PatientFName = m.PatientFName,
                PatientLName = m.PatientLName,
                PatientMName = m.PatientMName,
                PatientCID = m.PatientCID,
                USPhone = m.USphone,           
                CreatedDate = m.CreatedDate,

            }).ToList();
            //throw new NotImplementedException();
        }
        public PatientModel GetPatient(string patientcid)
        {

            var p = _domainObjectRepository.Get<Patient>(e => e.PatientCID == patientcid,
                                            new[] { "Bank", "Agency", "Doctor", "Hospital", "Specialty" });
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
                    IsActive = p.IsActive == true ? true : false,
                    EndTreatDate = p.EndTreatDate,
                    FirstApptDAte = p.FirstApptDate,
                    IsBeneficiary = p.IsBeneficiary == true ? true : false,
                    KWTPhone = p.KWTphone,
                    Notes = p.Notes,
                    PatientFName = p.PatientFName,
                    PatientLName = p.PatientLName,
                    PatientMName = p.PatientMName,
                    PatientCID = p.PatientCID,
                    USPhone = p.USphone,
                    CreatedDate = p.CreatedDate,
                    CreatedBy = p.CreatedBy,
                    ModifiedDate = p.ModifiedDate,
                    ModifiedBy = p.ModifiedBy,
                    Specialty = p.Specialty?.Specialty1,
                    Diagnosis = p.Diagnosis

                };
            }

            else
                return null;
                    // throw new PatientsMgtException(1, "error", "Getting Patient with CID", "There is no patient with this Civil ID: " + patientcid);
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
                    //BankName = _domainObjectRepository.Get<Bank>(b => b.BankID == m.BankID)?.BankName,
                    CompanionFName = m.CompanionFName,
                    CompanionMName = m.CompanionMName,
                    CompanionLName = m.CompanionLName,
                    //CompanionType = _domainObjectRepository.Get<CompanionType>(ct =>  ct.CompanionTypeID == m.CompanionTypeID ).CompanionType1,
                    DateIn = m.DateIn,
                    DateOut = m.DateOut,
                    IsActive = m.IsActive == true ? true : false,
                    IBan = m.IBan,
                    //BankCode = _domainObjectRepository.Get<Bank>(b => b.BankID == m.BankID)?.BankCode,
                    IsBeneficiary = m.IsBeneficiary == true ? true : false,
                    Notes = m.Notes,
                    PatientCID = m.PatientCID,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    ModifiedDate = m.ModifiedDate,
                    Id = m.Id,
                    ModifiedBy = m.ModifiedBy
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
                        _domainObjectRepository.Get<Hospital>(a => a.HospitalName == patient.Hospital)?.HospitalID,
                    BankID = _domainObjectRepository.Get<Bank>(a => a.BankName == patient.BankName)?.BankID,
                    IsBeneficiary = patient.IsBeneficiary,
                    IsActive = patient.IsActive,
                    KWTphone = patient.KWTPhone,
                    Iban = patient.Iban,
                    DoctorID = _domainObjectRepository.Get<Doctor>(a => a.DoctorName == patient.Doctor).DoctorID,
                    Notes = patient.Notes,
                    CreatedBy = patient.CreatedBy,
                    PatientCID = patient.PatientCID?.Trim(),
                    EndTreatDate = patient.EndTreatDate,
                    PatientFName = patient.PatientFName?.Trim(),
                    PatientLName = patient.PatientLName?.Trim(),
                    PatientMName = patient.PatientMName?.Trim(),
                    FirstApptDate = patient.FirstApptDAte,
                    SpecialtyId = _domainObjectRepository.Get<Specialty>(a => a.Specialty1 == patient.Specialty).SpecialtyId,
                    Diagnosis = patient.Diagnosis
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
            else
                throw new PatientsMgtException(1, "error", "Creating new Patient", String.Format("There is a Patient with the same Patient Civil ID '{0}' already in our records!", patient.PatientCID));
        }

        public PatientModel UpdatePatient(PatientModel patient)
        {
           
            var p = _domainObjectRepository.Get<Patient>(m => m.PatientCID == patient.PatientCID);
            if (p != null)
            {
                p.FirstApptDate = patient.FirstApptDAte;
                p.IsActive = patient.EndTreatDate != null ? false : true;
                p.Notes = patient.Notes;
                p.IsBeneficiary = patient.IsBeneficiary;
                p.KWTphone = patient.KWTPhone;
                p.PatientFName = patient.PatientFName;
                p.PatientLName = patient.PatientLName;
                p.PatientMName = patient.PatientMName;
                p.Iban = patient.Iban;
                p.EndTreatDate = patient.EndTreatDate;//String.Equals(a.BankName, patient.BankName, StringComparison.OrdinalIgnoreCase)
                p.AgencyID = _domainObjectRepository.Get<Agency>(a => a.AgencyName.Trim() == patient.Agency.Trim()).AgencyID;
                p.BankID = _domainObjectRepository.Get<Bank>(a => a.BankName == patient.BankName)?.BankID;
                p.DoctorID = _domainObjectRepository.Get<Doctor>(a => a.DoctorName == patient.Doctor).DoctorID;
                p.HospitalID = _domainObjectRepository.Get<Hospital>(a => a.HospitalName == patient.Hospital)?.HospitalID;
                p.USphone = patient.USPhone;
                p.SpecialtyId =_domainObjectRepository.Get<Specialty>(a => a.Specialty1 == patient.Specialty)?.SpecialtyId;
                p.Diagnosis = patient.Diagnosis;
                _domainObjectRepository.Update<Patient>(p);
                // check if the patient become inactive and the end treatment date is not null, then the patient should become part of the history.
                if (patient.IsActive == false && patient.EndTreatDate != null)
                {
                    var patientHistory = new PatientHistory()
                    {
                        AgencyID = p.AgencyID,
                        BankID = p.BankID,
                        CreatedDate = DateTime.Now,
                        DoctorID = p.DoctorID,
                        OldCreatedDate = p.CreatedDate,
                        EndTreatDate = p.EndTreatDate,
                        FirstApptDate = p.FirstApptDate,
                        HospitalID = p.HospitalID,
                        Iban = p.Iban,
                        IsActive = false,
                        IsBeneficiary = p.IsBeneficiary,
                        KWTphone = p.KWTphone,
                        Notes = p.Notes,
                        PatientCID = p.PatientCID,
                        PatientFName = p.PatientFName,
                        PatientLName = p.PatientLName,
                        PatientMName = p.PatientMName,
                        USphone = p.USphone,
                        PrimaryCompanionCid =
                            _domainObjectRepository.Get<Companion>(c => c.PatientCID == p.PatientCID &&
                                                                        c.CompanionTypeID ==
                                                                        (int) Enums.CompanionType.Primary)?.CompanionCID,
                        
                    };
                    _domainObjectRepository.Create<PatientHistory>(patientHistory);
                }
            }
            return patient;
        }


        // ToDo : this functionality may not be needed
        public int DeletePatient(PatientModel patient)
        {
            var p = _domainObjectRepository.Get<Patient>(pa => pa.PatientCID == patient.PatientCID);
            if (p != null)
            {
                //Delete all related tables to the patient
                //delete Beneficiary record
                _domainObjectRepository.Delete<Payment>(b => b.PatientCID == patient.PatientCID);
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
