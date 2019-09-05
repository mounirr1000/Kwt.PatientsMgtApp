using Kwt.PatientsMgtApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IBookTypeRepository
    {
        List<BookTypeModel> GetBookTypeList();
        void AddBookType(BookTypeModel bookTypeMdodel);
    }
}
