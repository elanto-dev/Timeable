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
        /// <summary>
        /// Returns all future events which end datetime or show end datetime is later that current time. 
        /// </summary>
        /// <param name="currentTime">Current DateTime</param>
        /// <returns>Event entities</returns>
        Task<IEnumerable<TDalEntity>> GetAllFutureEventsAsync(DateTime currentTime);
    }
}
