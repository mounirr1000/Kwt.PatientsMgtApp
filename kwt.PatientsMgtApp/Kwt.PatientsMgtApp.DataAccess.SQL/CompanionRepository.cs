using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class CompanionRepository : ICompanionRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

        public CompanionRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<CompanionModel> GetCompanions()
        {
            var result = _domainObjectRepository.All<Companion>().ToList();
            //Map to returnable object
            //var bank = _domainObjectRepository.All<Bank>(new[] { "Beneficiary", "Companion", "Patient", "PayRate" }).ToList();
            return result.Select(c => new CompanionModel()
            {
                CompanionCID = c.CompanionCID,
                BankName = _domainObjectRepository.Get<Bank>(b => b.BankID == c.BankID)?.BankName,
                CompanionFName = c.CompanionFName,
                CompanionMName = c.CompanionMName,
                CompanionLName = c.CompanionLName,
                CompanionType = _domainObjectRepository.Get<CompanionType>(ct => ct.CompanionTypeID == c.CompanionTypeID)?.CompanionType1,
                DateIn = c.DateIn,
                DateOut = c.DateOut,
                IsActive = c.IsActive == true ? true : false,
                IBan = c.IBan,
                BankCode = _domainObjectRepository.Get<Bank>(b => b.BankID == c.BankID)?.BankCode,
                IsBeneficiary = c.IsBeneficiary == true ? true : false,
                Notes = c.Notes,
                PatientCID = c.PatientCID,
                CreatedBy = c.CreatedBy,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate,
                Id = c.Id,
                ModifiedBy = c.ModifiedBy
            }).ToList();
        }

        public CompanionModel GetCompanion(string companioncid)
        {
            CompanionModel coModel;
            var companion = _domainObjectRepository.Get<Companion>(c => c.CompanionCID == companioncid);
            if (companion != null)
            {
                coModel = new CompanionModel()
                {
                    CompanionCID = companion.CompanionCID,
                    BankName = _domainObjectRepository.Get<Bank>(b => b.BankID == companion.BankID)?.BankName,
                    CompanionFName = companion.CompanionFName,
                    CompanionMName = companion.CompanionMName,
                    CompanionLName = companion.CompanionLName,
                    CompanionType = _domainObjectRepository.Get<CompanionType>(ct => ct.CompanionTypeID == companion.CompanionTypeID)?.CompanionType1,
                    DateIn = companion.DateIn,
                    DateOut = companion.DateOut,
                    IsActive = companion.IsActive == true ? true : false,
                    IBan = companion.IBan,
                    BankCode = _domainObjectRepository.Get<Bank>(b => b.BankID == companion.BankID)?.BankCode,
                    IsBeneficiary = companion.IsBeneficiary == true ? true : false,
                    Notes = companion.Notes,
                    PatientCID = companion.PatientCID,
                    CreatedBy = companion.CreatedBy,
                    CreatedDate = companion.CreatedDate,
                    ModifiedDate = companion.ModifiedDate,
                    Id = companion.Id,
                    ModifiedBy = companion.ModifiedBy
                };
            }
            else
            {
                coModel = new CompanionModel();

            }
            return coModel;
        }

        public CompanionModel GetPatientByCompanionCid(string companioncid)
        {
            return null;
        }
        public List<Companion> GetCompanionListByPatientCid(string patientcid)
        {
            return _domainObjectRepository.All<Companion>().Where(c => c.PatientCID == patientcid).ToList();
        }

        public Companion GetCompanionByPatientCid(string patientcid)
        {
            return _domainObjectRepository.Get<Companion>(c=>c.PatientCID==patientcid && c.CompanionTypeID == (int)Enums.CompanionType.Primary);
        }
        private void CheckCompanionType(List<Companion> companionList, int? newCompanionTypeid)
        {
            if (companionList.Any(comp => comp.CompanionTypeID == newCompanionTypeid))
            {
                throw new PatientsMgtException(1, "error", "Creating new Companion",
                    "You can't have two companions as primary type associated to the same user");
            }
        }
        public void AddCompanion(CompanionModel companion)
        {
            var existingRecord =
                _domainObjectRepository.Get<Companion>(
                    c => c.CompanionCID == companion.CompanionCID && c.PatientCID == companion.PatientCID);
            if (existingRecord != null)
            {
                throw new PatientsMgtException(1, "error", "Create new Companion", "There is aleady a record with the same companion and patient");
            }
            if (!String.IsNullOrEmpty(companion.PatientCID))
            {
                var newCompanionTypeid = _domainObjectRepository.Get<CompanionType>(ct => ct.CompanionType1 == companion.CompanionType)?
                    .CompanionTypeID;
                #region Validate companion Type
                //check if already there is a newCompanion associated with the pationcid in table who is primary
                //we should not have two companions with primary type account
                CompanionRepository companionRepo = new CompanionRepository();
                var companionList = companionRepo.GetCompanionListByPatientCid(companion.PatientCID);
                if (companionList != null && companionList.Count > 0)//patient is already in table with other newCompanion
                {
                    //check if this other companion with the same patient that are primary
                    if (newCompanionTypeid == (int)Enums.CompanionType.Primary)
                        CheckCompanionType(companionList, newCompanionTypeid);
                }
                //
                #endregion
                var patient = _domainObjectRepository.Get<Patient>(p => p.PatientCID == companion.PatientCID);
                if (patient != null)
                {
                    #region validate beneficiary
                    CheckBeneficiary(patient, companion, companionList, newCompanionTypeid);
                    #endregion

                    Companion newCompanion = new Companion()
                    {
                        CompanionCID = companion.CompanionCID,
                        CompanionFName = companion.CompanionFName,
                        CompanionMName = companion.CompanionMName,
                        CompanionLName = companion.CompanionLName,
                        CompanionTypeID = newCompanionTypeid,
                        DateIn = companion.DateIn,
                        DateOut = companion.DateOut,
                        IsActive = companion.IsActive, // == "Yes" ? true : false,
                        IBan = companion.IBan,
                        BankID = _domainObjectRepository.Get<Bank>(b => b.BankName == companion.BankName)?.BankID,
                        IsBeneficiary = companion.IsBeneficiary, // == "Yes" ? true : false,
                        Notes = companion.Notes,
                        PatientCID = companion.PatientCID,
                        CreatedBy = companion.CreatedBy
                    };
                    // check if the user entered a bank info when the companion is set as primary and beneficiary

                    _domainObjectRepository.Create<Companion>(newCompanion);
                    // once we created the Companion with a primary type companiontype, we need to call a method that does an insert into Beneficiary table
                    // if the user is not primary then should not be beneficiary
                    if (newCompanionTypeid == (int)Enums.CompanionType.Primary)
                    {
                        // this Beneficiary can be a ptient himself if he is Beneficiary by default
                        InsertIntoBeneficiaryTable(patient, newCompanion);
                    }
                }
            }

        }
        //Create a method to check which of the two newCompanion or patient is Benificiary
        private static void CheckBeneficiary(Patient patient, CompanionModel newCompanion, List<Companion> existingCompanion, int? companionTypeId)
        {
            //DomainObjectRepository newDomain = new DomainObjectRepository();
            //var companionTypeId = newDomain.Get<CompanionType>(ct => ct.CompanionType1 == newCompanion.CompanionType).CompanionTypeID;
            if (patient.IsBeneficiary == true &&
                (patient.IsBeneficiary == newCompanion.IsBeneficiary
                && companionTypeId == (int)Enums.CompanionType.Primary))
            {
                throw new PatientsMgtException(1, "error", "Creating new Companion",
                    "The Patient is already benificiary!! " +
                    "You can't have the companion as benificiary");
            }
            if (patient.IsBeneficiary == false &&
                (patient.IsBeneficiary == newCompanion.IsBeneficiary
                && companionTypeId == (int)Enums.CompanionType.Primary))
                throw new PatientsMgtException(1, "error", "Creating new Companion", "You need to set either the patient or the Companion as Beneficiary ");
            // only one Companion asscoiated to the patient should be benificiary and not more
            var duplicateComp = new Companion();

            duplicateComp = existingCompanion.SingleOrDefault(c => c.CompanionCID == newCompanion.CompanionCID);
            if (duplicateComp != null)
            {
                existingCompanion.Remove(duplicateComp);
            }
            if (newCompanion.IsBeneficiary == true &&
                existingCompanion != null &&
                existingCompanion.Any(comp => comp.IsBeneficiary == newCompanion.IsBeneficiary))
            {
                throw new PatientsMgtException(1, "error", "Creating new Companion",
                    "There is already one companion associated with the patient declared as beneficiary!!" +
                    "\r\n Only one companion should be beneficiary");
            }

        }
        //Create a method to insert the beneficiary into Benificiary table

        private void InsertIntoBeneficiaryTable(Patient patient, Companion companion)
        {
            Beneficiary beneficiary;
            // query the table to see if there is a record with the same patient CID

            var ben = _domainObjectRepository.Get<Beneficiary>(m => m.PatientCID == patient.PatientCID);

            if (patient?.IsBeneficiary == true)
            {
                beneficiary = new Beneficiary()
                {
                    BankID = patient.BankID,
                    PatientCID = patient.PatientCID,
                    IBan = patient.Iban,
                    BeneficiaryCID = patient.PatientCID,
                    BeneficiaryFName = patient.PatientFName,
                    BeneficiaryLName = patient.PatientLName,
                    BeneficiaryMName = patient.PatientMName,
                    CompanionCID = companion.CompanionCID,

                };
                // if there is a record but the patient is not the beneficiary we do update
                if (ben != null && ben.BeneficiaryCID != patient.PatientCID)
                {
                    //do update
                    ben.BeneficiaryCID = patient.PatientCID;
                    ben.BeneficiaryFName = patient.PatientFName;
                    ben.BeneficiaryLName = patient.PatientLName;
                    ben.BeneficiaryMName = patient.PatientMName;
                    ben.BankID = patient.BankID;
                    ben.IBan = patient.Iban;
                    if (companion.CompanionTypeID == (int)Enums.CompanionType.Primary)
                    {
                        ben.CompanionCID = companion.CompanionCID;
                    }
                    _domainObjectRepository.Update<Beneficiary>(ben);
                }
                else if (ben != null && ben.BeneficiaryCID == patient.PatientCID)
                {
                    // just update the companion id when this companion is primary
                    if (companion.CompanionTypeID == (int)Enums.CompanionType.Primary)
                    {
                        ben.CompanionCID = companion.CompanionCID;
                        _domainObjectRepository.Update<Beneficiary>(ben);
                    }
                }
                // there is no record in the table, so create one
                else if (ben == null)
                    _domainObjectRepository.Create<Beneficiary>(beneficiary);
            }
            else if (companion.IsBeneficiary == true)
            {
                beneficiary = new Beneficiary()
                {
                    BankID = companion.BankID,
                    PatientCID = companion.PatientCID,
                    IBan = companion.IBan,
                    BeneficiaryCID = companion.CompanionCID,
                    BeneficiaryFName = companion.CompanionFName,
                    BeneficiaryLName = companion.CompanionLName,
                    BeneficiaryMName = companion.CompanionMName,
                    CompanionCID = companion.CompanionCID,

                };
                // if there is a record but the patient is not the beneficiary we do update
                if (ben != null && ben.BeneficiaryCID != companion.CompanionCID)
                {
                    //do update
                    ben.BeneficiaryCID = companion.CompanionCID;
                    ben.BeneficiaryFName = companion.CompanionFName;
                    ben.BeneficiaryLName = companion.CompanionLName;
                    ben.BeneficiaryMName = companion.CompanionMName;
                    ben.BankID = companion.BankID;
                    ben.IBan = companion.IBan;
                    ben.CompanionCID = companion.CompanionCID;
                    _domainObjectRepository.Update<Beneficiary>(ben);
                }
                // there is no record in the table, so create one
                else if (ben == null)
                    _domainObjectRepository.Create<Beneficiary>(beneficiary);
            }

        }

        private void InsertIntoCompanionHistoryTable(Companion companion)
        {
            if (companion.IsActive == false && companion.DateOut != null)
            {
                // check that the companion is not in the table with the same patient
                // we can have more than one history for the same companion if the patient is diffrent
                var comp = _domainObjectRepository.Get<CompanionHistory>(c => c.CompanionCID == companion.CompanionCID
                            && c.PatientCID == companion.PatientCID);

                if (comp == null)
                {
                    _domainObjectRepository.Create<CompanionHistory>(new CompanionHistory()
                    {
                        CompanionCID = companion.CompanionCID,
                        DateIn = companion.DateIn,
                        DateOut = companion.DateOut,
                        PatientCID = companion.PatientCID,
                    });

                }

            }
        }
        public CompanionModel UpdateCompanion(CompanionModel companion)
        {
            var companionToUpdate = _domainObjectRepository.Get<Companion>(c => c.CompanionCID == companion.CompanionCID
            && c.PatientCID == companion.PatientCID);
            var newCompanionTypeid =
                _domainObjectRepository.Get<CompanionType>(ct => ct.CompanionType1 == companion.CompanionType)?
                    .CompanionTypeID;
            if (companionToUpdate != null)
            {
                companionToUpdate.CompanionCID = companion.CompanionCID;
                companionToUpdate.BankID = _domainObjectRepository.Get<Bank>(b => b.BankName == companion.BankName)?.BankID;
                companionToUpdate.CompanionFName = companion.CompanionFName;
                companionToUpdate.CompanionMName = companion.CompanionMName;
                companionToUpdate.CompanionLName = companion.CompanionLName;
                companionToUpdate.CompanionTypeID = newCompanionTypeid;
                companionToUpdate.DateIn = companion.DateIn;
                companionToUpdate.DateOut = companion.DateOut;
                companionToUpdate.IsActive = companion.IsActive;
                companionToUpdate.IBan = companion.IBan;
                companionToUpdate.BankID = _domainObjectRepository.Get<Bank>(b => b.BankName == companion.BankName)?.BankID;
                companionToUpdate.IsBeneficiary = companion.IsBeneficiary;
                companionToUpdate.Notes = companion.Notes;
                companionToUpdate.PatientCID = companion.PatientCID;
                companionToUpdate.ModifiedBy = companion.ModifiedBy;

                if (companion.IsActive == false && companion.DateOut == null)
                {
                    companionToUpdate.DateOut = DateTime.Now;
                }
                else if (companion.IsActive == true)
                {
                    companionToUpdate.DateOut = null;
                }
                // check if the patient is not beneficiary before doing the update to a beneficiary companion
                var patient = _domainObjectRepository.Get<Patient>(p => p.PatientCID == companion.PatientCID);
                CompanionRepository companionRepo = new CompanionRepository();
                var companionList = companionRepo.GetCompanionListByPatientCid(companion.PatientCID);

                // Assert that the user make an update according  to the rules
                CheckBeneficiary(patient, companion, companionList, newCompanionTypeid);
                // no 2 companions should be beneficiary or confelect with the patient
                CheckCompanionType(companionList, newCompanionTypeid); // No 2 companions should have primary as a type
                if (companionToUpdate.IsBeneficiary == true &&
                    (companionToUpdate.Bank == null || String.IsNullOrEmpty(companionToUpdate.IBan)))
                {
                    throw new PatientsMgtException(1, "error", "Update a Companion",
                        "You must eneter the bank info! since this companion is beneficiary");
                }
                if (companionToUpdate.IsBeneficiary == true &&
                    companionToUpdate.IsActive == false)
                {
                    throw new PatientsMgtException(1, "error", "Update a Companion",
                        "The companion should be active since is beneficiary");
                }
                if (companionToUpdate.IsBeneficiary == true &&
                        companionToUpdate.CompanionTypeID == (int)Enums.CompanionType.Secondary)
                {
                    throw new PatientsMgtException(1, "error", "Update a Companion",
                        String.Format("The companion should not be {0} since is beneficiary", companionToUpdate?.CompanionType?.CompanionType1));
                }
                if (companionToUpdate.IsActive == false && companionToUpdate.DateOut == null)
                {
                    throw new PatientsMgtException(1, "error", "Update a Companion",
                        String.Format("You have set the companion as inactive, so the date out should be set", companionToUpdate?.CompanionType?.CompanionType1));
                }
                // whene every rule is complied with, we do an update
                _domainObjectRepository.Update<Companion>(companionToUpdate);

                //insert into history when the companion is not active, or date out is set
                if (companionToUpdate.IsActive == false && companionToUpdate.DateOut != null)
                {
                    InsertIntoCompanionHistoryTable(companionToUpdate);
                }
                //Update the beneficiary table
                var ben = _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == companion.PatientCID);
                if (ben != null)
                {
                    if (companion.IsBeneficiary == true &&
                        ben.BeneficiaryCID != companion.CompanionCID)
                    {
                        ben.BeneficiaryCID = companion.CompanionCID;
                        ben.BeneficiaryFName = companion.CompanionFName;
                        ben.BeneficiaryLName = companion.CompanionLName;
                        ben.BeneficiaryMName = companion.CompanionMName;
                        ben.BankID = _domainObjectRepository.Get<Bank>(b => b.BankName == companion.BankName).BankID;
                        ben.IBan = companion.IBan;
                        _domainObjectRepository.Update<Beneficiary>(ben);
                    }
                    if (companion.IsBeneficiary == false &&
                        ben.BeneficiaryCID == companion.CompanionCID)
                    {
                        if (patient.IsBeneficiary == false)
                        {
                            throw new PatientsMgtException(1, "Error", "Update Companion",
                                "Neither the patient or the companion are beneficiary, Make one of them beneficiary to make an update");
                        }
                        else
                        {
                            ben.BeneficiaryCID = patient.PatientCID;
                            ben.BeneficiaryFName = patient.PatientFName;
                            ben.BeneficiaryLName = patient.PatientLName;
                            ben.BeneficiaryMName = patient.PatientMName;
                            ben.BankID = patient.BankID;
                            ben.IBan = patient.Iban;
                            _domainObjectRepository.Update<Beneficiary>(ben);
                        }
                    }
                }
            }

            return companion;
        }

        public int DeleteCompanion(string companionCid, string patientCid)
        {
            var comp = _domainObjectRepository.Get<Companion>(c => c.CompanionCID == companionCid
                                                          && c.PatientCID == patientCid);
            int index = 0;
            if (comp != null)
            {
                var ben = _domainObjectRepository.Get<Beneficiary>(c => c.CompanionCID == companionCid
                                                                      && c.PatientCID == patientCid);
                if (ben != null)
                {

                    if (ben.BeneficiaryCID == companionCid)
                    {
                        ben.BeneficiaryCID = null;
                        ben.BeneficiaryLName = null;
                        ben.BeneficiaryLName = null;
                        ben.BeneficiaryMName = null;
                        ben.IBan = null;
                        ben.IBan = null;
                        ben.CompanionCID = null;
                    }
                    else
                    {
                        ben.CompanionCID = null;
                    }
                    _domainObjectRepository.Update<Beneficiary>(ben);
                }
                //delete from companion history
                _domainObjectRepository.Delete<CompanionHistory>(ch => ch.CompanionCID == companionCid);
                index = _domainObjectRepository.Delete<Companion>(comp);
            }
            return index;
        }

        // Data Migration to insert into Beneficiary table 
        public void DataMigrationToInsertIntoBeneficiaryTable()
        {
            var comapnions = _domainObjectRepository.All<Companion>().ToList();
            var patients = _domainObjectRepository.All<Patient>().ToList();
            var beneficiaries = _domainObjectRepository.All<Beneficiary>().ToList();


            List<Companion> compToAdd = comapnions.Where(comp => comp.IsBeneficiary == true && comp.CompanionTypeID == (int) Enums.CompanionType.Primary)
                                    .Where(comp => !beneficiaries.Any(b => b.BeneficiaryCID == comp.CompanionCID)).ToList();
            List<Patient> patToAdd = patients.Where(pat => pat.IsBeneficiary == true)
                                    .Where(pat => !beneficiaries.Any(b => b.BeneficiaryCID == pat.PatientCID)).ToList();

            // Do the insert
            IList<Beneficiary> insertedBens = new List<Beneficiary>();
            IList<Beneficiary> bens = (from pat in patToAdd
                let companionCid = GetCompanionByPatientCid(pat.PatientCID)
                select new Beneficiary()
                {
                    BankID = pat.BankID,
                    BeneficiaryCID = pat.PatientCID,
                    BeneficiaryFName = pat.PatientFName,
                    BeneficiaryLName = pat.PatientLName,
                    BeneficiaryMName = pat.PatientMName,
                    CompanionCID = companionCid?.CompanionCID,
                    PatientCID = pat.PatientCID, IBan = pat.Iban,
                }).ToList();
            if (bens.Count > 0)
            {
                insertedBens= _domainObjectRepository.CreateBulk<Beneficiary>(bens);
            }

            //Delete Duplicate inserted
            bens = new List<Beneficiary>();
            bens = (from comp in compToAdd
                where insertedBens.All(b => b.PatientCID != comp.PatientCID)
                select new Beneficiary()
                {
                    BankID = comp.BankID,
                    BeneficiaryCID = comp.CompanionCID,
                    BeneficiaryFName = comp.CompanionFName,
                    BeneficiaryLName = comp.CompanionLName,
                    BeneficiaryMName = comp.CompanionMName,
                    CompanionCID = comp.CompanionCID,
                    PatientCID = comp.PatientCID,
                    IBan = comp.IBan,
                }).ToList();
            if (bens.Count > 0  )
            {
                _domainObjectRepository.CreateBulk<Beneficiary>(bens);
            }
            
            
        }
    }
}
