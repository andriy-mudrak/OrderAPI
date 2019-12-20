using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Delete(TEntity entity);
        Task<IEnumerable<TEntity>> Update(IEnumerable<TEntity> entity);
        Task<TEntity> Create(TEntity entity);
    }
}