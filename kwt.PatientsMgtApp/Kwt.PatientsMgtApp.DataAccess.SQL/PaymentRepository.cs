using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using kwt.PatientsMgtApp.Utilities.Errors;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System.Web;
namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly ICompanionRepository _companionRepository;
        private readonly IPayRateRepository _payRateRepository;
        private readonly IDeductionReasonRepository _deductionReasonRepository;
        private readonly IRejectionReasonRepository _rejectionReasonRepository;
        private readonly IPaymentTypeRepository _paymentTypeRepository;
        private readonly IAdjustmentReasonRepository _adjustmentReasonRepository;
        public PaymentRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
            _beneficiaryRepository = new BeneficiaryRepository();
            _patientRepository = new PatientRepository();
            _companionRepository = new CompanionRepository();
            _payRateRepository = new PayRateRepository();
            _deductionReasonRepository = new DeductionReasonRepository();
            _rejectionReasonRepository = new RejectionReasonRepository();
            _paymentTypeRepository= new PaymentTypeRepository();
            _adjustmentReasonRepository= new AdjustmentReasonRepository();
        }


        //new February 28, 2019
        public List<PaymentModel> GetPaymentsWithPhone(DateTime? date)
        {
            var payments = _domainObjectRepository.Filter<Payment>(p => p.PaymentDate == date, new[] { "Patient" });

            return payments.Select(p => p.Beneficiary != null ? new PaymentModel()
            {
                PatientPhone = p.Patient.USphone ?? p.Patient.KWTphone,
                PatientCID = p.PatientCID,
                PaymentStartDate = p.StartDate,
                PaymentEndDate = p.EndDate,
                TotalDue = p.TotalDue,

            } : null).OrderBy(p => p.CreatedDate).ToList();// returns null when the beneficiary is not set and the second null is returned when the companion is not set

        }
        //
        public List<PaymentReportModel> GetPaymentsReport(string patientCid = null, DateTime? startDate = null,
            DateTime? endDate = null, int? bankId = null)
        {

            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("pCid", patientCid);
            parms.Add("startDate", startDate);
            parms.Add("endDate", endDate);
            parms.Add("bankId", bankId);

            //pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter
            var report = _domainObjectRepository.ExecuteProcedure<PaymentReportModel>("GetPaymentListReport_SP", parms, false);
            return report;
        }
        //
        public List<PaymentReportModel> GetPaymentsReportWithoutParms()
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("pCid", null);
            parms.Add("startDate", null);
            parms.Add("endDate", null);
            parms.Add("bankId", null);

            //pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter
            var report = _domainObjectRepository.ExecuteProcedure<PaymentReportModel>("GetPaymentListReport_SP", parms, false);
            return report;


        }
        public List<PaymentModel> GetPayments()
        {
            var payments = _domainObjectRepository.All<Payment>(new[] { "Beneficiary", "Patient", "PayRate", "Companion", "PaymentDeductions", "RejectedPayments", "PaymentType", "PaymentHistories" });

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
                TotalDue = p.FinalAmountAfterCorrection ?? p.TotalDue,// this the final amount
                
                Id = p.PaymentID,
                BeneficiaryCID = p.Beneficiary.BeneficiaryCID,
                PayRateID = p.PayRateID,
                PatientPhone = p.Patient.USphone ,//?? p.Patient.KWTphone,
                PaymentID = p.PaymentID,
                //new 
                PaymentTypeId = p.PaymentTypeId,
                IsRejected = p.IsRejected,
                RejectedPaymentId = p.RejectedPaymentId,
                PaymentType = p.PaymentTypeId!=null ? new PaymentTypeModel()
                {
                    PaymentType1 = p.PaymentType.PaymentType1,
                    PaymentTypeId = p.PaymentType.PaymentTypeId
                }:null,

                //end new
                // new 
                PaymentDeductionObject = p.PaymentDeductions.Select(pd => new PaymentDeductionModel()
                {
                    PatientDeduction = pd.PDeduction,
                    CompanionAmount = p.CAmount,
                    PatientAmount = p.PAmount,
                    PatientCID = pd.PatientCID,
                    ModifiedDate = pd.ModifiedDate,
                    Notes = pd.Notes,
                    FinalAmount = pd.FinalAmount,
                    CompanionDeduction = pd.CDeduction,
                    PayRateID = pd.PayRateID,
                    ModifiedBy = pd.ModifiedBy,
                    PaymentID = pd.PaymentID,
                    DeductionID = pd.DeductionID,
                    DeductionDate = pd.DeductionDate,
                    TotalDeduction = pd.TotalDeduction,
                    CreatedBy = pd.CreatedBy,
                    DeductionEndDate = pd.EndDate,
                    DeductionStartDate = pd.StartDate,
                    CreatedDate = pd.CreatedDate,
                    AmountPaid = p.TotalDue,
                    CompanionCID = pd.CompanionCID,
                    CompanionEndDate = pd.CompanionEndDate,
                    CompanionStartDate = pd.CompanionStartDate,
                    PatientEndDate = pd.PatientEndDate,
                    PatientStartDate = pd.PatientStartDate,
                    //new 
                    PatientDeductionRate = pd.PatientDeductionRate,
                    CompanionDeductionRate = pd.CompanionDeductionRate,
                    //new
                    //DeductionReasons = _deductionReasonRepository.GetDeductionReasons(),
                     DeductionReasonId = pd.ReasonId

                }).OrderBy(t => t.DeductionDate).FirstOrDefault(),

                 PaymentHistories = p.PaymentHistories.Select(ph=>new PaymentHistoryModel()
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
                     
                 }).ToList()
                //PaymentDeductionNotes = p.PaymentDeductions.Count>0 ? p.PaymentDeductions.Select(pd=>pd.Notes).FirstOrDefault():"",
            } : null).OrderBy(p => p.CreatedDate).ToList();// returns null when the beneficiary is not set and the second null is returned when the companion is not set

        }

        public PaymentModel GetPaymentObject(string patientCid)
        {
            IPaymentRepository paymentRepository = new PaymentRepository();

            var lastPayment = paymentRepository.
                                GetPaymentsByPatientCid(patientCid)?
                               .OrderByDescending(p => p.CreatedDate).ThenByDescending(p => p.PaymentID)
                               .FirstOrDefault(p=>p.PaymentTypeId   !=(int)Enums.PaymentType.Adjustment);

            PaymentModel payment = new PaymentModel();
            var ben = _beneficiaryRepository.GetBeneficiary(patientCid);
            var patient = _patientRepository.GetPatient(patientCid);
            var companion = _companionRepository.GetCompanion(ben?.CompanionCID);
            if (patient != null)
            {
                payment.IsActive = patient.IsActive;
                payment.PatientCID = patientCid;
                payment.PatientFName = patient.PatientFName;
                payment.PatientLName = patient.PatientLName;
                payment.PatientMName = patient.PatientMName;
                payment.Agency = patient.Agency;
                payment.Hospital = patient.Hospital;
                payment.HasCompanion = patient.Companions?.Count(c=>c.JustBeneficiary !=true) > 0 &&
                                       patient.Companions.Exists(c => c.IsActive && c.CompanionType == "Primary");// to be fixed, no hard coded text 'primary'
                //new 
                payment.PatientNotes = patient.Notes;

                payment.IsPatientBlocked = patient.IsBlocked;

            }

            if (patient != null && ben != null)
            {
                if (companion!=null
                    &&(companion.JustBeneficiary==null||companion.JustBeneficiary==false))
                {
                    payment.CompanionCID = ben.CompanionCID;
                    payment.CompanionFName = companion?.CompanionFName;
                    payment.CompanionLName = companion?.CompanionLName;
                    payment.CompanionMName = companion?.CompanionMName;
                } 
                payment.BeneficiaryMName = ben.BeneficiaryMName;
                payment.BeneficiaryBank = ben.BankName;
                payment.BeneficiaryIBan = ben.IBan;
                payment.BeneficiaryCID = ben.BeneficiaryCID;
                payment.BeneficiaryFName = ben.BeneficiaryFName;
                payment.BeneficiaryLName = ben.BeneficiaryLName;


                //Todo: This should be done a better way
                payment.Payments = GetPaymentsByPatientCid(patientCid)?.OrderByDescending(p => p.CreatedDate).ToList();
                payment.PayRates = _payRateRepository.GetPayRatesList();
                payment.PatientPayRate = (int)Enums.PayRates.PatientRate;//_domainObjectRepository.Get<PayRate>(c => c.PayRateID == 1).PatientRate;
                payment.CompanionPayRate = !String.IsNullOrEmpty(ben.CompanionCID) ? (int)Enums.PayRates.CompanionRate : (int)Enums.PayRates.Other;

                // Create a Payment Deduction object
                payment.PaymentDeductionObject = new PaymentDeductionModel()
                {
                    PatientCID = payment.PatientCID,
                    AmountPaid = payment.TotalDue,
                    //new
                    CompanionCID = payment.CompanionCID,
                    BeneficiaryID = payment.Beneficiary?.Id,
                    PayRateID = payment.PayRateID,
                    LastPaymentStartDate = lastPayment?.PaymentStartDate,
                    LastPaymentEndDate = lastPayment?.PaymentEndDate,
                    //new
                    DeductionReasons = _deductionReasonRepository.GetDeductionReasons()
                };
                if (lastPayment?.PaymentStartDate != null && lastPayment?.PaymentEndDate != null)
                {
                    payment.PaymentDeductionObject.LastPaymentStartDate = lastPayment?.PaymentStartDate;
                    payment.PaymentDeductionObject.LastPaymentEndDate = lastPayment?.PaymentEndDate;
                }
                //
                //new 
                payment.PatientDeductionRate = payment.PaymentDeductionObject?.PatientDeductionRate ?? (int)Enums.PayRates.PatientRate;
                payment.CompanionDeductionRate = payment.PaymentDeductionObject?.CompanionDeductionRate ?? (int)Enums.PayRates.CompanionRate;

                // new 07/13/2019
                payment.IsRejected = payment.IsRejected;
                payment.PaymentTypeId = payment.PaymentTypeId;
                payment.RejectedPaymentId = payment.RejectedPaymentId;
                payment.PaymentTypes = _paymentTypeRepository.GetPaymentTypeList();
                payment.AdjustmentReasonID = payment.AdjustmentReasonID;
                payment.AdjustmentReasons = _adjustmentReasonRepository.GetAdjustmentReasonList();
                // get the list of all payment where payments are rejected
                //select * from payments where PaymentID not in 
                //(select rejectedPaymentId from Payments where rejectedPaymentId is not null)
                var rejectedPayments = payment.Payments?.Where(p => !payment.Payments.Select(py=>py.RejectedPaymentId).Contains(p.PaymentID)).Where(p => p.IsRejected == true).ToList();
                payment.RejectedPaymentList = rejectedPayments;//payment.Payments?.Where(p=>p.IsRejected==true).ToList();
                // end new 07/13/2019
            }
            return payment;
        }

        public PaymentModel GetPaymentObject()
        {
            PaymentModel payment = new PaymentModel();
            var patients = _domainObjectRepository.Filter<Patient>(p => p.IsActive == true).ToList();
            payment.ActivePatientCidList = patients?.Select(pa => pa.PatientCID).ToList();
            return payment;
        }


        public PaymentModel GetPaymentById(int paymentid)
        {
            var payment = _domainObjectRepository.Get<Payment>(p => p.PaymentID == paymentid,
                new[] { "Beneficiary", "Patient", "PayRate", "Companion", "PaymentDeductions","RejectedPayments", "PaymentType", "PaymentHistories" });
            PaymentModel pay = new PaymentModel();
            if (payment != null)
            {   
                var lastPayment = this.
                            GetPaymentsByPatientCid(payment?.PatientCID)?
                            .OrderByDescending(p => p.PaymentDate)
                            .SkipWhile(py => py.PaymentID == payment.PaymentID)
                            .FirstOrDefault();

                var pa = payment.Patient;
                var c = payment.Companion;
                var pr = payment.PayRate;
                var be = payment.Beneficiary;

                // get payment deduction if there is one
                if (payment.PaymentDeductions != null && payment.PaymentDeductions.Count > 0)
                {

                    pay.PaymentDeductionObject = payment.PaymentDeductions.Select(pd => new PaymentDeductionModel()
                    {
                        PatientDeduction = pd.PDeduction,
                        CompanionAmount = payment.CAmount,
                        PatientAmount = payment.PAmount,
                        PatientCID = pd.PatientCID,
                        ModifiedDate = pd.ModifiedDate,
                        Notes = pd.Notes,
                        FinalAmount = pd.FinalAmount,
                        CompanionDeduction = pd.CDeduction,
                        PayRateID = pd.PayRateID,
                        ModifiedBy = pd.ModifiedBy,
                        PaymentID = pd.PaymentID,
                        DeductionID = pd.DeductionID,
                        DeductionDate = pd.DeductionDate,
                        TotalDeduction = pd.TotalDeduction,
                        CreatedBy = pd.CreatedBy,
                        DeductionEndDate = pd.EndDate,
                        DeductionStartDate = pd.StartDate,
                        CreatedDate = pd.CreatedDate,
                        AmountPaid = payment.TotalDue,
                        CompanionCID = pd.CompanionCID,
                        CompanionEndDate = pd.CompanionEndDate,
                        CompanionStartDate = pd.CompanionStartDate,
                        PatientEndDate = pd.PatientEndDate,
                        PatientStartDate = pd.PatientStartDate,
                        //
                        LastPaymentStartDate = lastPayment?.PaymentStartDate,
                        LastPaymentEndDate = lastPayment?.PaymentEndDate,
                        //
                        //new 
                        PatientDeductionRate = pd.PatientDeductionRate,
                        CompanionDeductionRate = pd.CompanionDeductionRate,
                        //new
                        DeductionReasons = _deductionReasonRepository.GetDeductionReasons(),
                        DeductionReasonId = pd.ReasonId

                    }).OrderBy(p => p.DeductionDate).FirstOrDefault();

                }
                else
                {
                    pay.PaymentDeductionObject = new PaymentDeductionModel()
                    {
                        PaymentID = payment.PaymentID,
                        PatientCID = payment.PatientCID,
                        AmountPaid = payment.TotalDue,
                        PatientAmount = payment.PAmount,
                        CompanionAmount = payment.CAmount,
                        CompanionCID = payment.CompanionCID,
                        DeductionReasons = _deductionReasonRepository.GetDeductionReasons()
                    };
                    if (lastPayment?.PaymentStartDate != null && lastPayment?.PaymentEndDate != null)
                    {
                        //only get payment where lastpaymentdate is less than current payment start date
                        if (lastPayment.PaymentEndDate < payment.StartDate)
                        {
                            pay.PaymentDeductionObject.LastPaymentStartDate = lastPayment?.PaymentStartDate;
                            pay.PaymentDeductionObject.LastPaymentEndDate = lastPayment?.PaymentEndDate;
                        }

                    }
                }
                // PaymentModel pay = new PaymentModel();
                //{

                //new 
                pay.PatientDeductionRate = pay.PaymentDeductionObject?.PatientDeductionRate ?? (int)Enums.PayRates.PatientRate;
                pay.CompanionDeductionRate = pay.PaymentDeductionObject?.CompanionDeductionRate ?? (int)Enums.PayRates.CompanionRate;

                pay.PaymentID = payment.PaymentID;
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
                pay.PaymentDate = payment.PaymentDate ?? payment.CreatedDate;
                pay.PaymentLengthPeriod = payment.Period;
                pay.PaymentStartDate = payment.StartDate;
                pay.TotalDue = payment.TotalDue;
                pay.Id = payment.PaymentID;
                pay.BeneficiaryCID = payment.Beneficiary?.BeneficiaryCID;
                pay.CompanionFName = payment.Companion?.CompanionFName;
                pay.CompanionLName = payment.Companion?.CompanionLName;
                pay.CompanionMName = payment.Companion?.CompanionMName;
                pay.HasCompanion = pa.Companions?.Count(com=> com.justBeneficiary==null|| com.justBeneficiary==false) > 0;
                // new
                pay.PaymentDeductions = payment.PaymentDeductions?.Select(pd => new PaymentDeductionModel()
                {
                    PatientDeduction = pd.PDeduction,
                    CompanionAmount = payment.CAmount,
                    PatientAmount = payment.PAmount,
                    PatientCID = pd.PatientCID,
                    ModifiedDate = pd.ModifiedDate,
                    Notes = pd.Notes,
                    FinalAmount = pd.FinalAmount,
                    CompanionDeduction = pd.CDeduction,
                    PayRateID = pd.PayRateID,
                    ModifiedBy = pd.ModifiedBy,
                    PaymentID = pd.PaymentID,
                    DeductionID = pd.DeductionID,
                    DeductionDate = pd.DeductionDate,
                    TotalDeduction = pd.TotalDeduction,
                    CreatedBy = pd.CreatedBy,
                    DeductionEndDate = pd.EndDate,
                    DeductionStartDate = pd.StartDate,
                    CreatedDate = pd.CreatedDate,
                    AmountPaid = payment.TotalDue,
                    CompanionCID = pd.CompanionCID,
                    CompanionEndDate = pd.CompanionEndDate,
                    CompanionStartDate = pd.CompanionStartDate,
                    PatientEndDate = pd.PatientEndDate,
                    PatientStartDate = pd.PatientStartDate,
                    //
                    LastPaymentStartDate = lastPayment?.PaymentStartDate,
                    LastPaymentEndDate = lastPayment?.PaymentEndDate,
                    //new
                    DeductionReasons = _deductionReasonRepository.GetDeductionReasons(),
                    DeductionReasonId = pd.ReasonId

                }).ToList();

                //
                // new 07/13/2019
                pay.IsRejected = payment.IsRejected;
                pay.PaymentTypeId = payment.PaymentTypeId;
                pay.RejectedPaymentId = payment.RejectedPaymentId;
                
                pay.RejectedPayment = payment.RejectedPayments?.Select(r => new RejectedPaymentModel()
                {
                    PaymentID=r.PaymentID,
                    CreatedBy =r.CreatedBy,
                    CreatedDate=r.CreatedDate,
                    ModifiedBy = r.ModifiedBy,
                    ModifiedDate=r.ModifiedDate,
                    RejectedDate = r.RejectedDate,
                    RejectionID = r.RejectionID,
                    RejectionNotes= r.RejectionNotes,
                    RejectionReasonID= r.RejectionReasonID
                }).SingleOrDefault();
                pay.RejectionReasons = _rejectionReasonRepository.GetRejectionReasonList();
                pay.RejectedPayments = payment.RejectedPayments?.Select(rj => new RejectedPaymentModel()
                {
                    PaymentID = rj.PaymentID,
                    CreatedDate = rj.CreatedDate,
                    CreatedBy = rj.CreatedBy,
                    ModifiedBy = rj.ModifiedBy,
                    ModifiedDate = rj.ModifiedDate,
                    RejectedDate = rj.RejectedDate,
                    RejectionID = rj.RejectionID,
                    RejectionNotes = rj.RejectionNotes,
                    RejectionReasonID = rj.RejectionReasonID,
                }).ToList();
                if (pay.RejectedPayment == null)
                {
                    pay.RejectedPayment = new RejectedPaymentModel();
                }
                // once the payment is corrected we cannot update it back to be not rejected
                var payments = GetPaymentsByPatientCid(pay.PatientCID);
                pay.Payments = payments.OrderByDescending(p=>p.PaymentID).ToList();
                if (payments != null)
                {
                    var correctedPayment = payments.SingleOrDefault(p => p.RejectedPaymentId == pay.PaymentID);
                    if (correctedPayment != null)
                    {
                        pay.IsThisPaymentCorrected = true;
                        pay.CorrectedPaymentId = correctedPayment.PaymentID;
                    }
                }
                //};

                //new payment history
                pay.PaymentHistories = payment.PaymentHistories.Select(ph => new PaymentHistoryModel()
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
                return pay;
            }

            return null;
        }

        public List<PaymentModel> GetPaymentsByPatientCid(string pacientcid)
        {
            //IPaymentRepository paymentRepo = new PaymentRepository();
            var paymentList = GetPayments();
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
                if ((currentEndDate - currentStartDate).Days < 0)
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
            //IPaymentRepository paymentRepository = new PaymentRepository();

            var lastPayment =// paymentRepository.
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
                    if (dateDiff.Days <= 0 && !HttpContext.Current.User.IsInRole(Roles.SuperAdmin)
                        //Check if the last payemt total that was made on same date is different
                       // && lastPayment.TotalDue == payment.TotalDue
                        )
                    { 
                        // new 07-17-2019
                        // sometimes they need to make payment adjustment for some repeated dates old payments
                        if(payment.PaymentTypeId ==(int)Enums.PaymentType.Regular)
                        throw new PatientsMgtException(1, "error", "Add new Payment", "Last Payment end date was on " + lastEndDate + " So payment start date should be after " + lastEndDate);
                    }

                }
            }


            Payment newPayment = new Payment()
            {
                PatientCID = payment?.PatientCID,
                AgencyID = _domainObjectRepository.Get<Agency>(a => a.AgencyName == payment.Agency)?.AgencyID,
                CAmount = payment?.CompanionAmount,
                BeneficiaryID = _domainObjectRepository.Get<Beneficiary>(b => b.PatientCID == payment.PatientCID)?.BeneficiaryID,
                CompanionCID = payment?.CompanionCID,
                CreatedBy = payment?.CreatedBy,

                Notes = payment?.Notes,
                EndDate = payment?.PaymentEndDate,
                HospitalID = _domainObjectRepository.Get<Hospital>(h => h.HospitalName == payment.Hospital)?.HospitalID,
                ModifiedBy = payment?.ModifiedBy,

                PAmount = payment?.PatientAmount,
                PayRateID = _domainObjectRepository.Get<PayRate>(pr => pr.CompanionRate == payment.CompanionPayRate
                                                && pr.PatientRate == payment.PatientPayRate)?.PayRateID,
                PaymentDate = payment?.PaymentDate ?? DateTime.Now,
                Period = payment?.PaymentLengthPeriod,
                StartDate = payment?.PaymentStartDate,
                TotalDue = payment?.TotalDue,
                //new 07-17-2019
                PaymentTypeId = payment?.PaymentTypeId,
                RejectedPaymentId = payment?.RejectedPaymentId,
                IsRejected = payment?.IsRejected,
                AdjustmentReasonID = payment?.AdjustmentReasonID,
                
            };
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {

                    newPayment.FinalAmountAfterCorrection = payment?.PaymentDeductionObject?.FinalAmount;
                    newPayment.TotalCorrection = payment?.PaymentDeductionObject?.TotalDeduction > 0
                        ? payment.PaymentDeductionObject.TotalDeduction
                        : null;
                    var createdPayment = _domainObjectRepository.Create<Payment>(newPayment);
                    //chech if there payment deduction, if so add payment deduction
                    if (payment != null && createdPayment != null)
                    {
                        if (payment.PaymentDeductionObject != null)
                        {
                            if ((payment.PaymentDeductionObject.PatientStartDate != null &&
                                payment.PaymentDeductionObject.PatientEndDate != null) ||
                                (payment.PaymentDeductionObject.CompanionStartDate != null &&
                                payment.PaymentDeductionObject.CompanionEndDate != null))
                            {
                                // means there is a change in payment,
                                //so we should create this deduction on deduction table
                                payment.PaymentID = createdPayment.PaymentID;

                                // get last payment Startdate and endDate if there is history payment
                                if (lastPayment?.PaymentStartDate != null && lastPayment?.PaymentEndDate != null)
                                {
                                    payment.PaymentDeductionObject.LastPaymentStartDate = lastPayment?.PaymentStartDate;
                                    payment.PaymentDeductionObject.LastPaymentEndDate = lastPayment?.PaymentEndDate;
                                }
                                CreateDeduction(payment.PaymentDeductionObject, payment, false);
                                // if the deduction is created successfully, i need to update the payment to set the totalpaymentafterdeduction 
                                // newPayment.FinalAmountAfterCorrection = payment.PaymentDeductionObject.FinalAmount;
                                //_domainObjectRepository.Update<Payment>(newPayment);
                            }
                        }
                        // new 8/9/2019 create paymentHistory with any new payment

                        CreatePaymentHistory(createdPayment);
                        //
                    }
                        
                    scope.Complete();
                }
                finally
                {
                    scope.Dispose();

                }

            }

        }

        private void CreatePaymentHistory(Payment p)
        {
           PaymentHistory payHistory = new PaymentHistory()
           {
               PaymentID = p.PaymentID,
               CompanionCID = p.CompanionCID,
               PatientCID = p.PatientCID,
               PayRateID = p.PayRateID,
               PAmount = p.PAmount,
               PaymentDate = p.PaymentDate,
               IsRejected = p.IsRejected,
               Period = p.Period,
               PaymentTypeId = p.PaymentTypeId,
               RejectedPaymentId = p.RejectedPaymentId,
               StartDate = p.StartDate,
               EndDate = p.EndDate,
               TotalCorrection = p.TotalCorrection,
               TotalDue = p.TotalDue,
               ModifiedBy = p.ModifiedBy,
               ModifiedDate = p.ModifiedDate,
               Notes = p.Notes,
               CAmount = p.CAmount,
              HospitalID = p.HospitalID,
              FinalAmountAfterCorrection = p.FinalAmountAfterCorrection,
              BeneficiaryID = p.BeneficiaryID,
              AgencyID = p.AgencyID,
              AdjustmentReasonID = p.AdjustmentReasonID,
              CreatedBy = p.CreatedBy,
              CreatedDate = p.CreatedDate,
              BeneficiaryBankId= p.Beneficiary?.BankID,
              BeneficiaryFName = p.Beneficiary?.BeneficiaryFName,
              BeneficiaryMName = p.Beneficiary?.BeneficiaryMName,
              BeneficiaryLName = p.Beneficiary?.BeneficiaryLName,
              
              BeneficiaryBankIban = p.Beneficiary?.IBan,
              BeneficiaryCID = p.Beneficiary?.BeneficiaryCID,
              BeneficiaryCompanionCid= p.Beneficiary?.CompanionCID,
              BeneficiaryPatientCid = p.Beneficiary?.PatientCID,
              BeneficiaryBankName = _domainObjectRepository.Get<Bank>(a => a.BankID == p.Beneficiary.BankID)?.BankName,
              BeneficiaryBankCode = _domainObjectRepository.Get<Bank>(a => a.BankID == p.Beneficiary.BankID)?.BankCode,
              
               AgencyName = _domainObjectRepository.Get<Agency>(a => a.AgencyID == p.AgencyID)?.AgencyName,
               PaymentType = _domainObjectRepository.Get<PaymentType>(a => a.PaymentTypeId == p.PaymentTypeId)?.PaymentType1,
               AdjustmentReason = _domainObjectRepository.Get<AdjustmentReason>(a => a.AdjustmentReasonID == p.AdjustmentReasonID)?.AdjustmentReason1,
               HospitalName = _domainObjectRepository.Get<Hospital>(h => h.HospitalID == p.HospitalID)?.HospitalName,
               CompanionRate = _domainObjectRepository.Get<PayRate>(pa => pa.PayRateID == p.PayRateID)?.CompanionRate,
               PatientRate = _domainObjectRepository.Get<PayRate>(pa => pa.PayRateID == p.PayRateID)?.PatientRate,
               
           };
            _domainObjectRepository.Create<PaymentHistory>(payHistory);
        }
       
        public PaymentModel UpdatePayment(PaymentModel payment)
        {
            var paymentToUpdate = _domainObjectRepository.Get<Payment>(p => p.PaymentID == payment.Id, new[] { "PaymentDeductions", "RejectedPayments" });
            if (paymentToUpdate != null)
            {
                int? payrate;
                if (payment.CompanionPayRate == null)
                {
                    payment.CompanionPayRate = 0;
                }
                payrate = _domainObjectRepository.Get<PayRate>(pr => pr.CompanionRate == payment.CompanionPayRate
                                 && pr.PatientRate == payment.PatientPayRate).PayRateID;
                //if (payment.CompanionPayRate != null)
                //    payrate = _domainObjectRepository.Get<PayRate>(pr => pr.CompanionRate == payment.CompanionPayRate
                //                                                         && pr.PatientRate == payment.PatientPayRate)
                //        .PayRateID;
                //else // there is no companion for this patient, payrates should be where patientpayratid=75 and companionpayrates = 0
                //{
                //    payrate = _domainObjectRepository.Get<PayRate>(pr => pr.PatientRate == payment.PatientPayRate&& pr.CompanionRate ==0)// hardcoded, should be fixed
                //        .PayRateID;
                //}

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
                    PayRateID = payrate,
                    PaymentDate = payment.PaymentDate,
                    Period = payment.PaymentLengthPeriod,
                    StartDate = payment.PaymentStartDate,
                    TotalDue = payment.TotalDue,
                    Id = payment.Id,
                    PaymentID = payment.Id,
                    ModifiedDate = DateTime.Now,
                    PaymentDeductions = paymentToUpdate.PaymentDeductions,
                    // if there is a deduction we get the final amount after deduction otherwise null
                    FinalAmountAfterCorrection = payment.PaymentDeductionObject.FinalAmount > 0 ? payment.PaymentDeductionObject.FinalAmount : null,
                    TotalCorrection = payment.PaymentDeductionObject.TotalDeduction > 0 ? payment.PaymentDeductionObject.TotalDeduction : null,
                    //new 07-13-2019
                    IsRejected = payment.IsRejected,
                    RejectedPaymentId = payment.RejectedPaymentId,
                    PaymentTypeId=payment.PaymentTypeId,
                    RejectedPayments = paymentToUpdate.RejectedPayments,
                    AdjustmentReasonID = payment?.AdjustmentReasonID,
                    //RejectedPayments =  new List<RejectedPayment>().Add(new RejectedPayment()
                    //{
                    //    CreatedBy = payment.RejectedPayment.CreatedBy,
                    //    RejectionID = payment.RejectedPayment.RejectionID,
                    //    PaymentID = payment.RejectedPayment.PaymentID,
                    //    RejectedDate = payment.RejectedPayment.RejectedDate,
                    //    RejectionNotes = payment.RejectedPayment.RejectionNotes,
                    //    RejectionReasonID = payment.RejectedPayment.RejectionReasonID,
                    //})
                    //new 07-17-2019

                };
                //new 07-13-2019

                AddOrUpdateRejectedPayment(updatedPayment, payment);
                //end new
                // new 8/10/2019 updatepayment History
                UpdatePaymentHistory(updatedPayment);
                //
                paymentToUpdate = updatedPayment;
                //Todo This is a temp fix for updating the Payment
                PatientsMgtEntities dbContext = new PatientsMgtEntities();
                var entry = dbContext.Entry(paymentToUpdate);
                var deduc = paymentToUpdate.PaymentDeductions.SingleOrDefault();
                if (deduc != null)
                    deduc.Payment = null;
                //new
                var reject = paymentToUpdate.RejectedPayments.SingleOrDefault();
                if (reject != null)
                    reject.Payment = null;
                //end new
                dbContext.Set<Payment>().Attach(paymentToUpdate);
                entry.State = EntityState.Modified;
                dbContext.SaveChanges();
                
                AddOrUpdatePaymentDeduction(payment, paymentToUpdate.PaymentDeductions.Count > 0);
              
                

            }
            return payment;
        }
        private void UpdatePaymentHistory(Payment p)
        {
            var payHistory = _domainObjectRepository.Get<PaymentHistory>(ph => ph.PaymentID == p.PaymentID);
            if (payHistory != null)
            {
                payHistory.PayRateID = p.PayRateID;
                payHistory.PAmount = p.PAmount;
                payHistory.PaymentDate = p.PaymentDate;
                payHistory.IsRejected = p.IsRejected;
                payHistory.Period = p.Period;
                payHistory.PaymentTypeId = p.PaymentTypeId;
                payHistory.RejectedPaymentId = p.RejectedPaymentId;
                payHistory.StartDate = p.StartDate;
                payHistory.EndDate = p.EndDate;
                payHistory.TotalCorrection = p.TotalCorrection;
                payHistory.TotalDue = p.TotalDue;
                payHistory.ModifiedBy = p.ModifiedBy;
                payHistory.ModifiedDate = p.ModifiedDate;
                payHistory.Notes = p.Notes;
                payHistory.CAmount = p.CAmount;
                payHistory.FinalAmountAfterCorrection = p.FinalAmountAfterCorrection;
                payHistory.AdjustmentReasonID = p.AdjustmentReasonID;
                payHistory.CreatedBy = p.CreatedBy;
                payHistory.CreatedDate = p.CreatedDate;
                payHistory.PaymentType =
                    _domainObjectRepository.Get<PaymentType>(a => a.PaymentTypeId == p.PaymentTypeId)?.PaymentType1;
                payHistory.AdjustmentReason =
                    _domainObjectRepository.Get<AdjustmentReason>(a => a.AdjustmentReasonID == p.AdjustmentReasonID)?
                        .AdjustmentReason1;

                payHistory.CompanionRate =
                    _domainObjectRepository.Get<PayRate>(pa => pa.PayRateID == p.PayRateID)?.CompanionRate;
                payHistory.PatientRate =
                    _domainObjectRepository.Get<PayRate>(pa => pa.PayRateID == p.PayRateID)?.PatientRate;
                _domainObjectRepository.Update<PaymentHistory>(payHistory);
            }
        }
        // new 
        public void AddOrUpdateRejectedPayment(Payment updatedPayment, PaymentModel payment)
        {
            if (updatedPayment.IsRejected == true)
            {
                if (updatedPayment.RejectedPayments?.Count == 0)
                {
                    CreateRejectedPayment(payment);
                }
                else // means there is already a record with this payment in rejectedpayment table
                {
                    var rejctedPayment = new RejectedPayment()
                    {
                        RejectionNotes = payment.RejectedPayment.RejectionNotes,
                        RejectionReasonID = payment.RejectedPayment.RejectionReasonID,
                        ModifiedBy = payment.ModifiedBy,
                        ModifiedDate = DateTime.Now,
                        CreatedDate = payment.RejectedPayment.CreatedDate,
                        PaymentID = payment.RejectedPayment.PaymentID,
                        RejectionID = payment.RejectedPayment.RejectionID,
                        CreatedBy = payment.RejectedPayment.CreatedBy,
                        RejectedDate = payment.RejectedPayment.RejectedDate

                    };
                    //_domainObjectRepository.Update<RejectedPayment>(rejctedPayment);
                    PatientsMgtEntities dbContext = new PatientsMgtEntities();
                    var entry = dbContext.Entry(rejctedPayment);
                    dbContext.Set<RejectedPayment>().Attach(rejctedPayment);
                    entry.State = EntityState.Modified;
                    dbContext.SaveChanges();

                }


            }
            else // since the payment is no longer rejected we remove old one if there is on DB
            {
                if (updatedPayment.RejectedPayments?.Count > 0)
                {
                    // this means we have a record in rejectedPayment table that need to be removed
                    _domainObjectRepository.Delete<RejectedPayment>(updatedPayment.RejectedPayments.SingleOrDefault());
                }
            }
            //end new
        }

        private void CreateRejectedPayment(PaymentModel payment)
        {
            payment.RejectedPayment.PaymentID = payment.PaymentID;
            // this means the payment was never rejected, then we do an insert into rejctedpayment table
            var rejctedPayment = new RejectedPayment()
            {
                CreatedBy = payment.CreatedBy,
                CreatedDate = DateTime.Now,
                PaymentID = payment.PaymentID,
                RejectedDate = DateTime.Now,
                RejectionNotes = payment.RejectedPayment.RejectionNotes,
                RejectionReasonID = payment.RejectedPayment.RejectionReasonID,
                Id = payment.PaymentID
            };
            //
            rejctedPayment.Payment = null;
            _domainObjectRepository.Create<RejectedPayment>(rejctedPayment);
        }

        private void AddOrUpdatePaymentDeduction(PaymentModel paymentModel, bool? isDeductionExist = false)
        {
            var deductionObject = paymentModel.PaymentDeductionObject;
            //there is already a deduction in Database with the same payment id
            if (isDeductionExist == true || paymentModel.PaymentDeductions?.Count > 0) //update
            {
                if (deductionObject != null)
                {
                    var paymentDeduction = new PaymentDeduction()
                    {
                        CDeduction = deductionObject.CompanionDeduction,
                        PDeduction = deductionObject.PatientDeduction,
                        StartDate = deductionObject.DeductionStartDate ?? paymentModel.PaymentStartDate,
                        EndDate = deductionObject.DeductionEndDate ?? paymentModel.PaymentEndDate,
                        PaymentID = deductionObject.PaymentID,
                        FinalAmount = deductionObject.FinalAmount,
                        DeductionDate = deductionObject.DeductionDate ?? paymentModel.CreatedDate,
                        Notes = deductionObject.Notes,
                        TotalDeduction = deductionObject.TotalDeduction,
                        PatientCID = deductionObject.PatientCID,
                        ModifiedBy = deductionObject.ModifiedBy,
                        ModifiedDate = DateTime.Now,
                        CompanionCID = deductionObject.CompanionCID ?? paymentModel.CompanionCID,
                        BeneficiaryID = deductionObject.BeneficiaryID ?? paymentModel.Beneficiary?.Id,
                        PayRateID = deductionObject.PayRateID ?? paymentModel.PayRateID,
                        DeductionID = deductionObject.DeductionID,
                        CreatedDate = deductionObject.CreatedDate,
                        CreatedBy = deductionObject.CreatedBy ?? paymentModel.CreatedBy,
                        AmountPaid = deductionObject.AmountPaid,
                        CompanionEndDate = deductionObject.CompanionEndDate,
                        CompanionStartDate = deductionObject.CompanionStartDate,
                        PatientEndDate = deductionObject.PatientEndDate,
                        PatientStartDate = deductionObject.PatientStartDate,
                        //new 
                        PatientDeductionRate = paymentModel.PatientDeductionRate,
                        CompanionDeductionRate = paymentModel.CompanionDeductionRate,
                        ReasonId = deductionObject.DeductionReasonId

                    };

                    // _domainObjectRepository.Update<PaymentDeduction>(paymentDeduction);
                    PatientsMgtEntities dbContext = new PatientsMgtEntities();
                    var entry = dbContext.Entry(paymentDeduction);
                    dbContext.Set<PaymentDeduction>().Attach(paymentDeduction);
                    entry.State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            }
            else
            {
                if ((deductionObject?.PatientStartDate != null && deductionObject?.PatientEndDate != null) ||
                    deductionObject?.CompanionStartDate != null && deductionObject?.CompanionEndDate != null) //add
                {

                    CreateDeduction(deductionObject, paymentModel);
                }
            }
        }

        private void CreateDeduction(PaymentDeductionModel deductionObject, PaymentModel paymentModel, bool? isEdit = true)
        {
            var paymentDeduction = new PaymentDeduction()
            {
                CDeduction = deductionObject.CompanionDeduction,
                PDeduction = deductionObject.PatientDeduction,
                StartDate = deductionObject.DeductionStartDate,
                EndDate = deductionObject.DeductionEndDate,
                PaymentID = isEdit == true ? deductionObject.PaymentID : paymentModel.PaymentID,
                FinalAmount = deductionObject.FinalAmount,
                DeductionDate = DateTime.Now,
                Notes = deductionObject.Notes,
                TotalDeduction = deductionObject.TotalDeduction,
                PatientCID = deductionObject.PatientCID,
                CreatedDate = DateTime.Now,
                // since the deduction happens during the payment update,
                // we get how did the payment update as the one who created the deuction in the next line
                CreatedBy = deductionObject.ModifiedBy,
                CompanionCID = deductionObject.CompanionCID,
                BeneficiaryID = deductionObject.BeneficiaryID,
                PayRateID = deductionObject.PayRateID,
                AmountPaid = paymentModel.TotalDue,// we get the intial payment from Payment object
                CompanionEndDate = deductionObject.CompanionEndDate,
                CompanionStartDate = deductionObject.CompanionStartDate,
                PatientEndDate = deductionObject.PatientEndDate,
                PatientStartDate = deductionObject.PatientStartDate,
                Id = paymentModel.PaymentID,
                //new 
                PatientDeductionRate = paymentModel.PatientDeductionRate,
                CompanionDeductionRate = paymentModel.CompanionDeductionRate,
                ReasonId = deductionObject.DeductionReasonId // deduction reason

            };
            _domainObjectRepository.Create<PaymentDeduction>(paymentDeduction);
        }
        public int DeletePayment(PaymentModel payment)
        {

            var pay = _domainObjectRepository.Get<Payment>(p => p.PaymentID == payment.PaymentID);
            var index = 0;
            if (pay != null)
            {
                
                // delete deduction first from paymentDeduction
                var deduction = _domainObjectRepository.Get<PaymentDeduction>(p => p.PaymentID == payment.PaymentID);
                var rejection = _domainObjectRepository.Get<RejectedPayment>(p => p.PaymentID == payment.PaymentID);
                var paymentHistory = _domainObjectRepository.Get<PaymentHistory>(p => p.PaymentID == payment.PaymentID);
                if (deduction != null)
                    _domainObjectRepository.Delete<PaymentDeduction>(deduction);
                if (rejection != null)
                    _domainObjectRepository.Delete<RejectedPayment>(rejection);
                if (paymentHistory != null)
                    _domainObjectRepository.Delete<PaymentHistory>(paymentHistory);
                index = _domainObjectRepository.Delete<Payment>(pay);
            }
            return index;
            //throw new NotImplementedException();
        }

        // new
        public List<PaymentModel> GetNextPatientPayment(int? numberOfDays = null)
        {

            Dictionary<string, object> parms = new Dictionary<string, object>();

            parms.Add("numberOfDays", numberOfDays);

            //pCidParameter, hospitalParameter, doctorParameter, statusParameter, specialityParameter
            var patientList = _domainObjectRepository.ExecuteProcedure<Payment>("GetNextPaymentPatientList_SP", parms, false);

            return patientList.Select(p => new PaymentModel()
            {
                PaymentID = p.PaymentID,
                PaymentEndDate = p.EndDate,
                PaymentStartDate = p.StartDate,
                TotalDue = p.TotalDue,
                PaymentDate = p.PaymentDate,
                PatientCID = p.PatientCID,

            }).ToList();

        }
    }
}
