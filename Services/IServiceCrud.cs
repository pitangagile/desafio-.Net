using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Services
{
    public interface IServiceCrud<TEntity> : IServiceCrud where TEntity : class
	{
		Task<TEntity> RemoveByIdAsync(object id);
        Task<TEntity> FindByIdAsync(object id);
        IQueryable<TEntity> GetAll();
        Task<ICollection<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task UpdateAsync(TEntity obj);
	}
}
