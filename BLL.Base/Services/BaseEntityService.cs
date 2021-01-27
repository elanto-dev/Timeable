using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.Base.Mappers;
using Contracts.BLL.Base.Services;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Repositories;

namespace BLL.Base.Services
{
    public abstract class BaseEntityService<TBllEntity, TDalEntity, TUnitOfWork> : IEntityServiceBase<TBllEntity>
        where TBllEntity : class, new()
        where TDalEntity : class, new()
    where TUnitOfWork : IBaseUnitOfWork
    {
        protected readonly TUnitOfWork Uow;
        protected IBaseRepository<TDalEntity> ServiceRepository = default!;
        private readonly IBLLMapperBase _mapper;

        protected BaseEntityService(TUnitOfWork uow, IBLLMapperBase mapper)
        {
            Uow = uow;
            _mapper = mapper;
        }

        public virtual TBllEntity Update(TBllEntity entity)
        {
            return _mapper.Map<TBllEntity>(ServiceRepository.Update(_mapper.Map<TDalEntity>(entity)));
        }

        public virtual void Remove(TBllEntity entity)
        {
            ServiceRepository.Remove(_mapper.Map<TDalEntity>(entity));
        }

        public virtual void Remove(params object[] id)
        {
            ServiceRepository.Remove(id);
        }

        public TBllEntity GetUpdatesAfterUowSaveChanges(Guid guid)
        {
            return _mapper.Map<TBllEntity>(ServiceRepository.GetUpdatesAfterUowSaveChanges(guid));

        }

        public virtual async Task<IEnumerable<TBllEntity>> AllAsync()
        {
            return (await ServiceRepository.AllAsync()).Select(e => _mapper.Map<TBllEntity>(e)).ToList();
        }

        public virtual async Task<TBllEntity?> FindAsync(params object[] id)
        {
            return _mapper.Map<TBllEntity>(await ServiceRepository.FindAsync(id));
        }

        public virtual async Task<Guid> AddAsync(TBllEntity entity)
        {
            return await ServiceRepository.AddAsync(_mapper.Map<TDalEntity>(entity));
        }

        public IEnumerable<TBllEntity> All()
        {
            return ServiceRepository.All().Select(e => _mapper.Map<TBllEntity>(e)).ToList();
        }

        public TBllEntity Find(params object[] id)
        {
            return _mapper.Map<TBllEntity>(ServiceRepository.Find(id));
        }

        public Guid Add(TBllEntity entity)
        {
            return ServiceRepository.Add(_mapper.Map<TDalEntity>(entity));
        }
    }
}
