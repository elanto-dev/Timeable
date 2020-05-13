using System;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class PictureInScreenToPromotionMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.PromotionsForTimetable))
            {
                return MapFromInternal((DAL.DTO.PictureInScreen)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.PromotionsForTimetable MapFromInternal(DAL.DTO.PictureInScreen pictureInScreen)
        {
            var res = pictureInScreen == null ? null : new DTO.PromotionsForTimetable
            {
                Picture = PictureMapper.MapFromInternal(pictureInScreen.Picture),
                ShowAddSeconds = pictureInScreen.ShowAddSeconds
            };

            return res ?? default!;
        }
    }
}
