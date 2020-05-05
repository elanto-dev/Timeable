using System;
using System.Collections.Generic;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Helpers;
using DAL.Base.Mappers;
using DAL.Base.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base.Helpers
{
    public class BaseRepositoryFactory<TDbContext> : IBaseRepositoryFactory<TDbContext>
    where TDbContext : DbContext
{
    private readonly Dictionary<Type, Func<TDbContext, object>> _repositoryCreationMethodCache;

    public BaseRepositoryFactory() : this(new Dictionary<Type, Func<TDbContext, object>>())
    {
    }

    public BaseRepositoryFactory(Dictionary<Type, Func<TDbContext, object>> repositoryCreationMethods)
    {
        _repositoryCreationMethodCache = repositoryCreationMethods;
    }

    public void AddToCreationMethods<TRepository>(Func<TDbContext, TRepository> creationMethod)
        where TRepository : class
    {
        _repositoryCreationMethodCache.Add(typeof(TRepository), creationMethod);
    }


    public Func<TDbContext, object> GetRepositoryFactory<TRepository>()
    {
        if (_repositoryCreationMethodCache.ContainsKey(typeof(TRepository)))
        {
            return _repositoryCreationMethodCache[typeof(TRepository)];
        }

        throw new NullReferenceException("No repo creation method found for " + typeof(TRepository).FullName);
    }

    public Func<TDbContext, object> GetEntityRepositoryFactory<TDALEntity, TDomainEntity>()
        where TDALEntity : class, new()
        where TDomainEntity : class, IDomainEntity, new()
    {
        return dataContext =>
            new BaseRepository<TDALEntity, TDomainEntity, TDbContext>(dataContext,
                new BaseDalMapper<TDALEntity, TDomainEntity>());
    }
}
}
