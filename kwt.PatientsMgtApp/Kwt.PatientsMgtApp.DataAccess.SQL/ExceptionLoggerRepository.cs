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

        public ExceptionLoggerObject GetExceptionLogger(int exceptionId)
        {
            var exception=
            _domainObjectRepository.Get<ExceptionLogger>(e => e.Id == exceptionId);

            return new ExceptionLoggerObject()
            {
                ControllerName = exception.ControllerName,
                ExceptionStackTrace = exception.ExceptionStackTrace,
                ExceptionMessage = exception.ExceptionMessage,
                Id = exception.Id,
                LogTime = exception.LogTime
            };


        }

        public List<ExceptionLoggerObject> GetExceptionsLogger()
        {
            var exceptions = _domainObjectRepository.All<ExceptionLogger>();
            return exceptions?.Select(e => new ExceptionLoggerObject()
            {
                ControllerName = e.ControllerName,
                ExceptionStackTrace = e.ExceptionStackTrace,
                ExceptionMessage = e.ExceptionMessage,
                Id = e.Id,
                LogTime = e.LogTime
            }).ToList(); 
            
        }

        public ExceptionLoggerObject GetLatestExceptionLogger()
        {
            return GetExceptionsLogger()?.OrderByDescending(c => c.LogTime).ThenByDescending(e=>e.Id).FirstOrDefault();
        }

       public int DeleteExceptionsLogger()
       {
            return _domainObjectRepository.Delete("DELETE FROM ExceptionLoggers");
       }
    }
}
