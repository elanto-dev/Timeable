using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class AppUsersScreenMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.AppUsersScreen))
            {
                return MapFromInternal((DalDto.AppUsersScreen)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(DalDto.AppUsersScreen))
            {
                return MapFromExternal((BllDto.AppUsersScreen)inObject) as TOutObject ?? default!;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.AppUsersScreen MapFromInternal(DalDto.AppUsersScreen appUsersScreen)
        {
            var res = appUsersScreen == null ? null : new BllDto.AppUsersScreen
            {
                Id = appUsersScreen.Id,
                CreatedAt = appUsersScreen.CreatedAt,
                ChangedAt = appUsersScreen.ChangedAt,
                CreatedBy = appUsersScreen.CreatedBy,
                ChangedBy = appUsersScreen.ChangedBy,
                AppUserId = appUsersScreen.AppUserId,
                Screen = ScreenMapper.MapFromInternal(appUsersScreen.Screen),
                ScreenId = appUsersScreen.ScreenId
            };

            return res ?? default!;
        }

        public static DalDto.AppUsersScreen MapFromExternal(BllDto.AppUsersScreen appUsersScreen)
        {
            var res = appUsersScreen == null ? null : new DalDto.AppUsersScreen
            {
                Id = appUsersScreen.Id,
                CreatedAt = appUsersScreen.CreatedAt,
                ChangedAt = appUsersScreen.ChangedAt,
                CreatedBy = appUsersScreen.CreatedBy,
                ChangedBy = appUsersScreen.ChangedBy,
                AppUserId = appUsersScreen.AppUserId,
                Screen = ScreenMapper.MapFromExternal(appUsersScreen.Screen),
                ScreenId = appUsersScreen.ScreenId
            };
            return res ?? default!;
        }
    }
}
