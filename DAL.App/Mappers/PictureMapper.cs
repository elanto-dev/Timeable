using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class PictureMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.Picture))
            {
                return MapFromDomain((Picture)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(Picture))
            {
                return MapFromDal((DTO.Picture)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.Picture MapFromDomain(Picture picture)
        {
            var res = picture == null ? null : new DTO.Picture
            {
                Id = picture.Id,
                CreatedAt = picture.CreatedAt,
                ChangedAt = picture.ChangedAt,
                CreatedBy = picture.CreatedBy,
                ChangedBy = picture.ChangedBy,
                Path = picture.Path,
                Comment = picture.Comment
            };

            return res ?? default!;
        }

        public static Picture MapFromDal(DTO.Picture picture)
        {
            var res = picture == null ? null : new Picture
            {
                Id = picture.Id,
                CreatedAt = picture.CreatedAt,
                ChangedAt = picture.ChangedAt,
                CreatedBy = picture.CreatedBy,
                ChangedBy = picture.ChangedBy,
                Path = picture.Path,
                Comment = picture.Comment
            };

            return res ?? default!;
        }
    }
}
