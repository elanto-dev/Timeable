using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IEventInScheduleService : IEntityServiceBase<EventInSchedule>, IEventInScheduleRepository<EventInSchedule>
    {
        Task<IEnumerable<EventForTimetable>> GetAllEventsForCurrentScheduleAsync(int scheduleId);
    }
}
