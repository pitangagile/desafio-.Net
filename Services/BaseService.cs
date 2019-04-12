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

		public virtual async Task<TEntity> RemoveByIdAsync(object id)
		{
			return await _repository.RemoveByIdAsync(id);
		}

		public IQueryable<TEntity> GetAll()
		{
			return this._repository.GetAll();
		}

		public virtual async Task<ICollection<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			return await this._repository.GetAllIncludingAsync(false, includeProperties);
		}

		public virtual async Task<TEntity> FindByIdAsync(object id)
		{
			return await this._repository.FindByIdAsync(id);
		}

		public virtual Task UpdateAsync(TEntity obj)
		{
			return this._repository.UpdateAsync(obj);
		}

	}
}
