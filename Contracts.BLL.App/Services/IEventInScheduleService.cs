using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IEventInScheduleService : IEntityServiceBase<EventInSchedule>, IEventInScheduleRepository<EventInSchedule>
    {
        /// <summary>
        /// Returns all the events that were included to the current schedule.
        /// </summary>
        /// <param name="scheduleId">Schedule ID.</param>
        /// <returns></returns>
        Task<IEnumerable<EventForTimetable>> GetAllEventsForCurrentScheduleAsync(int scheduleId);
    }
}
