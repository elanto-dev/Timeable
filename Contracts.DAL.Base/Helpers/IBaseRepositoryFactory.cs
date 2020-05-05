using System;
using Microsoft.EntityFrameworkCore;

namespace Contracts.DAL.Base.Helpers
{
    public interface IBaseRepositoryFactory<TDbContext>
        where TDbContext : DbContext
    {
        void AddToCreationMethods<TRepository>(Func<TDbContext, TRepository> creationMethod)
            where TRepository : class;

        Func<TDbContext, object> GetRepositoryFactory<TRepository>();

        Func<TDbContext, object> GetEntityRepositoryFactory<TDalEntity, TDomainEntity>()
            where TDalEntity : class, new()
            where TDomainEntity : class, IDomainEntity, new();



    }
}
