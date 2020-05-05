using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class ScheduleMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.Schedule))
            {
                return MapFromInternal((DalDto.Schedule)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(DalDto.Schedule))
            {
                return MapFromExternal((BllDto.Schedule)inObject) as TOutObject ?? default!;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.Schedule MapFromInternal(DalDto.Schedule schedule)
        {
            var res = schedule == null ? null : new BllDto.Schedule
            {
                Id = schedule.Id,
                CreatedAt = schedule.CreatedAt,
                ChangedAt = schedule.ChangedAt,
                CreatedBy = schedule.CreatedBy,
                ChangedBy = schedule.ChangedBy,
                WeekNumber = schedule.WeekNumber,
                Date = schedule.Date,
                Prefix = schedule.Prefix
            };

            return res ?? default!;
        }

        public static DalDto.Schedule MapFromExternal(BllDto.Schedule schedule)
        {
            var res = schedule == null ? null : new DalDto.Schedule
            {
                Id = schedule.Id,
                CreatedAt = schedule.CreatedAt,
                ChangedAt = schedule.ChangedAt,
                CreatedBy = schedule.CreatedBy,
                ChangedBy = schedule.ChangedBy,
                WeekNumber = schedule.WeekNumber,
                Date = schedule.Date,
                Prefix = schedule.Prefix
            };
            return res ?? default!;
        }
    }
}
