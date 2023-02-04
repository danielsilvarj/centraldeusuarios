using CentralDeUsuarios.Domain.Core;
using CentralDeUsuarios.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Infra.Data.Repositories
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : class
    {

        private readonly SqlServerContext _sqlServerContext;

        protected BaseRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext;
        }

        public virtual void Create(TEntity entity)
        {
            _sqlServerContext.Entry(entity).State = EntityState.Added;
            _sqlServerContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            _sqlServerContext.Entry(entity).State = EntityState.Deleted;
            _sqlServerContext.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            _sqlServerContext.Entry(entity).State = EntityState.Modified;
            _sqlServerContext.SaveChanges();
        }

        public virtual TEntity FindById(TKey Id)
        {
            return _sqlServerContext.Set<TEntity>().Find(Id);
        }

        public virtual List<TEntity> GetAll()
        {
            return _sqlServerContext.Set<TEntity>().ToList();
        }

        public void Dispose()
        {
            _sqlServerContext.Dispose();
        }
    }
}
