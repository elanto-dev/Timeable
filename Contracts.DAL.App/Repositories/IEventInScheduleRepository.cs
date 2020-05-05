using System;
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
        Task<IEnumerable<TDalEntity>> GetAllEventsForScheduleAsync(int scheduleId);
    }
}
