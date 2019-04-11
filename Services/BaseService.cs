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
		private readonly IValidator _validator;
		private readonly IRepositoryBase<TEntity> _repository;

		public BaseService(IRedisConnectionFactory connectionFactory, IValidator<TEntity> validator, IRepositoryBase<TEntity> repository) : base(connectionFactory)
		{
			_validator = validator;
			_repository = repository;
		}

		public virtual void Delete(object id)
		{
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

		public virtual TEntity Find(Expression<Func<TEntity, bool>> match)
		{
			return _dbContext.Set<TEntity>().SingleOrDefault(match);
		}

		public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
		{
		}

		public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match)
		{
		}

		public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
		{
		}

		public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
		{
		}

		public virtual async Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
		{
		}

		public IQueryable<TEntity> GetAll()
		{
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
		}

		public virtual async Task<TEntity> GetByIdAsync(object id)
		{
			return await _dbContext.Set<TEntity>().FindAsync(id);
		}

		public virtual TEntity Insert(TEntity obj)
		{
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

		public virtual TEntity Update(TEntity obj, object key)
		{
		}

		public virtual async Task<TEntity> UpdateAsync(TEntity obj, object key)
		{
		}

	}
}
