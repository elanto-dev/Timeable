using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class SubjectInScheduleMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.SubjectInSchedule))
            {
                return MapFromDomain((SubjectInSchedule)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(SubjectInSchedule))
            {
                return MapFromDal((DTO.SubjectInSchedule)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.SubjectInSchedule MapFromDomain(SubjectInSchedule subjectInSchedule)
        {
            var res = subjectInSchedule == null ? null : new DTO.SubjectInSchedule
            {
                Id = subjectInSchedule.Id,
                CreatedAt = subjectInSchedule.CreatedAt,
                ChangedAt = subjectInSchedule.ChangedAt,
                CreatedBy = subjectInSchedule.CreatedBy,
                ChangedBy = subjectInSchedule.ChangedBy,
                SubjectId = subjectInSchedule.SubjectId,
                Subject = SubjectMapper.MapFromDomain(subjectInSchedule.Subject),
                ScheduleId = subjectInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromDomain(subjectInSchedule.Schedule),
                UniqueIdentifier = subjectInSchedule.UniqueIdentifier,
                SubjectType = subjectInSchedule.SubjectType,
                Rooms = subjectInSchedule.Rooms,
                Groups = subjectInSchedule.Groups,
                StartDateTime = subjectInSchedule.StartDateTime,
                EndDateTime = subjectInSchedule.EndDateTime
            };

            return res ?? default!;
        }

        public static SubjectInSchedule MapFromDal(DTO.SubjectInSchedule subjectInSchedule)
        {
            var res = subjectInSchedule == null ? null : new SubjectInSchedule
            {
                Id = subjectInSchedule.Id,
                CreatedAt = subjectInSchedule.CreatedAt,
                ChangedAt = subjectInSchedule.ChangedAt,
                CreatedBy = subjectInSchedule.CreatedBy,
                ChangedBy = subjectInSchedule.ChangedBy,
                SubjectId = subjectInSchedule.SubjectId,
                Subject = SubjectMapper.MapFromDal(subjectInSchedule.Subject),
                ScheduleId = subjectInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromDal(subjectInSchedule.Schedule),
                UniqueIdentifier = subjectInSchedule.UniqueIdentifier,
                SubjectType = subjectInSchedule.SubjectType,
                Rooms = subjectInSchedule.Rooms,
                Groups = subjectInSchedule.Groups,
                StartDateTime = subjectInSchedule.StartDateTime,
                EndDateTime = subjectInSchedule.EndDateTime
            };

            return res ?? default!;
        }
    }
}
