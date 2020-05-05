using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class ScreenMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.Screen))
            {
                return MapFromDomain((Screen)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(Screen))
            {
                return MapFromDal((DTO.Screen)inObject) as TOutObject;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.Screen MapFromDomain(Screen screen)
        {
            var res = screen == null ? null : new DTO.Screen
            {
                Id = screen.Id,
                UniqueIdentifier = screen.UniqueIdentifier,
                CreatedAt = screen.CreatedAt,
                ChangedAt = screen.ChangedAt,
                CreatedBy = screen.CreatedBy,
                ChangedBy = screen.ChangedBy,
                Prefix = screen.Prefix,
                IsActive = screen.IsActive,
                ShowScheduleSeconds = screen.ShowScheduleSeconds
            };

            return res!;
        }

        public static Screen MapFromDal(DTO.Screen screen)
        {
            var res = screen == null ? null : new Screen
            {
                Id = screen.Id,
                UniqueIdentifier = screen.UniqueIdentifier,
                CreatedAt = screen.CreatedAt,
                ChangedAt = screen.ChangedAt,
                CreatedBy = screen.CreatedBy,
                ChangedBy = screen.ChangedBy,
                Prefix = screen.Prefix,
                IsActive = screen.IsActive,
                ShowScheduleSeconds = screen.ShowScheduleSeconds
            };

            return res!;
        }
    }
}
