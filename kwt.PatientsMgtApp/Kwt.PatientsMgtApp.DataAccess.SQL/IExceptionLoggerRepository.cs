using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IExceptionLoggerRepository
    {
        void AddExceptionLogger(ExceptionLoggerObject exception);
    }
}
