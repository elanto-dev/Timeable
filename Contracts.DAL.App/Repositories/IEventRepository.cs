using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IEventRepository : IEventRepository<DalAppDTO.Event>
    {
    }

    public interface IEventRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        Task<IEnumerable<TDalEntity>> GetAllFutureEventsAsync(DateTime startDate);
    }
}
