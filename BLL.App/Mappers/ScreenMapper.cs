using System;
using BLL.App.Helpers;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class ScreenMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.Screen))
            {
                return MapFromInternal((DalDto.Screen)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.Screen))
            {
                return MapFromExternal((BllDto.Screen)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.Screen MapFromInternal(DalDto.Screen screen)
        {
            var res = screen == null ? null : new BllDto.Screen
            {
                Id = screen.Id,
                UniqueIdentifier = screen.UniqueIdentifier,
                CreatedAt = screen.CreatedAt,
                ChangedAt = screen.ChangedAt,
                CreatedBy = screen.CreatedBy,
                ChangedBy = screen.ChangedBy,
                Prefix = screen.Prefix,
                IsActive = screen.IsActive,
                ShowScheduleSeconds = SecondsValueManager.GetSelectedValue(screen.ShowScheduleSeconds, true)
            };

            return res!;
        }

        public static DalDto.Screen MapFromExternal(BllDto.Screen screen)
        {
            var res = screen == null ? null : new DalDto.Screen
            {
                Id = screen.Id,
                UniqueIdentifier = screen.UniqueIdentifier,
                CreatedAt = screen.CreatedAt,
                ChangedAt = screen.ChangedAt,
                CreatedBy = screen.CreatedBy,
                ChangedBy = screen.ChangedBy,
                Prefix = screen.Prefix,
                IsActive = screen.IsActive,
                ShowScheduleSeconds = SecondsValueManager.GetIntValue(screen.ShowScheduleSeconds)
            };
            return res!;
        }
    }
}
