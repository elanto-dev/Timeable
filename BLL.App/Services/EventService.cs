using System;
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
    public class EventService : BaseEntityService<Event, DAL.DTO.Event, IAppUnitOfWork>, IEventService
    {
        public EventService(IAppUnitOfWork uow) : base(uow, new EventMapper())
        {
            ServiceRepository = Uow.Events;
        }

        public async Task<IEnumerable<Event>> GetAllFutureEventsAsync(DateTime startDate)
        {
            return (await Uow.Events.GetAllFutureEventsAsync(startDate)).Select(EventMapper.MapFromInternal);
        }

        public async Task<IEnumerable<EventForSettings>> GetAllFutureEventsForSettingsAsync(DateTime startDate)
        {
            return (await Uow.Events.GetAllFutureEventsAsync(startDate)).Select(EventInScheduleToSettingsMapper.MapFromInternal);
        }
    }
}
