using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class SubjectMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.Subject))
            {
                return MapFromDomain((Subject)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(Subject))
            {
                return MapFromDal((DTO.Subject)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.Subject MapFromDomain(Subject subject)
        {
            var res = subject == null ? null : new DTO.Subject
            {
                Id = subject.Id,
                CreatedAt = subject.CreatedAt,
                ChangedAt = subject.ChangedAt,
                CreatedBy = subject.CreatedBy,
                ChangedBy = subject.ChangedBy,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName
            };

            return res ?? default!;
        }

        public static Subject MapFromDal(DTO.Subject subject)
        {
            var res = subject == null ? null : new Subject
            {
                Id = subject.Id,
                CreatedAt = subject.CreatedAt,
                ChangedAt = subject.ChangedAt,
                CreatedBy = subject.CreatedBy,
                ChangedBy = subject.ChangedBy,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName
            };

            return res ?? default!;
        }
    }
}
