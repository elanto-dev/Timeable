using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class ScheduleMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.Schedule))
            {
                return MapFromDomain((Schedule)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(Schedule))
            {
                return MapFromDal((DTO.Schedule)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.Schedule MapFromDomain(Schedule schedule)
        {
            var res = schedule == null ? null : new DTO.Schedule
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

        public static Schedule MapFromDal(DTO.Schedule schedule)
        {
            var res = schedule == null ? null : new Schedule
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
