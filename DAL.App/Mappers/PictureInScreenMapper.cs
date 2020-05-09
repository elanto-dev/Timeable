using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class PictureInScreenMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.PictureInScreen))
            {
                return MapFromDomain((PictureInScreen)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(PictureInScreen))
            {
                return MapFromDal((DTO.PictureInScreen)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.PictureInScreen MapFromDomain(PictureInScreen pictureInScreen)
        {
            var res = pictureInScreen == null ? null : new DTO.PictureInScreen
            {
                Id = pictureInScreen.Id,
                CreatedAt = pictureInScreen.CreatedAt,
                ChangedAt = pictureInScreen.ChangedAt,
                CreatedBy = pictureInScreen.CreatedBy,
                ChangedBy = pictureInScreen.ChangedBy,
                PictureId = pictureInScreen.PictureId,
                Picture = PictureMapper.MapFromDomain(pictureInScreen.Picture),
                ScreenId = pictureInScreen.ScreenId,
                Screen = ScreenMapper.MapFromDomain(pictureInScreen.Screen),
                IsBackgroundPicture = pictureInScreen.IsBackgroundPicture,
                ShowAddSeconds = pictureInScreen.ShowAddSeconds
            };

            return res ?? default!;
        }

        public static PictureInScreen MapFromDal(DTO.PictureInScreen pictureInScreen)
        {
            var res = pictureInScreen == null ? null : new PictureInScreen
            {
                Id = pictureInScreen.Id,
                CreatedAt = pictureInScreen.CreatedAt,
                ChangedAt = pictureInScreen.ChangedAt,
                CreatedBy = pictureInScreen.CreatedBy,
                ChangedBy = pictureInScreen.ChangedBy,
                PictureId = pictureInScreen.PictureId,
                Picture = PictureMapper.MapFromDal(pictureInScreen.Picture),
                ScreenId = pictureInScreen.ScreenId,
                Screen = ScreenMapper.MapFromDal(pictureInScreen.Screen),
                IsBackgroundPicture = pictureInScreen.IsBackgroundPicture,
                ShowAddSeconds = pictureInScreen.ShowAddSeconds
            };

            return res ?? default!;
        }
    }
}
