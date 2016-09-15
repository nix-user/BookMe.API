using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.BusinessLogic.Repository
{
    public interface IRepository<T>
    {
        T GetById(int id);

        void Insert(T entity);

        void Delete(T entity);

        void DeleteRange(Expression<Func<T, bool>> predicate);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        IQueryable<T> Entities { get; }
    }
}