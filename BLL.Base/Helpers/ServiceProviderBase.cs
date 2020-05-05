using System;
using System.Collections.Generic;
using Contracts.BLL.Base.Helpers;
using Contracts.DAL.Base;

namespace BLL.Base.Helpers
{
    public class ServiceProviderBase<TUnitOfWork> : IServiceProviderBase
        where TUnitOfWork : IBaseUnitOfWork
    {
        protected readonly Dictionary<Type, object> ServiceCache;
        protected readonly IServiceFactoryBase<TUnitOfWork> ServiceFactory;
        protected readonly TUnitOfWork Uow;


        public ServiceProviderBase(IServiceFactoryBase<TUnitOfWork> serviceFactory, TUnitOfWork uow) : this(
            new Dictionary<Type, object>(), serviceFactory, uow)
        {
        }

        public ServiceProviderBase(Dictionary<Type, object> serviceCache,
            IServiceFactoryBase<TUnitOfWork> serviceFactory,
            TUnitOfWork uow)
        {
            ServiceCache = serviceCache;
            ServiceFactory = serviceFactory;
            Uow = uow;
        }

        public virtual TService GetService<TService>()
        {
            if (ServiceCache.ContainsKey(typeof(TService)))
            {
                return (TService)ServiceCache[typeof(TService)];
            }
            // didn't find the repo in cache, lets create it

            var repoCreationMethod = ServiceFactory.GetServiceFactory<TService>();


            object repo = repoCreationMethod(Uow);


            ServiceCache[typeof(TService)] = repo;
            return (TService)repo;
        }
    }
}
