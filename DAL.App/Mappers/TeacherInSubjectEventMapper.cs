using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class TeacherInSubjectEventMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.TeacherInSubjectEvent))
            {
                return MapFromDomain((TeacherInSubjectEvent)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(TeacherInSubjectEvent))
            {
                return MapFromDal((DTO.TeacherInSubjectEvent)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.TeacherInSubjectEvent MapFromDomain(TeacherInSubjectEvent teacherInSubjectEvent)
        {
            var res = teacherInSubjectEvent == null ? null : new DTO.TeacherInSubjectEvent
            {
                Id = teacherInSubjectEvent.Id,
                CreatedAt = teacherInSubjectEvent.CreatedAt,
                ChangedAt = teacherInSubjectEvent.ChangedAt,
                CreatedBy = teacherInSubjectEvent.CreatedBy,
                ChangedBy = teacherInSubjectEvent.ChangedBy,
                TeacherId = teacherInSubjectEvent.TeacherId,
                Teacher = TeacherMapper.MapFromDomain(teacherInSubjectEvent.Teacher),
                SubjectInScheduleId = teacherInSubjectEvent.SubjectInScheduleId,
                SubjectInSchedule = SubjectInScheduleMapper.MapFromDomain(teacherInSubjectEvent.SubjectInSchedule)
            };

            return res ?? default!;
        }

        public static TeacherInSubjectEvent MapFromDal(DTO.TeacherInSubjectEvent teacherInSubjectEvent)
        {
            var res = teacherInSubjectEvent == null ? null : new TeacherInSubjectEvent
            {
                Id = teacherInSubjectEvent.Id,
                CreatedAt = teacherInSubjectEvent.CreatedAt,
                ChangedAt = teacherInSubjectEvent.ChangedAt,
                CreatedBy = teacherInSubjectEvent.CreatedBy,
                ChangedBy = teacherInSubjectEvent.ChangedBy,
                TeacherId = teacherInSubjectEvent.TeacherId,
                Teacher = TeacherMapper.MapFromDal(teacherInSubjectEvent.Teacher),
                SubjectInScheduleId = teacherInSubjectEvent.SubjectInScheduleId,
                SubjectInSchedule = SubjectInScheduleMapper.MapFromDal(teacherInSubjectEvent.SubjectInSchedule)
            };

            return res ?? default!;
        }
    }
}
