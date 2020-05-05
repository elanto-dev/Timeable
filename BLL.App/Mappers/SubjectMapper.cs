using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class SubjectMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.Subject))
            {
                return MapFromInternal((DalDto.Subject)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.Subject))
            {
                return MapFromExternal((BllDto.Subject)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.Subject MapFromInternal(DalDto.Subject subject)
        {
            var res = subject == null ? null : new BllDto.Subject
            {
                Id = subject.Id,
                CreatedAt = subject.CreatedAt,
                ChangedAt = subject.ChangedAt,
                CreatedBy = subject.CreatedBy,
                ChangedBy = subject.ChangedBy,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName
            };

            return res;
        }

        public static DalDto.Subject MapFromExternal(BllDto.Subject subject)
        {
            var res = subject == null ? null : new DalDto.Subject
            {
                Id = subject.Id,
                CreatedAt = subject.CreatedAt,
                ChangedAt = subject.ChangedAt,
                CreatedBy = subject.CreatedBy,
                ChangedBy = subject.ChangedBy,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName
            };
            return res;
        }
    }
}
