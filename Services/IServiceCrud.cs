using Domains;
using Infrastructure;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IServiceCrud<T> : IServiceCrud where T : IBaseDomain
	{
        T Insert(T obj);

        Task<T> InsertAsync(T obj);

        void Delete(T obj);

        Task<int> DeleteAsync(T obj);

        T Find(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);

        T GetById(long id);

        Task<T> GetByIdAsync(long id);

        IQueryable<T> GetAll();

        Task<ICollection<T>> GetAllAsync();

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>> [] includeProperties);

        Task<ICollection<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);

        T Update(T obj, object key);

        Task<T> UpdateAsync(T obj, object key);

		void DeleteCache(string key);

		T GetCache(string key);

		void SaveCache(string key, T obj);
	}
}
