﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface ISpecialityRepository
    {
        List<SpecialtyModel> GetSpecialities();
        SpecialtyModel GetSpeciality(int specialityid);

        void AddSpeciality(SpecialtyModel specialty);
    }
}
