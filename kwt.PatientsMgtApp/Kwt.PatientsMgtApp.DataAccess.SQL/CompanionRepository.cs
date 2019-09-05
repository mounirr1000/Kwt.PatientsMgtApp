using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

            var result = _domainObjectRepository.All<Companion>(new[] { "Bank", "CompanionType" }).ToList();
            //Map to returnable object
            //var bank = _domainObjectRepository.All<Bank>(new[] { "Beneficiary", "Companion", "Patient", "PayRate" }).ToList();
            return result.Select(c => new CompanionModel()
            {
                CompanionCID = c.CompanionCID,
                BankName = c.Bank?.BankName,// _domainObjectRepository.Get<Bank>(b => b.BankID == c.BankID)?.BankName,
                CompanionFName = c.CompanionFName,
                CompanionMName = c.CompanionMName,
                CompanionLName = c.CompanionLName,
                CompanionType = c.CompanionType.CompanionType1,//, _domainObjectRepository.Get<CompanionType>(ct => ct.CompanionTypeID == c.CompanionTypeID)?.CompanionType1,
                DateIn = c.DateIn,
                DateOut = c.DateOut,
                IsActive = c.IsActive == true,
                IBan = c.IBan,
                BankCode = c.Bank?.BankName,//_domainObjectRepository.Get<Bank>(b => b.BankID == c.BankID)?.BankCode,
                IsBeneficiary = c.IsBeneficiary == true,
                Notes = c.Notes,
                PatientCID = c.PatientCID,
                CreatedBy = c.CreatedBy,
                CreatedDate = c.CreatedDate,
                ModifiedDate = c.ModifiedDate,
                Id = c.Id,
                ModifiedBy = c.ModifiedBy,
                JustBeneficiary = c.justBeneficiary
            }).ToList();
        }

        public CompanionModel GetCompanion(string companioncid)//we should check patient cid
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
                    ModifiedBy = companion.ModifiedBy,
                    JustBeneficiary = companion.justBeneficiary
                };
            }
            else
            {
                coModel = null;

            }
            return coModel;
        }

        public CompanionModel GetCompanion(string companioncid, string patientCid)
        {
            CompanionModel coModel;
            var companion = _domainObjectRepository.Get<Companion>(c => c.CompanionCID == companioncid && c.PatientCID == patientCid);
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
                    ModifiedBy = companion.ModifiedBy,
                    JustBeneficiary = companion.justBeneficiary
                };
            }
            else
            {
                coModel = null;

            }
            return coModel;
        }

        public CompanionModel GetCompanion(int id)
        {
            CompanionModel coModel;
            var companion = _domainObjectRepository.Get<Companion>(c => c.Id == id);
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
                    ModifiedBy = companion.ModifiedBy,
                    JustBeneficiary = companion.justBeneficiary
                };
            }
            else
            {
                coModel = null;

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
            return _domainObjectRepository.Get<Companion>(c => c.PatientCID == patientcid && c.CompanionTypeID == (int)Enums.CompanionType.Primary);
        }
        private void CheckCompanionType(List<Companion> companionList, int? newCompanionTypeid)
        {
            if (companionList.Any(comp => comp.CompanionTypeID == newCompanionTypeid))
            {
                throw new PatientsMgtException(1, "error", "Creating/updating Companion",
                    "You can't have two companions as primary associated to the same patient");
            }
        }
        public void AddCompanion(CompanionModel companion)
        {
            var existingRecord =
                _domainObjectRepository.Get<Companion>(
                    c => c.CompanionCID == companion.CompanionCID && c.PatientCID == companion.PatientCID);
            if (existingRecord != null)
            {
                throw new PatientsMgtException(1, "error", "Create new Companion", "There is aleady a record with the same companion and patient CID");
            }
            if (companion.CompanionCID == companion.PatientCID)
            {
                if (!HttpContext.Current.User.IsInRole(Roles.SuperAdmin))
                    throw new PatientsMgtException(1, "error", "Create new Companion", "Companion and patient have the same CID, If this is what you want, Let your Super Admin create this for you");
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
                        CheckCompanionType(companionList.Where(c => c.IsActive == true)?.ToList(), newCompanionTypeid);
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
                        CreatedBy = companion.CreatedBy,
                        justBeneficiary = companion.JustBeneficiary
                    };
                    // check if the user entered a bank info when the companion is set as primary and beneficiary

                    _domainObjectRepository.Create<Companion>(newCompanion);
                    // once we created the Companion with a primary type companiontype, we need to call a method that does an insert into Beneficiary table
                    // if the user is not primary then should not be beneficiary
                    if ((newCompanionTypeid == (int)Enums.CompanionType.Primary)
                        && newCompanion.IsActive == true)
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
                && companionTypeId == (int)Enums.CompanionType.Primary)
                //new 
                && newCompanion.IsActive
                )
            {
                throw new PatientsMgtException(1, "error", "Creating/updating Companion",
                    "The Patient is already benificiary!! " +
                    "You can't have the companion as benificiary");
            }
            if (patient.IsBeneficiary == false &&
                (patient.IsBeneficiary == newCompanion.IsBeneficiary
                && companionTypeId == (int)Enums.CompanionType.Primary)
                //new 
                && newCompanion.IsActive
                )
                throw new PatientsMgtException(1, "error", "Creating/updating Companion", "The Patient is not beneficiary, You need to set either the patient or the Companion as Beneficiary ");
            // only one Companion asscoiated to the patient should be benificiary and not more
            var duplicateComp = new Companion();

            duplicateComp = existingCompanion.SingleOrDefault(c => c.CompanionCID == newCompanion.CompanionCID);
            if (duplicateComp != null)
            {
                existingCompanion.Remove(duplicateComp);
            }
            if (newCompanion.IsBeneficiary == true &&
                existingCompanion != null &&
                existingCompanion.Any(comp => comp.IsBeneficiary == newCompanion.IsBeneficiary && comp.IsActive == true)
                //new 
                && newCompanion.IsActive
                )
            {
                throw new PatientsMgtException(1, "error", "Creating/updating Companion",
                    "There is already one companion associated with the patient declared as beneficiary!! " +
                    "Only one companion should be beneficiary");
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
                    if (companion.CompanionTypeID == (int)Enums.CompanionType.Primary && companion.IsActive == true)
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
                // var comp = _domainObjectRepository.Get<CompanionHistory>(c => c.CompanionCID == companion.CompanionCID
                //             && c.PatientCID == companion.PatientCID);

                //    if (comp == null)
                // {
                _domainObjectRepository.Create<CompanionHistory>(new CompanionHistory()
                {
                    CompanionCID = companion.CompanionCID,
                    DateIn = companion.DateIn,
                    DateOut = companion.DateOut,
                    PatientCID = companion.PatientCID,
                    CompanionType = _domainObjectRepository.Get<CompanionType>(ct => ct.CompanionTypeID == companion.CompanionTypeID)?.CompanionType1,
                    IsActive = companion.IsActive,
                    CreatedDate = companion.CreatedDate,
                    IsBeneficiary = companion.IsBeneficiary,
                    ModifiedBy = companion.ModifiedBy,
                    ModifiedDate = companion.ModifiedDate,
                    Name = companion.CompanionFName + " " + companion.CompanionMName + " " + companion.CompanionLName,
                    Notes = companion.Notes,
                    CreatedBy = companion.CreatedBy,

                });

                // }

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
                //companionToUpdate.BankID = _domainObjectRepository.Get<Bank>(b => b.BankName == companion.BankName)?.BankID;
                companionToUpdate.IsBeneficiary = companion.IsBeneficiary;
                companionToUpdate.Notes = companion.Notes;
                companionToUpdate.PatientCID = companion.PatientCID;
                companionToUpdate.ModifiedBy = companion.ModifiedBy;

                companionToUpdate.justBeneficiary = companion.JustBeneficiary;

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
                CheckCompanionType(companionList.Where(c => c.IsActive == true)?.ToList(), newCompanionTypeid); // No 2 companions should have primary as a type
                //if (companionToUpdate.IsBeneficiary == true &&
                //    (companionToUpdate.Bank == null || String.IsNullOrEmpty(companionToUpdate.IBan)))
                //{
                //    throw new PatientsMgtException(1, "error", "Update a Companion",
                //        "You must eneter the bank info! since this companion is beneficiary");
                //}
                //if (companionToUpdate.IsBeneficiary == true &&
                //    companionToUpdate.IsActive == false)
                //{
                //    throw new PatientsMgtException(1, "error", "Update a Companion",
                //        "The companion should be active since is beneficiary");
                //}
                //if (companionToUpdate.IsBeneficiary == true &&
                //        companionToUpdate.CompanionTypeID == (int)Enums.CompanionType.Secondary)
                //{
                //    throw new PatientsMgtException(1, "error", "Update a Companion",
                //        String.Format("The companion should not be {0} since is beneficiary", companionToUpdate?.CompanionType?.CompanionType1));
                //}
                //if (companionToUpdate.IsActive == false && companionToUpdate.DateOut == null)
                //{
                //    throw new PatientsMgtException(1, "error", "Update a Companion",
                //        String.Format("You have set the companion as inactive, so the date out should be set", companionToUpdate?.CompanionType?.CompanionType1));
                //}
                // whene every rule is complied with, we do an update

                UpdateBeneficiary(companion, companionToUpdate, patient);
                _domainObjectRepository.Update<Companion>(companionToUpdate);

                //insert into history when the companion is not active, or date out is set
                if (companionToUpdate.IsActive == false && companionToUpdate.DateOut != null)
                {
                    InsertIntoCompanionHistoryTable(companionToUpdate);
                }
                //Update the beneficiary table

            }

            return companion;
        }

        private void UpdateBeneficiary(CompanionModel companion, Companion companionToUpdate, Patient patient)
        {
            var ben = _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == companion.PatientCID);
            if (ben != null)
            {
                if (companionToUpdate.IsActive == false)// the new change to companion
                {
                    if (ben.CompanionCID == companionToUpdate.CompanionCID)
                        ben.CompanionCID = null;// set the companion cid associoted with the benefeciary to null
                    if (ben.BeneficiaryCID == companion.CompanionCID)// we have the companion as beneficiary
                    {
                        //if (patient.IsBeneficiary == false && patient.IsActive == true)// there is no beneficiary to the patient, the update should not happen
                        //    throw new PatientsMgtException(1, "Error", "Update Companion",
                        //        "the primary companion associated with the patient is no " +
                        //        "longer active, and the patient is not beneficiary, " +
                        //        "Make the patient beneficary or add a another primary companion " +
                        //        "who is beneficiary before setting this comapnion as inactive");
                        ben.BeneficiaryCID = null;
                        ben.BeneficiaryFName = null;
                        ben.BeneficiaryLName = null;
                        ben.BeneficiaryMName = null;
                        ben.BankID = null;
                        ben.IBan = null;
                        if (patient.IsBeneficiary == true)
                        {
                            ben.BeneficiaryCID = patient.PatientCID;
                            ben.BeneficiaryFName = patient.PatientFName;
                            ben.BeneficiaryLName = patient.PatientLName;
                            ben.BeneficiaryMName = patient.PatientMName;
                            ben.BankID = patient.BankID;
                            ben.IBan = patient.Iban;
                        }
                    }
                    _domainObjectRepository.Update<Beneficiary>(ben);
                    return;
                }
                if (companion.IsBeneficiary == true //&& // the companion is the benefeciary, and was not intially , patient was, or some other companion
                                                    // ben.BeneficiaryCID != companion.CompanionCID
                    )
                {

                    ben.BeneficiaryCID = companion.CompanionCID;
                    ben.BeneficiaryFName = companion.CompanionFName;
                    ben.BeneficiaryLName = companion.CompanionLName;
                    ben.BeneficiaryMName = companion.CompanionMName;
                    ben.BankID = _domainObjectRepository.Get<Bank>(b => b.BankName == companion.BankName).BankID;
                    ben.IBan = companion.IBan;
                    ben.CompanionCID = companion.CompanionCID;
                    _domainObjectRepository.Update<Beneficiary>(ben);
                    return;
                }
                if (companion.IsBeneficiary == false
                    //&& ben.BeneficiaryCID == companion.CompanionCID
                    )
                {
                    // check if there is other companion associated to this patient who is active and beneficiary and primary
                    // get the list of primary companion for the patient
                    var companionList = GetCompanionListByPatientCid(patient.PatientCID);
                    if (companionList != null)
                    {
                        var primaryCompanions = companionList.Where(c => c.CompanionTypeID == (int)Enums.CompanionType.Primary && c.IsActive == true).ToList();
                        if (primaryCompanions.Count() > 1)// more than one primary companion to the same user
                        {
                            throw new PatientsMgtException(1, "Error", "Update Companion",
                            "You can't have more than one active primary companion to the same patient, Only one primary companion can be assigned to the same patient");
                        }
                        if (patient.IsBeneficiary == false)
                        {
                            if (primaryCompanions.Any()) // means we have only one primary companion who is active
                            {

                                if (primaryCompanions.Exists(c => c.IsBeneficiary == false))
                                    // the only active primary companion with patient is not beneficiary
                                    throw new PatientsMgtException(1, "Error", "Update Companion",
                                        "You can't make the companion as not beneficiary, since the patient already not beneficiary");
                                // if we pass this far, means the companion in our records is the beneiciary
                                var compBene = primaryCompanions.FirstOrDefault();// get the only bene
                                ben.BeneficiaryCID = compBene?.PatientCID;
                                ben.BeneficiaryFName = compBene?.CompanionFName;
                                ben.BeneficiaryLName = compBene?.CompanionLName;
                                ben.BeneficiaryMName = compBene?.CompanionMName;
                                ben.BankID = compBene?.BankID;
                                ben.IBan = compBene?.IBan;
                                ben.CompanionCID = compBene?.CompanionCID;
                            }
                        }
                        else
                        {
                            ben.BeneficiaryCID = patient.PatientCID;
                            ben.BeneficiaryFName = patient.PatientFName;
                            ben.BeneficiaryLName = patient.PatientLName;
                            ben.BeneficiaryMName = patient.PatientMName;
                            ben.BankID = patient.BankID;
                            ben.IBan = patient.Iban;
                        }


                    }
                    //



                    if (companionToUpdate.IsActive == true &&
                   (companionToUpdate.CompanionTypeID ==
                    (int)Enums.CompanionType.Primary))
                    {
                        if (ben.CompanionCID != companionToUpdate.CompanionCID)
                        {
                            ben.CompanionCID = companionToUpdate.CompanionCID;
                        }

                    }
                    _domainObjectRepository.Update<Beneficiary>(ben);

                }
                //when the companion is not beneficiary but he is primary companion, 
                //the beneficiary table companion cid should be update it to take the primary comanion cid


            }
        }
        public int DeleteCompanion(string companionCid, string patientCid)
        {
            var comp = _domainObjectRepository.Get<Companion>(c => c.CompanionCID == companionCid
                                                          && c.PatientCID == patientCid, new[] { "CompanionHistories", "Payments", "PaymentDeductions" });

            int index = 0;
            if (comp != null)
            {
                //
                PaymentRepository paymentRepository = new PaymentRepository();
                var payment = paymentRepository.GetPaymentsByCompanionCid(comp.CompanionCID);
                var companionsHistory = comp.CompanionHistories;
                if (payment != null && payment.Count > 0)
                {
                    if (payment.Exists(p => p.PatientCID == comp.PatientCID))
                        throw new PatientsMgtException(1, "error", "Deleting Companion",
                    string.Format("There are payments done to this companion and patient {1}, You can't delete this Companion unless you delete all the payments asscoiated with this companion: {0}", comp.CompanionCID, comp.PatientCID));
                }
                if (companionsHistory != null && companionsHistory.Count > 0)
                {
                    var isSuperAdmin = HttpContext.Current.User.IsInRole(Roles.SuperAdmin);
                    if (!isSuperAdmin)
                        throw new PatientsMgtException(1, "error", "Deleting Patient",
                        "There are companion history with this companion, You can't delete this companion since there are history to compaion cid : " + comp.CompanionCID + ", Check with the Super Admin to confirm the delete");
                }
                //
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
                        ben.BankID = null;
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

        public int DeleteCompanion(int id)
        {
            var comp = _domainObjectRepository.Get<Companion>(c => c.Id == id);

            int index = 0;
            if (comp != null)
            {
                //
                PaymentRepository paymentRepository = new PaymentRepository();
                var payment = paymentRepository.GetPaymentsByCompanionCid(comp.CompanionCID);
                if (payment != null && payment.Count > 0)
                {
                    if (payment.Exists(p => p.PatientCID == comp.PatientCID))
                        throw new PatientsMgtException(1, "error", "Deleting Companion",
                        "There are payments done to this companion and patient, You can't delete this Companion unless you delete all the payments asscoiated with this companion: " + comp.CompanionCID);
                }
                //
                var ben = _domainObjectRepository.Get<Beneficiary>(c => c.CompanionCID == comp.CompanionCID
                                                                      && c.PatientCID == comp.PatientCID);
                if (ben != null)
                {

                    if (ben.BeneficiaryCID == comp.CompanionCID)
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
                _domainObjectRepository.Delete<CompanionHistory>(ch => ch.CompanionCID == comp.CompanionCID && ch.PatientCID == comp.PatientCID);
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


            List<Companion> compToAdd = comapnions.Where(comp => comp.IsBeneficiary == true && comp.CompanionTypeID == (int)Enums.CompanionType.Primary && comp.IsActive == true)
                                    .Where(comp => !beneficiaries.Any(b => b.BeneficiaryCID == comp.CompanionCID)).ToList();
            List<Patient> patToAdd = patients.Where(pat => pat.IsBeneficiary == true && pat.IsActive == true)
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
                                           PatientCID = pat.PatientCID,
                                           IBan = pat.Iban,
                                       }).ToList();
            if (bens.Count > 0)
            {
                insertedBens = _domainObjectRepository.CreateBulk<Beneficiary>(bens);
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
            if (bens.Count > 0)
            {
                _domainObjectRepository.CreateBulk<Beneficiary>(bens);
            }


        }
    }
}
