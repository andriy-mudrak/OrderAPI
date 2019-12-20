using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IServiceBase<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> Get(int id);
        Task<TModel> Detele(TModel model);
        Task<TModel> Create(TModel model);
        Task<IEnumerable<TModel>> Update(IEnumerable<TModel> model);
    }
}