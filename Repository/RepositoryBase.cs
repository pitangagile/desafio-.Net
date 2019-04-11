using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
	public class RepositoryBase<TEntity, TContext> where TEntity : class where TContext: DbContext
	{
		protected readonly ApplicationDbContext<TContext> _dbContext;
		protected DbSet<TEntity> _dbSet;
		private bool disposed = false;

		public RepositoryBase(ApplicationDbContext<TContext> context)
		{
			this._dbContext = context;
		}

		//https://imasters.com.br/back-end/advanced-repository-pattern-com-entity-framework-core

		public virtual async Task<Tuple<IEnumerable<TEntity>, int>> GetAllAsync(int skip, int take, Expression<Func<TEntity, bool>> where,
			Expression<Func<TEntity, object>> orderBy, bool asNoTracking = true)
		{
			var dataBaseCount = await this._dbSet.CountAsync().ConfigureAwait(false);
			if (asNoTracking)
				return new Tuple<IEnumerable<TEntity>, int>(await this._dbSet.AsNoTracking().OrderBy(orderBy).Where(where).Skip(skip).Take(take).ToListAsync().ConfigureAwait(false), dataBaseCount);

			return new Tuple<IEnumerable<TEntity>, int>(await this._dbSet.OrderBy(orderBy).Where(where).Skip(skip).Take(take).ToListAsync().ConfigureAwait(false), dataBaseCount);
		}

		public virtual async Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>> match, bool asNoTracking = true)
		{
			if (asNoTracking)
				return await this._dbSet.AsNoTracking().Where(match).ToListAsync();
			return await this._dbSet.Where(match).ToListAsync();
		}

		public virtual IQueryable<TEntity> GetAllIncluding(bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> queryable = GetAll(asNoTracking);
			foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
			{
				queryable = queryable.Include<TEntity, object>(includeProperty);
			}

			return queryable;
		}

		public virtual async Task<ICollection<TEntity>> GetAllIncludingAsync(bool asNoTracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			return await GetAllIncluding(asNoTracking, includeProperties).ToListAsync();
		}

		public virtual IQueryable<TEntity> GetAll(bool asNoTracking = true)
		{
			if (asNoTracking)
				return this._dbSet.AsNoTracking();

			return this._dbSet;
		}

		public virtual async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> match, bool asNoTracking = true)
		{
			if (asNoTracking)
				return await this._dbSet.AsNoTracking().SingleOrDefaultAsync(match);

			return await this._dbSet.SingleOrDefaultAsync(match);
		}

		public virtual async Task<TEntity> FindByIdAsync(object id)
		{
			return await this._dbSet.FindAsync(id);
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

		public virtual async Task<TEntity> RemoveById(object id)
		{
			TEntity finded = await this._dbSet.FindAsync(id);

			if (finded != null)
			{
				this._dbSet.Remove(finded);
			}
			return finded;
		}

		public virtual async Task SaveChangesAsync()
		{
			await this._dbContext.SaveChangesAsync().ConfigureAwait(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					_dbContext.Dispose();
				}
				this.disposed = true;
			}
		}
	}
}
