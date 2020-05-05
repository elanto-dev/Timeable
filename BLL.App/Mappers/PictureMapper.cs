using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class PictureMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.Picture))
            {
                return MapFromInternal((DalDto.Picture)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.Picture))
            {
                return MapFromExternal((BllDto.Picture)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.Picture MapFromInternal(DalDto.Picture picture)
        {
            var res = picture == null ? null : new BllDto.Picture
            {
                Id = picture.Id,
                CreatedAt = picture.CreatedAt,
                ChangedAt = picture.ChangedAt,
                CreatedBy = picture.CreatedBy,
                ChangedBy = picture.ChangedBy,
                Path = picture.Path,
                Comment = picture.Comment
            };

            return res;
        }

        public static DalDto.Picture MapFromExternal(BllDto.Picture picture)
        {
            var res = picture == null ? null : new DalDto.Picture
            {
                Id = picture.Id,
                CreatedAt = picture.CreatedAt,
                ChangedAt = picture.ChangedAt,
                CreatedBy = picture.CreatedBy,
                ChangedBy = picture.ChangedBy,
                Path = picture.Path,
                Comment = picture.Comment
            };
            return res;
        }
    }
}
