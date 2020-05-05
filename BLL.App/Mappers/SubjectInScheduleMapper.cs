using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class SubjectInScheduleMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.SubjectInSchedule))
            {
                return MapFromInternal((DalDto.SubjectInSchedule)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(DalDto.SubjectInSchedule))
            {
                return MapFromExternal((BllDto.SubjectInSchedule)inObject) as TOutObject ?? default!;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.SubjectInSchedule MapFromInternal(DalDto.SubjectInSchedule subjectInSchedule)
        {
            var res = subjectInSchedule == null ? null : new BllDto.SubjectInSchedule
            {
                Id = subjectInSchedule.Id,
                CreatedAt = subjectInSchedule.CreatedAt,
                ChangedAt = subjectInSchedule.ChangedAt,
                CreatedBy = subjectInSchedule.CreatedBy,
                ChangedBy = subjectInSchedule.ChangedBy,
                SubjectId = subjectInSchedule.SubjectId,
                Subject = SubjectMapper.MapFromInternal(subjectInSchedule.Subject),
                ScheduleId = subjectInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromInternal(subjectInSchedule.Schedule),
                UniqueIdentifier = subjectInSchedule.UniqueIdentifier,
                SubjectType = subjectInSchedule.SubjectType,
                Rooms = subjectInSchedule.Rooms,
                Groups = subjectInSchedule.Groups,
                StartDateTime = subjectInSchedule.StartDateTime,
                EndDateTime = subjectInSchedule.EndDateTime
            };

            return res ?? default!;
        }

        public static DalDto.SubjectInSchedule MapFromExternal(BllDto.SubjectInSchedule subjectInSchedule)
        {
            var res = subjectInSchedule == null ? null : new DalDto.SubjectInSchedule
            {
                Id = subjectInSchedule.Id,
                CreatedAt = subjectInSchedule.CreatedAt,
                ChangedAt = subjectInSchedule.ChangedAt,
                CreatedBy = subjectInSchedule.CreatedBy,
                ChangedBy = subjectInSchedule.ChangedBy,
                SubjectId = subjectInSchedule.SubjectId,
                Subject = SubjectMapper.MapFromExternal(subjectInSchedule.Subject ?? default!),
                ScheduleId = subjectInSchedule.ScheduleId,
                Schedule = ScheduleMapper.MapFromExternal(subjectInSchedule.Schedule),
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
