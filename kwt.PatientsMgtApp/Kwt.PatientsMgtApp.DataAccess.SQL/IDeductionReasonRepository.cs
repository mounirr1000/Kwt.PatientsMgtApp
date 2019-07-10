using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IDeductionReasonRepository
    {

        List<DeductionReasonModel> GetDeductionReasons();
        DeductionReasonModel GetDeductionReason(int reasonId);

        void AddDeductionReason(DeductionReasonModel deductionReason);
    }
}
