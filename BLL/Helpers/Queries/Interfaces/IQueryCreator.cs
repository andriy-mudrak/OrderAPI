using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL.Helpers.Queries.Interfaces
{
    public interface IQueryCreator<TEntity> where TEntity : class
    {
        Task<Expression<Func<TEntity, bool>>> Get(int Id);
    }
}