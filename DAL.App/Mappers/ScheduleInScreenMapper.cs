using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class ScheduleInScreenMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.ScheduleInScreen))
            {
                return MapFromDomain((ScheduleInScreen)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(ScheduleInScreen))
            {
                return MapFromDal((DTO.ScheduleInScreen)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.ScheduleInScreen MapFromDomain(ScheduleInScreen scheduleInScreen)
        {
            var res = scheduleInScreen == null ? null : new DTO.ScheduleInScreen
            {
                Id = scheduleInScreen.Id,
                CreatedAt = scheduleInScreen.CreatedAt,
                ChangedAt = scheduleInScreen.ChangedAt,
                CreatedBy = scheduleInScreen.CreatedBy,
                ChangedBy = scheduleInScreen.ChangedBy,
                ScheduleId = scheduleInScreen.ScheduleId,
                Schedule = ScheduleMapper.MapFromDomain(scheduleInScreen.Schedule),
                ScreenId = scheduleInScreen.ScreenId,
                Screen = ScreenMapper.MapFromDomain(scheduleInScreen.Screen)
            };

            return res ?? default!;
        }

        public static ScheduleInScreen MapFromDal(DTO.ScheduleInScreen scheduleInScreen)
        {
            var res = scheduleInScreen == null ? null : new ScheduleInScreen
            {
                Id = scheduleInScreen.Id,
                CreatedAt = scheduleInScreen.CreatedAt,
                ChangedAt = scheduleInScreen.ChangedAt,
                CreatedBy = scheduleInScreen.CreatedBy,
                ChangedBy = scheduleInScreen.ChangedBy,
                ScheduleId = scheduleInScreen.ScheduleId,
                Schedule = ScheduleMapper.MapFromDal(scheduleInScreen.Schedule),
                ScreenId = scheduleInScreen.ScreenId,
                Screen = ScreenMapper.MapFromDal(scheduleInScreen.Screen)
            };

            return res ?? default!;
        }
    }
}
