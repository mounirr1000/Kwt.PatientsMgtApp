using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class CompanionTypeRepository: ICompanionTypeRepository
    {
        readonly private IDomainObjectRepository _domainObjectRepository;
        public CompanionTypeRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }

        public List<CompanionTypeModel> GetCompanionTypes()
        {
            var banks = _domainObjectRepository.All<CompanionType>();

            return banks?.Select(c => new CompanionTypeModel()
            {
                CompanionType = c.CompanionType1,
                Id = c.CompanionTypeID,
               

            }).OrderBy(ct => ct.Id).ToList();
        }

        public CompanionTypeModel GetCompanionType(int companionTypeId)
        {

            var companionType = _domainObjectRepository.Get<CompanionType>(c => c.Id == companionTypeId);

            if (companionType != null)
                return new CompanionTypeModel()
                {
                    CompanionType = companionType.CompanionType1,
                    Id = companionType.CompanionTypeID,
                };
            return null;
        }

        public int DeleteCompanionType(int companionTypeId)
        {
            var companionType = _domainObjectRepository.Get<CompanionType>(c => c.Id == companionTypeId);
            if (companionType != null)
            {
                // we should remove refrenced Object in other class before do this delete
                // pending ....
                return _domainObjectRepository.Delete<CompanionType>(companionType);
            }
            return 0;
        }

        public void UpdateCompanionType(CompanionTypeModel companionType)
        {
            ICompanionTypeRepository ct = new CompanionTypeRepository();
            var oldcompanionType = ct.GetCompanionType(companionType.Id);
            var companionTypeToUpdate = new CompanionType();

            if (oldcompanionType == null) return;
            
            companionTypeToUpdate.CompanionType1 = companionType.CompanionType;
            _domainObjectRepository.Update<CompanionType>(companionTypeToUpdate);
        }

        public CompanionTypeModel AddCompanionType(CompanionTypeModel companionType)
        {
            var companionTypeToAdd = new CompanionType()
            {
               
                CompanionType1 = companionType.CompanionType
            };
            _domainObjectRepository.Create<CompanionType>(companionTypeToAdd);
            return companionType;
        }
    }
}
