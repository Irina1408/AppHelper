using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppServices
{
    public class EntityService : ServiceBase
    {
        public EntityService()
            : base()
        { }

        public EntityService(DbContext dbContext)
            : base(dbContext)
        { }

        public DbSet<T> Entities<T>() where T : class
        {
            return Data.Set<T>();
        }

        public List<T> EntitiesList<T>() where T : class
        {
            return Data.Set<T>().ToList();
        }

        public T Create<T>() where T : class
        {
            var db = Data.Set<T>();
            return db.Create();
        }

        public T Add<T>(T entity) where T : class
        {
            var db = Data.Set<T>();
            return db.Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            var db = Data.Set<T>();
            db.Remove(entity);
        }
    }
}
