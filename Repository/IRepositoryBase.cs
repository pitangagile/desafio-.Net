using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public interface IRepositoryBase<TEntity> where TEntity: class
	{
		Task<Tuple<IEnumerable<TEntity>, int>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> where, 
			Expression<Func<TEntity, object>> orderBy, bool asNoTracking = true);
		Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> match, bool asNoTracking = true);
		Task<ICollection<TEntity>> GetAllIncludingAsync(bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
		IQueryable<TEntity> GetAll(bool asNoTracking = true);
		Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> match, bool asNoTracking = true);
		Task<TEntity> FindByIdAsync(object id);
		Task AddAsync(TEntity entity);
		Task AddCollectionAsync(IEnumerable<TEntity> entities);
		IEnumerable<TEntity> AddCollectionWithProxy(IEnumerable<TEntity> entities);
		Task UpdateAsync(TEntity entity);
		Task UpdateCollectionAsync(IEnumerable<TEntity> entities);
		IEnumerable<TEntity> UpdateCollectionWithProxy(IEnumerable<TEntity> entities);
		Task RemoveByAsync(Func<TEntity, bool> where);
		Task RemoveAsync(TEntity entity);
		Task<TEntity> RemoveByIdAsync(object id);
		Task SaveChangesAsync();
		void Dispose();
	}
}
