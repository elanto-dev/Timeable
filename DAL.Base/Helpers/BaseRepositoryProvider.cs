using System;
using System.Collections.Generic;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Helpers;
using Contracts.DAL.Base.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base.Helpers
{
    public class BaseRepositoryProvider<TDbContext> : IBaseRepositoryProvider
        where TDbContext : DbContext
    {
        protected readonly Dictionary<Type, object> RepositoryCache;
        protected readonly IBaseRepositoryFactory<TDbContext> RepositoryFactory;
        protected readonly TDbContext DataContext;

        public BaseRepositoryProvider(IBaseRepositoryFactory<TDbContext> repositoryFactory, TDbContext dataContext) :
            this(new Dictionary<Type, object>(), repositoryFactory, dataContext)
        {
        }

        public BaseRepositoryProvider(Dictionary<Type, object> repositoryCache,
            IBaseRepositoryFactory<TDbContext> repositoryFactory, TDbContext dataContext)
        {
            RepositoryCache = repositoryCache;
            RepositoryFactory = repositoryFactory;
            DataContext = dataContext;
        }

        public virtual TRepository GetRepository<TRepository>()
        {
            if (RepositoryCache.ContainsKey(typeof(TRepository)))
            {
                return (TRepository) RepositoryCache[typeof(TRepository)];
            }
            // didn't find the repo in cache, lets create it

            var repoCreationMethod = RepositoryFactory.GetRepositoryFactory<TRepository>();


            object repo = repoCreationMethod(DataContext);


            RepositoryCache[typeof(TRepository)] = repo;
            return (TRepository) repo;
        }


        public virtual IBaseRepository<TDalEntity> GetEntityRepository<TDalEntity, TDomainEntity>()
            where TDalEntity : class, new()
            where TDomainEntity : class, IDomainEntity, new()
        {
            if (RepositoryCache.ContainsKey(typeof(IBaseRepository<TDalEntity>)))
            {
                return (IBaseRepository<TDalEntity>) RepositoryCache[typeof(IBaseRepository<TDalEntity>)];
            }

            // didn't find the repo in cache, lets create it
            var repoCreationMethod = RepositoryFactory.GetEntityRepositoryFactory<TDalEntity, TDomainEntity>();

            object repo = repoCreationMethod(DataContext);


            RepositoryCache[typeof(IBaseRepository<TDalEntity>)] = repo;
            return (IBaseRepository<TDalEntity>) repo;

        }
    }
}
