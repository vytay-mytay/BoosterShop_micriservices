using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.API.DAL.Abstract
{
    public interface IRepository<T> : IDisposable where T : class
    {
        IQueryable<T> Table { get; }

        IList<T> GetAll();

        IQueryable<T> Get(Expression<Func<T, bool>> predicate);

        T Find(Expression<Func<T, bool>> predicate);

        T GetById(object id);

        void Insert(T entity);

        void InsertRange(IEnumerable<T> entities);

        Task InsertAsync(T entity);

        void Update(T entity);

        void UpdateRange(List<T> entities);

        Task UpdateAsync(T entity);

        void Delete(T entity);

        void DeleteById(int id);

        int Count();

        int Count(Expression<Func<T, bool>> predicate);

        void DeleteRange(IEnumerable<T> entities);

        IEnumerable<S> ExecuteStoredProcedure<S>(string storedProcedure, Dictionary<string, object> parameters) where S : class;

        bool Any(Expression<Func<T, bool>> predicate);
    }
}
