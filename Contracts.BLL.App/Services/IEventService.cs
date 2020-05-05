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
        Task<IEnumerable<EventForSettings>> GetAllFutureEventsForSettingsAsync(DateTime startDate);
    }
}
