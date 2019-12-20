using BLL.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Helpers.Queries.Interfaces;
using DAL.Repositories.Interfaces;

namespace BLL.Services
{
    public class ServiceBase<TModel, TEntity> : IServiceBase<TModel>
        where TModel : class
        where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _repositoryBase;
        private readonly IQueryCreator<TEntity> _queryCreator;
        private readonly IMapper _mapper;

        public ServiceBase(IRepositoryBase<TEntity> repositoryBase, IQueryCreator<TEntity> queryCreator, IMapper mapper)
        {
            _repositoryBase = repositoryBase;
            _mapper = mapper;
            _queryCreator = queryCreator;
        }

        public async Task<TModel> Create(TModel model)
        {
            var result = await _repositoryBase.Create(_mapper.Map<TEntity>(model));
            return _mapper.Map<TModel>(result);
        }

        public async Task<TModel> Detele(TModel model)
        {
            var result = await _repositoryBase.Delete(_mapper.Map<TEntity>(model));
            return _mapper.Map<TModel>(result);
        }

        public async Task<IEnumerable<TModel>> Update(IEnumerable<TModel> model)
        {
            var result = await _repositoryBase.Update(_mapper.Map<IEnumerable<TEntity>>(model));
            return _mapper.Map<IEnumerable<TModel>>(result);
        }

        public async Task<IEnumerable<TModel>> Get(int id)
        {
            var query = await _queryCreator.Get(id);
            var items = await _repositoryBase.Get(query); 
            return _mapper.Map<IEnumerable<TModel>>(items);
        }
    }
}