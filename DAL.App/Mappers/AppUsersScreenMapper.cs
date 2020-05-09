using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class AppUsersScreenMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject) 
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.AppUsersScreen))
            {
                return MapFromDomain((AppUsersScreen)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(AppUsersScreen))
            {
                return MapFromDal((DTO.AppUsersScreen)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.AppUsersScreen MapFromDomain(AppUsersScreen appUsersScreen)
        {
            var res = appUsersScreen == null ? null : new DTO.AppUsersScreen
            {
                Id = appUsersScreen.Id,
                CreatedAt = appUsersScreen.CreatedAt,
                ChangedAt = appUsersScreen.ChangedAt,
                CreatedBy = appUsersScreen.CreatedBy,
                ChangedBy = appUsersScreen.ChangedBy,
                AppUserId = appUsersScreen.AppUserId,
                Screen = ScreenMapper.MapFromDomain(appUsersScreen.Screen),
                ScreenId = appUsersScreen.ScreenId
            };

            return res ?? default!;
        }

        public static AppUsersScreen MapFromDal(DTO.AppUsersScreen appUsersScreen)
        {
            var res = appUsersScreen == null ? null : new AppUsersScreen
            {
                Id = appUsersScreen.Id,
                CreatedAt = appUsersScreen.CreatedAt,
                ChangedAt = appUsersScreen.ChangedAt,
                CreatedBy = appUsersScreen.CreatedBy,
                ChangedBy = appUsersScreen.ChangedBy,
                AppUserId = appUsersScreen.AppUserId,
                Screen = ScreenMapper.MapFromDal(appUsersScreen.Screen),
                ScreenId = appUsersScreen.ScreenId
            };

            return res ?? default!;
        }
    }
}
