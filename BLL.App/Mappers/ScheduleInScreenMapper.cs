using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class ScheduleInScreenMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.ScheduleInScreen))
            {
                return MapFromInternal((DalDto.ScheduleInScreen)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.ScheduleInScreen))
            {
                return MapFromExternal((BllDto.ScheduleInScreen)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.ScheduleInScreen MapFromInternal(DalDto.ScheduleInScreen scheduleInScreen)
        {
            var res = scheduleInScreen == null ? null : new BllDto.ScheduleInScreen
            {
                Id = scheduleInScreen.Id,
                CreatedAt = scheduleInScreen.CreatedAt,
                ChangedAt = scheduleInScreen.ChangedAt,
                CreatedBy = scheduleInScreen.CreatedBy,
                ChangedBy = scheduleInScreen.ChangedBy,
                ScheduleId = scheduleInScreen.ScheduleId,
                Schedule = ScheduleMapper.MapFromInternal(scheduleInScreen.Schedule),
                ScreenId = scheduleInScreen.ScreenId,
                Screen = ScreenMapper.MapFromInternal(scheduleInScreen.Screen)
            };

            return res;
        }

        public static DalDto.ScheduleInScreen MapFromExternal(BllDto.ScheduleInScreen scheduleInScreen)
        {
            var res = scheduleInScreen == null ? null : new DalDto.ScheduleInScreen
            {
                Id = scheduleInScreen.Id,
                CreatedAt = scheduleInScreen.CreatedAt,
                ChangedAt = scheduleInScreen.ChangedAt,
                CreatedBy = scheduleInScreen.CreatedBy,
                ChangedBy = scheduleInScreen.ChangedBy,
                ScheduleId = scheduleInScreen.ScheduleId,
                Schedule = ScheduleMapper.MapFromExternal(scheduleInScreen.Schedule),
                ScreenId = scheduleInScreen.ScreenId,
                Screen = ScreenMapper.MapFromExternal(scheduleInScreen.Screen)
            };
            return res;
        }
    }
}
