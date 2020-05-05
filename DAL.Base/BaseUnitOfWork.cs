using System.Threading.Tasks;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Helpers;
using Contracts.DAL.Base.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base
{
    public class BaseUnitOfWork<TDbContext> : IBaseUnitOfWork
        where TDbContext : DbContext
    {
        protected readonly TDbContext UowDbContext;
        protected readonly IBaseRepositoryProvider RepositoryProvider;


        public BaseUnitOfWork(TDbContext dataContext, IBaseRepositoryProvider repositoryProvider)
        {
            RepositoryProvider = repositoryProvider;
            UowDbContext = dataContext;
        }

        public IBaseRepository<TDALEntity> BaseRepository<TDALEntity, TDomainEntity>()
            where TDomainEntity : class, IDomainEntity, new()
            where TDALEntity : class, new()
        {
            return RepositoryProvider.GetEntityRepository<TDALEntity, TDomainEntity>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await UowDbContext.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return UowDbContext.SaveChanges();
        }
    }
}
