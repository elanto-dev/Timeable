using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class EventInScheduleMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.EventInSchedule))
            {
                return MapFromInternal((DalDto.EventInSchedule)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.EventInSchedule))
            {
                return MapFromExternal((BllDto.EventInSchedule)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.EventInSchedule MapFromInternal(DalDto.EventInSchedule eventInSchedule)
        {
            var res = eventInSchedule == null ? null : new BllDto.EventInSchedule
            {
                Id = eventInSchedule.Id,
                CreatedAt = eventInSchedule.CreatedAt,
                ChangedAt = eventInSchedule.ChangedAt,
                CreatedBy = eventInSchedule.CreatedBy,
                ChangedBy = eventInSchedule.ChangedBy,
                ScheduleId = eventInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromInternal(eventInSchedule.Schedule),
                Event = EventMapper.MapFromInternal(eventInSchedule.Event),
                EventId = eventInSchedule.EventId
            };

            return res;
        }

        public static DalDto.EventInSchedule MapFromExternal(BllDto.EventInSchedule eventInSchedule)
        {
            var res = eventInSchedule == null ? null : new DalDto.EventInSchedule
            {
                Id = eventInSchedule.Id,
                CreatedAt = eventInSchedule.CreatedAt,
                ChangedAt = eventInSchedule.ChangedAt,
                CreatedBy = eventInSchedule.CreatedBy,
                ScheduleId = eventInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromExternal(eventInSchedule.Schedule),
                Event = EventMapper.MapFromExternal(eventInSchedule.Event),
                EventId = eventInSchedule.EventId,
                ChangedBy = eventInSchedule.ChangedBy,
            };
            return res;
        }
    }
}
