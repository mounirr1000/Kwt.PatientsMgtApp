using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class CompanionManagmentRepository: ICompanionManagmentRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;

       public  CompanionManagmentRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<CompanionTypeModel> GetCompanionTypes()
        {
            var types = _domainObjectRepository.All<CompanionType>();
            return types?.Select(ct=> new CompanionTypeModel()
            {
             CompanionType = ct.CompanionType1,
             Id   = ct.CompanionTypeID
            }).ToList();
        }
    }
}
