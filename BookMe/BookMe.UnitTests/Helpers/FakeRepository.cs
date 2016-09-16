using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookMe.BusinessLogic.Repository;

namespace BookMe.UnitTests.Helpers
{
    class FakeRepository<T> : IRepository<T> where T : class
    {
        private readonly IList<T> collection;

        public FakeRepository()
        {
            this.collection = new List<T>();
        }

        public FakeRepository(IList<T> collection)
        {
            this.collection = collection;
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            this.collection.Add(entity);
        }

        public void Delete(T entity)
        {
            this.collection.Remove(entity);
        }

        public void DeleteRange(Expression<Func<T, bool>> predicate)
        {
            var entitiesToRemove = this.collection.Where(predicate.Compile());
            foreach (var entity in entitiesToRemove)
            {
                this.collection.Remove(entity);
            }
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return this.collection.Where(predicate.Compile()).AsQueryable();
        }

        public IQueryable<T> Entities => this.collection.AsQueryable();

        public void Save()
        {
        }
    }
}
