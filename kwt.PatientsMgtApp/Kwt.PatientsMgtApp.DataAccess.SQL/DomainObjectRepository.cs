using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Kwt.PatientsMgtApp.Core.Interfaces;
using System.Transactions;
using Kwt.PatientsMgtApp.PersistenceDB.EDMX;
using System.Data.Entity;

namespace Kwt.PatientsMgtApp.DataAccess.SQL
{
    public class DomainObjectRepository : IDomainObjectRepository
    {
        private PatientsMgtEntities dbContext;

        public DomainObjectRepository()
        {
            dbContext = new PatientsMgtEntities();

            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
        }

        public IQueryable<T> All<T>(string[] includes = null) where T : class, IDomainObject
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return dbContext.Set<T>().AsQueryable();
        }

        public T Get<T>(Expression<Func<T, bool>> expression, string[] includes = null) where T : class, IDomainObject
        {
            return All<T>(includes).FirstOrDefault(expression);
        }

        public virtual T Find<T>(Expression<Func<T, bool>> predicate, string[] includes = null) where T : class, IDomainObject
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefault<T>(predicate);
            }

            return dbContext.Set<T>().FirstOrDefault<T>(predicate);
        }

        public virtual IQueryable<T> Filter<T>(Expression<Func<T, bool>> predicate, string[] includes = null) where T : class, IDomainObject
        {
            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return dbContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        public IQueryable<T> Filter<T>(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 50, string[] includes = null) where T : class, IDomainObject
        {
            int skipCount = index * size;
            IQueryable<T> _resetSet;

            //HANDLE INCLUDES FOR ASSOCIATED OBJECTS IF APPLICABLE
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dbContext.Set<T>().Where<T>(predicate).AsQueryable() : dbContext.Set<T>().AsQueryable();
            }

            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class, IDomainObject
        {
            return dbContext.Set<T>().Count<T>(predicate) > 0;
        }

        public T Create<T>(T TObject) where T : class, IDomainObject
        {
            //ADD CREATE DATE IF APPLICABLE
            if (TObject is IAuditObject)
            {
                (TObject as IAuditObject).CreatedDate = DateTime.Now;
            }

            var newEntry = dbContext.Set<T>().Add(TObject);
            dbContext.SaveChanges();
            return newEntry;
        }

        public IList<T> CreateBulk<T>(IList<T> t) where T : class, IDomainObject
        {
            using (TransactionScope scope = new TransactionScope())
            {
                PatientsMgtEntities context = null;
                IList<T> newBatch = new List<T>();
                try
                {
                    context = new PatientsMgtEntities();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    int count = 0;
                    foreach (var entityToInsert in t)
                    {
                        ++count;

                        if (entityToInsert is IAuditObject)
                        {
                            (entityToInsert as IAuditObject).CreatedDate = DateTime.Now;
                        }

                        T newEntry = context.Set<T>().Add(entityToInsert);
                        newBatch.Add(newEntry);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    if (context != null)
                        context.Dispose();
                }

                scope.Complete();

                return newBatch;
            }
        }

        public int Delete<T>(T TObject) where T : class, IDomainObject
        {
            dbContext.Set<T>().Remove(TObject);
            return dbContext.SaveChanges();
        }

        public int Delete<T>(Expression<Func<T, bool>> predicate) where T : class, IDomainObject
        {
            var objects = Filter<T>(predicate);
            foreach (var obj in objects)
                dbContext.Set<T>().Remove(obj);
            return dbContext.SaveChanges();
        }
        //
        public int Delete(string command) 
        {
            return dbContext.Database.ExecuteSqlCommand(command);
        }
            //
        public int Update<T>(T TObject) where T : class, IDomainObject
        {

            if (TObject is IAuditObject)
            {
                (TObject as IAuditObject).ModifiedDate = DateTime.Now;
            }
            var entry = dbContext.Entry(TObject);
            //if (entry.State == EntityState.Detached || entry.State == EntityState.Modified)
            //{
                
                dbContext.Set<T>().Attach(TObject);
                entry.State = EntityState.Modified;
            //}
            return dbContext.SaveChanges();
        }

        public int Update<T, T2>(T TObject, T2 TObjectDetail)
            where T : class, IDomainObject
            where T2 : class, IDomainObject
        {
            if (TObject is IAuditObject)
            {
                (TObject as IAuditObject).ModifiedDate = DateTime.Now;
            }

            var entry = dbContext.Entry(TObject);
            dbContext.Set<T>().Attach(TObject);
            entry.State = EntityState.Modified;

            if (TObjectDetail is IAuditObject)
            {
                (TObjectDetail as IAuditObject).ModifiedDate = DateTime.Now;
            }

            var entry2 = dbContext.Entry(TObjectDetail);
            dbContext.Set<T2>().Attach(TObjectDetail);
            entry2.State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        public int UpdateBulk<T>(IList<T> TObject) where T : class, IDomainObject
        {
            foreach (var entityToUpdate in TObject)
            {
                if (entityToUpdate is IAuditObject)
                {
                    (entityToUpdate as IAuditObject).ModifiedDate = DateTime.Now;
                }
                var entry = dbContext.Entry(entityToUpdate);
                dbContext.Set<T>().Attach(entityToUpdate);
                entry.State = EntityState.Modified;
            }

            return dbContext.SaveChanges();
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public int ExecuteSqlCommand(string command, Dictionary<string, object> sqlParams)
        {
            IList<SqlParameter> paramsTemp = new List<SqlParameter>();
            foreach (var sqlParam in sqlParams)
            {
                if (sqlParam.Value != null)
                {
                    paramsTemp.Add(new SqlParameter(sqlParam.Key, sqlParam.Value));
                }
                else
                {
                    paramsTemp.Add(new SqlParameter(sqlParam.Key, DBNull.Value));
                }
            }

            SqlParameter[] parameters = paramsTemp.ToArray();

            return dbContext.Database.ExecuteSqlCommand(command, parameters);
        }


        public void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams)
        {
            dbContext.Database.ExecuteSqlCommand(procedureCommand, sqlParams);
        }

        public List<T> ExecuteProcedure<T>(string procedureCommand, Dictionary<string, object> sqlParams, bool transaction = false)
        {
            IList<SqlParameter> paramsTemp = new List<SqlParameter>();
            foreach (var sqlParam in sqlParams)
            {
                if (sqlParam.Value != null)
                {
                    paramsTemp.Add(new SqlParameter(sqlParam.Key, sqlParam.Value));
                }
                else
                {
                    paramsTemp.Add(new SqlParameter(sqlParam.Key, DBNull.Value));
                }
                procedureCommand = procedureCommand + " @" + sqlParam.Key + ",";
            }
            System.Data.SqlClient.SqlParameter[] spParams = paramsTemp.ToArray();
            var command = procedureCommand.TrimEnd(",".ToCharArray());

            if (!transaction)
            {
                return dbContext.Database.SqlQuery<T>(command, spParams).ToList();
            }

            List<T> result = null;

            using (var dbContextTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    result = dbContext.Database.SqlQuery<T>(command, spParams).ToList();
                    dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }

            return result;
        }

        public void Dispose()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }



        public int UpdateManyBulk<T, T2, T3, T4>(List<T> TObject, List<T2> TObject2, List<T3> TObject3, List<T4> TObject4)
            where T : class, IDomainObject
            where T2 : class, IDomainObject
            where T3 : class, IDomainObject
            where T4 : class, IDomainObject
        {
            foreach (var entityToUpdate in TObject)
            {
                if (entityToUpdate is IAuditObject)
                {
                    (entityToUpdate as IAuditObject).ModifiedDate = DateTime.Now;
                }
                var entry = dbContext.Entry(entityToUpdate);
                dbContext.Set<T>().Attach(entityToUpdate);
                entry.State = EntityState.Modified;
            }

            foreach (var entityToUpdate in TObject2)
            {
                if (entityToUpdate is IAuditObject)
                {
                    (entityToUpdate as IAuditObject).ModifiedDate = DateTime.Now;
                }
                var entry = dbContext.Entry(entityToUpdate);
                dbContext.Set<T2>().Attach(entityToUpdate);
                entry.State = EntityState.Modified;
            }

            foreach (var entityToUpdate in TObject3)
            {
                if (entityToUpdate is IAuditObject)
                {
                    (entityToUpdate as IAuditObject).ModifiedDate = DateTime.Now;
                }
                var entry = dbContext.Entry(entityToUpdate);
                dbContext.Set<T3>().Attach(entityToUpdate);
                entry.State = EntityState.Modified;
            }

            foreach (var entityToUpdate in TObject4)
            {
                if (entityToUpdate is IAuditObject)
                {
                    (entityToUpdate as IAuditObject).ModifiedDate = DateTime.Now;
                }
                var entry = dbContext.Entry(entityToUpdate);
                dbContext.Set<T4>().Attach(entityToUpdate);
                entry.State = EntityState.Modified;
            }
            return dbContext.SaveChanges();
        }
    }
}

