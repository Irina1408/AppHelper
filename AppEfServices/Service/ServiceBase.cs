using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppEfServices.Settings;
using AppServices.Exceptions;

namespace AppServices
{
    public class ServiceBase : IDisposable
    {
        private readonly DbContext dataContext;

        public DbContext Data
        {
            get { return dataContext; }
        }

        public ServiceBase()
        {
            // initialise data context
            //dataContext = new DbContext();
        }

        public ServiceBase(IEfConnectionSettings connectionSettings)
        {
            // initialise data context
            dataContext = new DbContext(connectionSettings.EfConnectionString);
        }

        public ServiceBase(string connectionString)
        {
            // initialise data context
            dataContext = new DbContext(connectionString);
        }

        public ServiceBase(DbContext dataContext)
        {
            this.dataContext = dataContext;
        }
        
        public int SaveChanges()
        {
            try
            {
                return dataContext.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                SqlException exception = ex.InnerException.InnerException as SqlException;
                //exception.Number = 547 - Foreign Key violation
                if (exception != null && exception.Number == 547)
                {
                    // set ex.InnerException as inner exception for best debugging
                    throw new ReferenceConstraintException(ex.Message, ex.InnerException);
                }
                throw;
            }
        }

        public bool HasChanges()
        {
            return dataContext.ChangeTracker.HasChanges();
        }

        public ObservableCollection<TEntity> GetLocal<TEntity>() where TEntity : class
        {
            dataContext.Set<TEntity>().Load();
            return dataContext.Set<TEntity>().Local;
        }

        public ObservableCollection<TEntity> GetLocal<TEntity>(DbSet<TEntity> dbSet) where TEntity : class
        {
            dbSet.Load();
            return dbSet.Local;
        }

        public async Task<ObservableCollection<TEntity>> GetLocalAsync<TEntity>(DbSet<TEntity> dbSet, CancellationToken cancellationToken) where TEntity : class
        {
            await dbSet.LoadAsync(cancellationToken);

            return dbSet.Local;
        }

        #region IDisposable support

        public virtual void Dispose()
        {
            dataContext.Dispose();
        }

        #endregion
    }
}
