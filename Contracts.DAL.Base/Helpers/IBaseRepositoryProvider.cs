using Contracts.DAL.Base.Repositories;

namespace Contracts.DAL.Base.Helpers
{
    public interface IBaseRepositoryProvider
    {
        TRepository GetRepository<TRepository>();

        IBaseRepository<TDalEntity> GetEntityRepository<TDalEntity, TDomainEntity>()
            where TDalEntity : class, new()
            where TDomainEntity : class, IDomainEntity, new();
    }
}
