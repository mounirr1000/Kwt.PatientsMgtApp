using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class PatientExtensionRepository : IPatientExtensionRepository
    {
        readonly private IDomainObjectRepository _domainObjectRepository;
        public PatientExtensionRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public void AddExtension(PatientExtensionModel extensionModel)
        {

            var extension = new PatientExtension()
            {
                PatientCID = extensionModel.PatientCID,
                ExtensionStartDate = extensionModel.ExtensionStartDate,
                IsPaid = extensionModel.IsPaid,
                ExtensionEndDate = extensionModel.ExtensionEndDate,
                ExtensionDocLink = extensionModel.ExtensionDocLink,
                CreatedDate = DateTime.Now,
                FileName=extensionModel.FileName

            };
            _domainObjectRepository.Create<PatientExtension>(extension);
        }

        public bool DeleteExtension(int extensionId)
        {
            var extToDelete = _domainObjectRepository.Get<PatientExtension>(e => e.ExtensionId == extensionId);
            if (extToDelete != null)
            {
                return _domainObjectRepository.Delete(extToDelete) > 0;
            }
            return false;
        }

        public PatientExtensionModel EditExtension(PatientExtensionModel extensionModel)
        {
            var extToUpdate = _domainObjectRepository.Get<PatientExtension>(e => e.ExtensionId == extensionModel.ExtensionId);
            if (extToUpdate != null)
            {
                extToUpdate.ExtensionEndDate = extensionModel.ExtensionEndDate;
                extToUpdate.ExtensionDocLink = extensionModel.ExtensionDocLink;
                extToUpdate.ExtensionStartDate = extensionModel.ExtensionStartDate;
                extToUpdate.IsPaid = extensionModel.IsPaid;
                extToUpdate.ModifiedDate = DateTime.Now;
                extToUpdate.FileName = extensionModel.FileName;
                _domainObjectRepository.Update(extToUpdate);
            }
            return extensionModel;
        }

        //public  PatientExtensionModel GetExtension()
        //{
        //    throw new NotImplementedException();
        //}

        public PatientExtensionModel GetExtension(int extensionId)
        {
            var ex = _domainObjectRepository.Get<PatientExtension>(e => e.ExtensionId == extensionId);

            if (ex != null)
                return new PatientExtensionModel()
                {
                    CreatedDate = ex.CreatedDate,
                    ExtensionId = ex.ExtensionId,
                    ExtensionDocLink = ex.ExtensionDocLink,
                    ExtensionEndDate = ex.ExtensionEndDate,
                    ExtensionStartDate = ex.ExtensionStartDate,
                    IsPaid = ex.IsPaid,
                    ModifiedDate = ex.ModifiedDate,
                    PatientCID = ex.PatientCID,
                    FileName=ex.FileName,
                };
            return null;
        }

        public List<PatientExtensionModel> GetExtensionList()
        {
            var extensions = _domainObjectRepository.All<PatientExtension>();

            var extensionList = extensions?.Select(e => new PatientExtensionModel()
            {
                CreatedDate = e.CreatedDate,
                ExtensionDocLink = e.ExtensionDocLink,
                ExtensionEndDate = e.ExtensionEndDate,
                ExtensionId = e.ExtensionId,
                ExtensionStartDate = e.ExtensionStartDate,
                ModifiedDate = e.ModifiedDate,
                PatientCID = e.PatientCID,
                IsPaid = e.IsPaid,
                FileName=e.FileName,

            }).OrderBy(ex => ex.PatientCID).ToList();
            return extensionList;
        }
        public List<PatientExtensionModel> GetOpenExtensionList()
        {
            var extensions = GetExtensionList().Where(ex => ex.IsPaid != true).ToList();
            return extensions;
        }

        public PatientExtensionModel GetPatientExtensionByCID(string patientCid)
        {
            var patExtension = GetExtensionList().Where(ex => ex.PatientCID == patientCid && ex.IsPaid != true);
            var extension = patExtension?.FirstOrDefault();
            return extension;
        }

        public PatientExtensionModel UpdatePatientExtension(PatientExtensionModel patExtension)
        {

            var updatedPatExt = _domainObjectRepository.Get<PatientExtension>(ex => ex.PatientCID == patExtension.PatientCID && ex.IsPaid != true);
            if (updatedPatExt != null)
            {
                updatedPatExt.ExtensionEndDate = patExtension.ExtensionEndDate;
                updatedPatExt.ExtensionDocLink = patExtension.ExtensionDocLink;
                updatedPatExt.ExtensionStartDate = patExtension.ExtensionStartDate;
                updatedPatExt.IsPaid = patExtension.IsPaid;
                updatedPatExt.ModifiedDate = DateTime.Now;
                updatedPatExt.FileName = patExtension.FileName;
                _domainObjectRepository.Update(updatedPatExt);
                if (patExtension.IsPaid == true)
                {
                    CreatePatientExtensionHistory(patExtension);
                   // DeleteExtension(patExtension.ExtensionId);
                }

            }
            return patExtension;
        }

        private void CreatePatientExtensionHistory(PatientExtensionModel patExt)
        {
            var extHistory = new PatientExtensionHistory()
            {
                ExtensionId = patExt.ExtensionId,
                PatientCID = patExt.PatientCID,
                IsPaid = patExt.IsPaid,
                ExtensionDocLink = patExt.ExtensionDocLink,
                ExtensionCreatedDate = patExt.CreatedDate,
                ExtensionEndDate = patExt.ExtensionEndDate,
                ExtensionStartDate = patExt.ExtensionStartDate,
                ExtensionModifiedDate = patExt.ModifiedDate,
                CreatedDate = DateTime.Now,
                FileName=patExt.FileName
            };

            _domainObjectRepository.Create<PatientExtensionHistory>(extHistory);
        }

    }
}
