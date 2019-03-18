using Domains;
using FluentValidation;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class ApplicationUserService : BaseServiceRedis<ApplicationUser>, IApplicationUserService
	{
		protected readonly DbContext _dbContext;
		private bool disposed = false;
		
		public ApplicationUserService(DbContext dbContext, IRedisConnectionFactory connectionFactory) : base(connectionFactory)
		{
			_dbContext = dbContext;
		}

		public int Count()
		{
			return _dbContext.Set<ApplicationUser>().Count();
		}

		public async Task<int> CountAsync()
		{
			return await _dbContext.Set<ApplicationUser>().CountAsync();
		}

		public virtual void Delete(ApplicationUser obj)
		{
			ApplicationUser exist = _dbContext.Set<ApplicationUser>().Find(obj.Id);

			if (exist != null)
			{
				BeforeDelete(obj);

				_dbContext.Set<ApplicationUser>().Update(obj);
				AfterDelete(obj);

				_dbContext.SaveChanges();
			}
		}

		public virtual async Task<int> DeleteAsync(ApplicationUser obj)
		{
			ApplicationUser exist = _dbContext.Set<ApplicationUser>().Find(obj.Id);

			if (exist != null)
			{
				await BeforeDeleteAsync(obj);

				_dbContext.Set<ApplicationUser>().Update(obj);
				await AfterDeleteAsync(obj);
			}

			return await _dbContext.SaveChangesAsync();
		}

		protected virtual void BeforeDelete(ApplicationUser obj)
		{
		}

		protected virtual void AfterDelete(ApplicationUser obj)
		{
		}

		protected virtual Task BeforeDeleteAsync(ApplicationUser obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterDeleteAsync(ApplicationUser obj)
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

		public virtual ApplicationUser Find(Expression<Func<ApplicationUser, bool>> match)
		{
			return _dbContext.Set<ApplicationUser>().SingleOrDefault(match);
		}

		public ICollection<ApplicationUser> FindAll(Expression<Func<ApplicationUser, bool>> match)
		{
			return _dbContext.Set<ApplicationUser>().Where(match).ToList();
		}

		public async Task<ICollection<ApplicationUser>> FindAllAsync(Expression<Func<ApplicationUser, bool>> match)
		{
			return await _dbContext.Set<ApplicationUser>().Where(match).ToListAsync();
		}

		public virtual async Task<ApplicationUser> FindAsync(Expression<Func<ApplicationUser, bool>> match)
		{
			return await _dbContext.Set<ApplicationUser>().SingleOrDefaultAsync(match);
		}

		public virtual IQueryable<ApplicationUser> FindBy(Expression<Func<ApplicationUser, bool>> predicate)
		{
			IQueryable<ApplicationUser> query = _dbContext.Set<ApplicationUser>().Where(predicate);

			return query;
		}

		public virtual async Task<ICollection<ApplicationUser>> FindByAsync(Expression<Func<ApplicationUser, bool>> predicate)
		{
			return await _dbContext.Set<ApplicationUser>().Where(predicate).ToListAsync();
		}

		public virtual IQueryable<ApplicationUser> GetAll()
		{
			return _dbContext.Set<ApplicationUser>();
		}

		public virtual async Task<ICollection<ApplicationUser>> GetAllAsync()
		{
			return await _dbContext.Set<ApplicationUser>().ToListAsync();
		}

		public virtual IQueryable<ApplicationUser> GetAllIncluding(params Expression<Func<ApplicationUser, object>>[] includeProperties)
		{
			IQueryable<ApplicationUser> queryable = GetAll();
			foreach (Expression<Func<ApplicationUser, object>> includeProperty in includeProperties)
			{
				queryable = queryable.Include<ApplicationUser, object>(includeProperty);
			}

			return queryable;
		}

		public virtual async Task<ICollection<ApplicationUser>> GetAllIncludingAsync(params Expression<Func<ApplicationUser, object>>[] includeProperties)
		{
			ICollection<ApplicationUser> queryable = await GetAllIncluding(includeProperties).ToListAsync();

			return queryable;
		}

		public virtual ApplicationUser GetById(long id)
		{
			return _dbContext.Set<ApplicationUser>().Find(id);
		}

		public virtual async Task<ApplicationUser> GetByIdAsync(long id)
		{
			return await _dbContext.Set<ApplicationUser>().FindAsync(id);
		}

		public virtual ApplicationUser Insert(ApplicationUser obj)
		{
			BeforeInsert(obj);
			_dbContext.Set<ApplicationUser>().Add(obj);
			_dbContext.SaveChanges();
			AfterInsert(obj);

			return obj;
		}

		public virtual async Task<ApplicationUser> InsertAsync(ApplicationUser obj)
		{
			await BeforeInsertAsync(obj);
			await _dbContext.Set<ApplicationUser>().AddAsync(obj);
			await _dbContext.SaveChangesAsync();
			await AfterInsertAsync(obj);

			return obj;
		}

		protected virtual void BeforeInsert(ApplicationUser obj)
		{
		}

		protected virtual void AfterInsert(ApplicationUser obj)
		{
		}

		protected virtual Task BeforeInsertAsync(ApplicationUser obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterInsertAsync(ApplicationUser obj)
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

		public virtual ApplicationUser Update(ApplicationUser obj, object key)
		{
			if (obj == null)
				return null;

			ApplicationUser exist = _dbContext.Set<ApplicationUser>().Find(key);

			if (exist != null)
			{
				BeforeUpdate(exist);
				_dbContext.Entry(exist).CurrentValues.SetValues(obj);
				AfterUpdate(obj);
				_dbContext.SaveChanges();
			}
			return exist;
		}

		public virtual async Task<ApplicationUser> UpdateAsync(ApplicationUser obj, object key)
		{
			if (obj == null)
				return null;

			ApplicationUser exist = await _dbContext.Set<ApplicationUser>().FindAsync(key);

			if (exist != null)
			{
				await BeforeUpdateAsync(exist);
				_dbContext.Entry(exist).CurrentValues.SetValues(obj);
				await AfterUpdateAsync(obj);
				await _dbContext.SaveChangesAsync();
			}
			return exist;
		}

		protected virtual void BeforeUpdate(ApplicationUser obj)
		{
		}

		protected virtual void AfterUpdate(ApplicationUser obj)
		{
		}

		protected virtual Task BeforeUpdateAsync(ApplicationUser obj)
		{
			return Task.CompletedTask;
		}

		protected virtual Task AfterUpdateAsync(ApplicationUser obj)
		{
			return Task.CompletedTask;
		}
	}
}
