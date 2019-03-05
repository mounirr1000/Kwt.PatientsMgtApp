using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class ExceptionLoggerRepository: IExceptionLoggerRepository
    {
       readonly private IDomainObjectRepository _domainObjectRepository;

        public ExceptionLoggerRepository()
        {
            _domainObjectRepository =new  DomainObjectRepository();
        }

        public void AddExceptionLogger(ExceptionLoggerObject exception)
        {
            var exceptionLoggerObject = new ExceptionLogger
            {
                ControllerName = exception.ControllerName,
                ExceptionMessage = exception.ExceptionMessage,
                ExceptionStackTrace = exception.ExceptionStackTrace,
                LogTime = DateTime.Now
            };
            _domainObjectRepository.Create<ExceptionLogger>(exceptionLoggerObject);
        }
    }
}
