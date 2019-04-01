using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repository;

namespace Services
{
	public abstract class BaseService<TEntity> : BaseServiceRedis<TEntity>, IServiceCrud<TEntity> where TEntity : class
	{
		protected readonly DbContext _dbContext;
		private bool disposed = false;
		private readonly IValidator _validator;
		private readonly IRepositoryBase<TEntity> _repository;

		public BaseService(DbContext dbContext, IRedisConnectionFactory connectionFactory, IValidator<TEntity> validator, IRepositoryBase<TEntity> repository) : base(connectionFactory)
		{
			_validator = validator;
			_dbContext = dbContext;
			_repository = repository;
		}

		public int Count()
		{
			return _dbContext.Set<TEntity>().Count();
		}

		public async Task<int> CountAsync()
		{
			return await _dbContext.Set<TEntity>().CountAsync();
		}

		public virtual void Delete(object id)
		{
			TEntity finded = _dbContext.Set<TEntity>().Find(id);

			if (finded != null)
			{
				_dbContext.Remove(finded);

				_dbContext.SaveChanges();
			}
		}

		public virtual async Task<int> DeleteAsync(object id)
		{
			TEntity finded = _dbContext.Set<TEntity>().Find(id);

			if (finded != null)
			{
				_dbContext.Remove(finded);

				_dbContext.SaveChanges();
			}

			return await _dbContext.SaveChangesAsync();
		}

		protected virtual void BeforeDelete(TEntity obj)
		{
		}

		protected virtual void AfterDelete(TEntity obj)
		{
		}

		protected virtual Task BeforeDeleteAsync(TEntity obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterDeleteAsync(TEntity obj)
		{
			return Task.CompletedTask;
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

		public virtual TEntity Find(Expression<Func<TEntity, bool>> match)
		{
			return _dbContext.Set<TEntity>().SingleOrDefault(match);
		}

		public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
		{
			return _dbContext.Set<TEntity>().Where(match).ToList();
		}

		public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
		{
			return await _dbContext.Set<TEntity>().Where(match).ToListAsync();
		}

		public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
		{
			return await _dbContext.Set<TEntity>().SingleOrDefaultAsync(match);
		}

		public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
		{
			IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

			return query;
		}

		public virtual async Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
		}

		public IQueryable<TEntity> GetAll()
		{
			return _dbContext.Set<TEntity>();
		}

		public virtual async Task<ICollection<TEntity>> GetAllAsync()
		{
			return await _dbContext.Set<TEntity>().ToListAsync();
		}

		public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> queryable = GetAll();
			foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
			{
				queryable = queryable.Include<TEntity, object>(includeProperty);
			}

			return queryable;
		}

		public virtual async Task<ICollection<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			ICollection<TEntity> queryable = await GetAllIncluding(includeProperties).ToListAsync();

			return queryable;
		}

		public virtual TEntity GetById(object id)
		{
			return _dbContext.Set<TEntity>().Find(id);
		}

		public virtual async Task<TEntity> GetByIdAsync(object id)
		{
			return await _dbContext.Set<TEntity>().FindAsync(id);
		}

		public virtual TEntity Insert(TEntity obj)
		{
			var validated = _validator.Validate(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}
			BeforeInsert(obj);
			_dbContext.Set<TEntity>().Add(obj);
			_dbContext.SaveChanges();
			AfterInsert(obj);

			return obj;
		}

		public virtual async Task<TEntity> InsertAsync(TEntity obj)
		{
			var validated = await _validator.ValidateAsync(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}

			await BeforeInsertAsync(obj);
			await _dbContext.Set<TEntity>().AddAsync(obj);
			await _dbContext.SaveChangesAsync();
			await AfterInsertAsync(obj);

			return obj;
		}

		protected virtual void BeforeInsert(TEntity obj)
		{
		}

		protected virtual void AfterInsert(TEntity obj)
		{
		}

		protected virtual Task BeforeInsertAsync(TEntity obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterInsertAsync(TEntity obj)
		{
			return Task.CompletedTask;
		}

		public virtual void Save()
		{
			_dbContext.SaveChanges();
		}

		public virtual async Task<int> SaveAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public virtual TEntity Update(TEntity obj, object key)
		{
			if (obj == null)
				return null;

			var validated = _validator.Validate(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}

			TEntity exist = _dbContext.Set<TEntity>().Find(key);

			if (exist != null)
			{
				//if (exist._semaphore != obj._semaphore)
				//{
				//	throw new Exception("Object updated by another user!");
				//}
				BeforeUpdate(exist);
				_dbContext.Entry(exist).CurrentValues.SetValues(obj);
				AfterUpdate(obj);
				_dbContext.SaveChanges();
			}
			return exist;
		}

		public virtual async Task<TEntity> UpdateAsync(TEntity obj, object key)
		{
			if (obj == null)
				return null;

			var validated = await _validator.ValidateAsync(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}

			TEntity exist = await _dbContext.Set<TEntity>().FindAsync(key);

			if (exist != null)
			{
				//if (exist._semaphore != obj._semaphore)
				//{
				//	throw new Exception("Object updated by another user!");
				//}
				await BeforeUpdateAsync(exist);
				_dbContext.Entry(exist).CurrentValues.SetValues(obj);
				await AfterUpdateAsync(obj);
				await _dbContext.SaveChangesAsync();
			}
			return exist;
		}

		protected virtual void BeforeUpdate(TEntity obj)
		{
		}

		protected virtual void AfterUpdate(TEntity obj)
		{
		}

		protected virtual Task BeforeUpdateAsync(TEntity obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterUpdateAsync(TEntity obj)
		{
			return Task.CompletedTask;
		}
	}
}
