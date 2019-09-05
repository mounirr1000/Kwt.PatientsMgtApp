using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class BookTypeRepository: IBookTypeRepository
    {
        private readonly IDomainObjectRepository _domainObjectRepository;
        public BookTypeRepository()
        {
            _domainObjectRepository = new DomainObjectRepository();
        }
        public List<BookTypeModel> GetBookTypeList()
        {
            var newBookTypes = _domainObjectRepository.All<BookType>();

            return newBookTypes.Select(p => new BookTypeModel()
            {
                BookType = p.BookType1,
                BookTypeID = p.BookTypeID
            }).ToList();
        }

        public void AddBookType(BookTypeModel bookTypeMdodel)
        {
            if (bookTypeMdodel != null)
            {
                BookType bookType = new BookType()
                {
                    //  AdjustmentReasonID = adjustmentReason.AdjustmentReasonID,
                    BookType1 = bookTypeMdodel.BookType
                };
                _domainObjectRepository.Create<BookType>(bookType);
            }
        }
    }
}
