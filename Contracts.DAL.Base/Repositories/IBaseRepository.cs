using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.DAL.Base.Repositories
{
    public interface IBaseRepository<TDalEntity>
        where TDalEntity : class,  new()
    {
        IEnumerable<TDalEntity> All();
        Task<IEnumerable<TDalEntity>> AllAsync();
        TDalEntity Find(params object[] id);
        Task<TDalEntity?> FindAsync(params object[] id);
        Guid Add(TDalEntity entity);
        Task<Guid> AddAsync(TDalEntity entity);
        TDalEntity Update(TDalEntity entity);
        void Remove(TDalEntity entity);
        void Remove(params object[] id);
        TDalEntity GetUpdatesAfterUowSaveChanges(Guid guid);
    }
}
