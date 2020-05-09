using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class EventInScheduleService : BaseEntityService<EventInSchedule, DAL.DTO.EventInSchedule, IAppUnitOfWork>, IEventInScheduleService
    {
        public EventInScheduleService(IAppUnitOfWork uow) : base(uow, new EventInScheduleMapper())
        {
            ServiceRepository = Uow.EventInSchedules;
        }

        public async Task<IEnumerable<EventInSchedule>> GetAllEventsForScheduleAsync(int scheduleId)
        {
            return (await Uow.EventInSchedules.GetAllEventsForScheduleAsync(scheduleId)).Select(EventInScheduleMapper.MapFromInternal);
        }

        public async Task<IEnumerable<EventForTimetable>> GetAllEventsForCurrentScheduleAsync(int scheduleId)
        {
            return (await Uow.EventInSchedules.GetAllEventsForScheduleAsync(scheduleId)).Select(
                EventInScheduleToTimetableMapper.MapFromInternal);
        }
    }
}
