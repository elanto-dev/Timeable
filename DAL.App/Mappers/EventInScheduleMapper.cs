using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class EventInScheduleMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.EventInSchedule))
            {
                return MapFromDomain((EventInSchedule)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(EventInSchedule))
            {
                return MapFromDal((DTO.EventInSchedule)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.EventInSchedule MapFromDomain(EventInSchedule eventInSchedule)
        {
            var res = eventInSchedule == null ? null : new DTO.EventInSchedule
            {
                Id = eventInSchedule.Id,
                CreatedAt = eventInSchedule.CreatedAt,
                ChangedAt = eventInSchedule.ChangedAt,
                CreatedBy = eventInSchedule.CreatedBy,
                ChangedBy = eventInSchedule.ChangedBy,
                ScheduleId = eventInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromDomain(eventInSchedule.Schedule),
                Event = EventMapper.MapFromDomain(eventInSchedule.Event),
                EventId = eventInSchedule.EventId
            };

            return res ?? default!;
        }

        public static EventInSchedule MapFromDal(DTO.EventInSchedule eventInSchedule)
        {
            var res = eventInSchedule == null ? null : new EventInSchedule
            {
                Id = eventInSchedule.Id,
                CreatedAt = eventInSchedule.CreatedAt,
                ChangedAt = eventInSchedule.ChangedAt,
                CreatedBy = eventInSchedule.CreatedBy,
                ChangedBy = eventInSchedule.ChangedBy,
                ScheduleId = eventInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromDal(eventInSchedule.Schedule),
                Event = EventMapper.MapFromDal(eventInSchedule.Event),
                EventId = eventInSchedule.EventId
            };

            return res ?? default!;
        }
    }
}
