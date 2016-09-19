using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.Repository;

namespace BookMe.Data.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;

        public EFRepository(DbContext context)
        {
            this.context = context;
        }

        public void Delete(T entity)
        {
            this.context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Entities => this.context.Set<T>();
        public void Save()
        {
            this.context.SaveChanges();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.context.Set<T>().Where(predicate);
        }

        public T GetById(int id)
        {
            return this.context.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            this.context.Set<T>().Add(entity);
        }

        public void DeleteRange(Expression<Func<T, bool>> predicate)
        {
            var entities = this.context.Set<T>().Where(predicate);
            this.context.Set<T>().RemoveRange(entities);
        }
    }
}
