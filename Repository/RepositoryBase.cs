using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class RepositoryBase<TEntity> where TEntity : class
	{
		protected readonly DbContext _dbContext;
		protected DbSet<TEntity> _dbSet;

		public RepositoryBase(DbContext context)
		{
			this._dbContext = context;
		}

		//https://imasters.com.br/back-end/advanced-repository-pattern-com-entity-framework-core

		public virtual async Task<Tuple<IEnumerable<TEntity>, int>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> orderBy, bool asNoTracking = true)
		{
			var dataBaseCount = await this._dbSet.CountAsync().ConfigureAwait(false);
			if (asNoTracking)
				return new Tuple<IEnumerable<TEntity>, int>(await this._dbSet.AsNoTracking().OrderBy(orderBy).Where(where).Skip(skip).Take(take).ToListAsync().ConfigureAwait(false), dataBaseCount);

			return new Tuple<IEnumerable<TEntity>, int>(await this._dbSet.OrderBy(orderBy).Where(where).Skip(skip).Take(take).ToListAsync().ConfigureAwait(false), dataBaseCount);
		}

		public virtual async Task AddAsync(TEntity entity)
		{
			await this._dbSet.AddAsync(entity).ConfigureAwait(false);
		}

		public virtual async Task AddCollectionAsync(IEnumerable<TEntity> entities)
		{
			await this._dbSet.AddRangeAsync(entities).ConfigureAwait(false);
		}

		public virtual IEnumerable<TEntity> AddCollectionWithProxy(IEnumerable<TEntity> entities)
		{
			foreach (var entity in entities)
			{
				this._dbSet.Add(entity);
				yield return entity;
			}
		}

		public virtual Task UpdateAsync(TEntity entity)
		{
			this._dbSet.Update(entity);
			return Task.CompletedTask;
		}

		public virtual Task UpdateCollectionAsync(IEnumerable<TEntity> entities)
		{
			this._dbSet.UpdateRange(entities);
			return Task.CompletedTask;
		}

		public virtual IEnumerable<TEntity> UpdateCollectionWithProxy(IEnumerable<TEntity> entities)
		{
			foreach (var entity in entities)
			{
				this._dbSet.Update(entity);
				yield return entity;
			}
		}

		public virtual Task RemoveByAsync(Func<TEntity, bool> where)
		{
			this._dbSet.RemoveRange(this._dbSet.ToList().Where(where));
			return Task.CompletedTask;
		}

		public virtual Task RemoveAsync(TEntity entity)
		{
			this._dbSet.Remove(entity);
			return Task.CompletedTask;
		}

		public virtual async Task SaveChangesAsync()
		{
			await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
		}

	}
}
