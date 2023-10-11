using Kwt.PatientsMgtApp.Core.Models;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IEmployeeRepository
    {
        #region  Employee CRUD Operation


        List<EmployeeModel> GetEmployees();          //CRUD     

        EmployeeModel GetEmployeeObject();

        EmployeeModel GetEmployee(int employeeId);                

        void AddEmployee(EmployeeModel employee);

        EmployeeModel UpdateEmployee(EmployeeModel employee);

        int DeleteEmployee(EmployeeModel employee);
        #endregion

        #region Employee Payment
        void CreateEmployeeRegularPayment(List<EmployeeModel> EmployeeList);
        #endregion
    }
}
