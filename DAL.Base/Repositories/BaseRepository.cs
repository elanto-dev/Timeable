using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Mappers;
using Contracts.DAL.Base.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base.Repositories
{
    public class BaseRepository<TDalEntity, TDomainEntity, TDbContext> :
        BaseRepository<TDalEntity, TDomainEntity, TDbContext, int>
        where TDalEntity : class, new()
        where TDomainEntity : class, IDomainEntity, new()
        where TDbContext : DbContext
    {
        public BaseRepository(TDbContext repositoryDbContext, IBaseDalMapper mapper) : base(repositoryDbContext, mapper)
        {
        }
    }


    public class BaseRepository<TDalEntity, TDomainEntity, TDbContext, TKey> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
        where TDomainEntity : class, IDomainEntity<TKey>, new()
        where TDbContext : DbContext
        where TKey : struct, IComparable
    {
        protected readonly TDbContext RepositoryDbContext;
        protected readonly DbSet<TDomainEntity> RepositoryDbSet;

        private readonly IBaseDalMapper _mapper;


        protected readonly IDictionary<Guid, TDomainEntity> EntityCreationCache = new Dictionary<Guid, TDomainEntity>();


        public BaseRepository(TDbContext repositoryDbContext, IBaseDalMapper mapper)
        {
            RepositoryDbContext = repositoryDbContext;
            _mapper = mapper;
            // get the dbset by type from db context
            RepositoryDbSet = RepositoryDbContext.Set<TDomainEntity>();
        }


        public virtual TDalEntity Update(TDalEntity entity)
        {
            return _mapper.Map<TDalEntity>(RepositoryDbSet.Update(_mapper.Map<TDomainEntity>(entity)).Entity);
        }

        public virtual void Remove(TDalEntity entity)
        {
            RepositoryDbSet.Remove(_mapper.Map<TDomainEntity>(entity));
        }

        public virtual void Remove(params object[] id)
        {
            RepositoryDbSet.Remove(RepositoryDbSet.Find(id));
        }

        public TDalEntity GetUpdatesAfterUowSaveChanges(Guid guid)
        {
            return EntityCreationCache.ContainsKey(guid) ?
                _mapper.Map<TDalEntity>(EntityCreationCache[guid]) : default!;
        }

        public IEnumerable<TDalEntity> All()
        {
            return RepositoryDbSet.Select(e => _mapper.Map<TDalEntity>(e));
        }

        public virtual async Task<IEnumerable<TDalEntity>> AllAsync()
        {
            return (await RepositoryDbSet.ToListAsync())
                .Select(e => _mapper.Map<TDalEntity>(e));
        }

        public virtual async Task<TDalEntity?> FindAsync(params object[] id)
        {
            return _mapper.Map<TDalEntity>(await RepositoryDbSet.FindAsync(id));
        }

        public virtual async Task<Guid> AddAsync(TDalEntity entity)
        {
            //EntityCreationCache
            var res = (await RepositoryDbSet.AddAsync(_mapper.Map<TDomainEntity>(entity))).Entity;
            var guid = Guid.NewGuid();
            EntityCreationCache.Add(guid, res);
            return guid;
        }

        public TDalEntity Find(params object[] id)
        {
            return _mapper.Map<TDalEntity>(RepositoryDbSet.Find(id));
        }

        public Guid Add(TDalEntity entity)
        {
            var res = RepositoryDbSet.Add(_mapper.Map<TDomainEntity>(entity)).Entity;
            var guid = Guid.NewGuid();
            EntityCreationCache.Add(guid, res);
            return guid;
        }
    }
}
