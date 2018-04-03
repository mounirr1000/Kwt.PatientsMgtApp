using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IBankRepository
    {

        List<BankModel> GetBanks();

        BankModel GetBank(int bankId);

        int DeleteBank(int bankId);

        void UpdateBank(BankModel bank);

        BankModel AddBank(BankModel bank);

    }
}
