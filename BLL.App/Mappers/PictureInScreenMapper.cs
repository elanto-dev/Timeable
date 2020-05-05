using System;
using BLL.App.Helpers;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class PictureInScreenMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.PictureInScreen))
            {
                return MapFromInternal((DalDto.PictureInScreen)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.PictureInScreen))
            {
                return MapFromExternal((BllDto.PictureInScreen)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.PictureInScreen MapFromInternal(DalDto.PictureInScreen pictureInScreen)
        {
            var res = pictureInScreen == null ? null : new BllDto.PictureInScreen
            {
                Id = pictureInScreen.Id,
                CreatedAt = pictureInScreen.CreatedAt,
                ChangedAt = pictureInScreen.ChangedAt,
                CreatedBy = pictureInScreen.CreatedBy,
                ChangedBy = pictureInScreen.ChangedBy,
                PictureId = pictureInScreen.PictureId,
                Picture = PictureMapper.MapFromInternal(pictureInScreen.Picture),
                ScreenId = pictureInScreen.ScreenId,
                Screen = ScreenMapper.MapFromInternal(pictureInScreen.Screen)!,
                IsBackgroundPicture = pictureInScreen.IsBackgroundPicture,
                ShowAddSeconds = SecondsValueManager.GetSelectedValue(pictureInScreen.ShowAddSeconds, false)
            };

            return res!;
        }

        public static DalDto.PictureInScreen MapFromExternal(BllDto.PictureInScreen pictureInScreen)
        {
            var res = pictureInScreen == null ? null : new DalDto.PictureInScreen
            {
                Id = pictureInScreen.Id,
                CreatedAt = pictureInScreen.CreatedAt,
                ChangedAt = pictureInScreen.ChangedAt,
                CreatedBy = pictureInScreen.CreatedBy,
                ChangedBy = pictureInScreen.ChangedBy,
                PictureId = pictureInScreen.PictureId,
                Picture = PictureMapper.MapFromExternal(pictureInScreen.Picture),
                ScreenId = pictureInScreen.ScreenId,
                Screen = ScreenMapper.MapFromExternal(pictureInScreen.Screen),
                IsBackgroundPicture = pictureInScreen.IsBackgroundPicture,
                ShowAddSeconds = SecondsValueManager.GetIntValue(pictureInScreen.ShowAddSeconds)
            };
            return res!;
        }
    }
}
