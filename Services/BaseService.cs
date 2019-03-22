using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Services
{
	public abstract class BaseService<T> : BaseServiceRedis<T>, IServiceCrud<T> where T: class
	{
		protected readonly DbContext _dbContext;
		private bool disposed = false;
		private readonly IValidator _validator;

		public BaseService(DbContext dbContext, IRedisConnectionFactory connectionFactory, IValidator<T> validator) : base(connectionFactory)
		{
			_validator = validator;
			_dbContext = dbContext;
		}

		public int Count()
		{
			return _dbContext.Set<T>().Count();
		}

		public async Task<int> CountAsync()
		{
			return await _dbContext.Set<T>().CountAsync();
		}

		public virtual void Delete(object id)
		{
			T finded = _dbContext.Set<T>().Find(id);

			if (finded != null)
			{
				_dbContext.Remove(finded);

				_dbContext.SaveChanges();
			}
		}

		public virtual async Task<int> DeleteAsync(object id)
		{
			T finded = _dbContext.Set<T>().Find(id);

			if (finded != null)
			{
				_dbContext.Remove(finded);

				_dbContext.SaveChanges();
			}

			return await _dbContext.SaveChangesAsync();
		}

		protected virtual void BeforeDelete(T obj)
		{
		}

		protected virtual void AfterDelete(T obj)
		{
		}

		protected virtual Task BeforeDeleteAsync(T obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterDeleteAsync(T obj)
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

		public virtual T Find(Expression<Func<T, bool>> match)
		{
			return _dbContext.Set<T>().SingleOrDefault(match);
		}

		public ICollection<T> FindAll(Expression<Func<T, bool>> match)
		{
			return _dbContext.Set<T>().Where(match).ToList();
		}

		public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
		{
			return await _dbContext.Set<T>().Where(match).ToListAsync();
		}

		public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match)
		{
			return await _dbContext.Set<T>().SingleOrDefaultAsync(match);
		}

		public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
		{
			IQueryable<T> query = _dbContext.Set<T>().Where(predicate);

			return query;
		}

		public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
		{
			return await _dbContext.Set<T>().Where(predicate).ToListAsync();
		}

		public IQueryable<T> GetAll()
		{
			return _dbContext.Set<T>();
		}

		public virtual async Task<ICollection<T>> GetAllAsync()
		{
			return await _dbContext.Set<T>().ToListAsync();
		}

		public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> queryable = GetAll();
			foreach (Expression<Func<T, object>> includeProperty in includeProperties)
			{
				queryable = queryable.Include<T, object>(includeProperty);
			}

			return queryable;
		}

		public virtual async Task<ICollection<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
		{
			ICollection<T> queryable = await GetAllIncluding(includeProperties).ToListAsync();

			return queryable;
		}

		public virtual T GetById(object id)
		{
			return _dbContext.Set<T>().Find(id);
		}

		public virtual async Task<T> GetByIdAsync(object id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public virtual T Insert(T obj)
		{
			var validated = _validator.Validate(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}
			BeforeInsert(obj);
			_dbContext.Set<T>().Add(obj);
			_dbContext.SaveChanges();
			AfterInsert(obj);

			return obj;
		}

		public virtual async Task<T> InsertAsync(T obj)
		{
			var validated = await _validator.ValidateAsync(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}

			await BeforeInsertAsync(obj);
			await _dbContext.Set<T>().AddAsync(obj);
			await _dbContext.SaveChangesAsync();
			await AfterInsertAsync(obj);

			return obj;
		}

		protected virtual void BeforeInsert(T obj)
		{
		}

		protected virtual void AfterInsert(T obj)
		{
		}

		protected virtual Task BeforeInsertAsync(T obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterInsertAsync(T obj)
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

		public virtual T Update(T obj, object key)
		{
			if (obj == null)
				return null;

			var validated = _validator.Validate(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}

			T exist = _dbContext.Set<T>().Find(key);

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

		public virtual async Task<T> UpdateAsync(T obj, object key)
		{
			if (obj == null)
				return null;

			var validated = await _validator.ValidateAsync(obj);
			if (!validated.IsValid)
			{
				throw new Exception(JsonConvert.SerializeObject(validated.Errors));
			}

			T exist = await _dbContext.Set<T>().FindAsync(key);

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

		protected virtual void BeforeUpdate(T obj)
		{
		}

		protected virtual void AfterUpdate(T obj)
		{
		}

		protected virtual Task BeforeUpdateAsync(T obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterUpdateAsync(T obj)
		{
			return Task.CompletedTask;
		}
	}
}
