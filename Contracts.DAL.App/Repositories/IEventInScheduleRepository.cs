using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IEventInScheduleRepository : IEventInScheduleRepository<DalAppDTO.EventInSchedule>
    {
    }

    public interface IEventInScheduleRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        /// <summary>
        /// Returns all the EventInSchedule for the schedule.
        /// </summary>
        /// <param name="scheduleId">Schedule ID</param>
        /// <returns>EventInSchedule entities</returns>
        Task<IEnumerable<TDalEntity>> GetAllEventsForScheduleAsync(int scheduleId);
    }
}
