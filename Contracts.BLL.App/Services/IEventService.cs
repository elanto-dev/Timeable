using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IEventService : IEntityServiceBase<Event>, IEventRepository<Event>
    {
        /// <summary>
        /// Returns all events that are happening in the future or have not being finished yet.
        /// Events are shown in schedule settings view.
        /// </summary>
        /// <param name="currentDateTime">Time with which to compare.</param>
        /// <returns></returns>
        Task<IEnumerable<EventForSettings>> GetAllFutureEventsForSettingsAsync(DateTime currentDateTime);
    }
}
