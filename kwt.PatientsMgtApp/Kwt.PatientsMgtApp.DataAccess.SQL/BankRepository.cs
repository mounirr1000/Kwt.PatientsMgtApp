using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtAppt.PersistenceDB.EDMX;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class BankRepository : IBankRepository
    {
       readonly private IDomainObjectRepository _domainObjectRepository;
        public BankRepository()
        {
           _domainObjectRepository =new DomainObjectRepository(); 
        }

        public List<BankModel> GetBanks()
        {
            var banks = _domainObjectRepository.All<Bank>();

            return banks?.Select(b=>new BankModel()
            {
                BankCode = b.BankCode,
                BankID = b.BankID,
                BankName = b.BankName,
               
            }).ToList();
        }

       public BankModel GetBank(int bankId)
        {

            var bank= _domainObjectRepository.Get<Bank>(b=>b.BankID==bankId);

            if(bank!=null)
            return new BankModel()
            {
                BankCode = bank.BankCode,
                BankID = bank.BankID,
                BankName = bank.BankName,
            };
           return null;
        }

       public int DeleteBank(int bankId)
       {
           var bank = _domainObjectRepository.Get<Bank>(b => b.BankID == bankId);
           if (bank != null)
           {
                // we should remove refrenced Object in other class before do this delete
                // pending ....
           return _domainObjectRepository.Delete<Bank>(bank);
           }
            return 0;
        }

        public void UpdateBank(BankModel bank)
        {
            IBankRepository _bank=new BankRepository();
            var oldBank= _bank.GetBank(bank.BankID);
            var bankToUpdate = new Bank();

            if (oldBank == null) return;
            bankToUpdate.BankCode = bank.BankCode;
            bankToUpdate.BankName = bank.BankName;
            _domainObjectRepository.Update<Bank>(bankToUpdate);
        }

        public BankModel AddBank(BankModel bank)
        {

            return bank;
        }
    }
}
