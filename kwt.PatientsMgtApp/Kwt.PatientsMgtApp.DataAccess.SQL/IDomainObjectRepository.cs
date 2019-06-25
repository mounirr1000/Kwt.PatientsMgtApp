using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Kwt.PatientsMgtApp.Core.Interfaces;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public interface IDomainObjectRepository : IDisposable
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All<T>(string[] includes = null) where T : class, IDomainObject;

        /// <summary>
        /// Select Single Item by specified expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> expression, string[] includes = null) where T : class, IDomainObject;

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Find<T>(Expression<Func<T, bool>> predicate, string[] includes = null) where T : class, IDomainObject;

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <returns></returns>
        IQueryable<T> Filter<T>(Expression<Func<T, bool>> predicate, string[] includes = null) where T : class, IDomainObject;

        /// <summary>
        /// Gets objects from database with filting and paging.
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        /// <returns></returns>
        IQueryable<T> Filter<T>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50, string[] includes = null) where T : class, IDomainObject;

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <returns></returns>
        bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class, IDomainObject;

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        T Create<T>(T t) where T : class, IDomainObject;

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        /// <returns></returns>
        IList<T> CreateBulk<T>(IList<T> t) where T : class, IDomainObject;

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>
        int Delete<T>(T t) where T : class, IDomainObject;

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> predicate) where T : class, IDomainObject;


        int Delete(string command);
        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        /// <returns></returns>
        int Update<T>(T t) where T : class, IDomainObject;

        int Update<T, T2>(T TObject, T2 TObjectDetail)
            where T : class, IDomainObject
            where T2 : class, IDomainObject;

        int UpdateBulk<T>(IList<T> TObject) where T : class, IDomainObject;

        int UpdateManyBulk<T, T2, T3, T4>(List<T> TObject, List<T2> TObject2, List<T3> TObject3, List<T4> TObject4)
            where T : class, IDomainObject
            where T2 : class, IDomainObject
            where T3 : class, IDomainObject
            where T4 : class, IDomainObject;

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();

        int ExecuteSqlCommand(String command, Dictionary<string, object> sqlParams);

        /// <summary>
        /// Executes the procedure.
        /// </summary>
        /// <param name="procedureCommand">The procedure command.</param>
        /// <param name="sqlParams">The SQL params.</param>
        void ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams);

        List<T> ExecuteProcedure<T>(String procedureCommand, Dictionary<string, object> sqlParams, bool transaction = false);


        //async List<T> ExecuteProcedureWithTransactionAsync<T>(String[] procedureCommand, Dictionary<string, object> sqlParams); 
        //List<T> ExecuteProcedureWithTransaction<T>(String procedureCommand, Dictionary<string, object> sqlParams); 
    }
}
