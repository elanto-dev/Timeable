using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class TeacherInSubjectEventMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.TeacherInSubjectEvent))
            {
                return MapFromInternal((DalDto.TeacherInSubjectEvent)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.TeacherInSubjectEvent))
            {
                return MapFromExternal((BllDto.TeacherInSubjectEvent)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.TeacherInSubjectEvent MapFromInternal(DalDto.TeacherInSubjectEvent teacherInSubjectEvent)
        {
            var res = teacherInSubjectEvent == null ? null : new BllDto.TeacherInSubjectEvent
            {
                Id = teacherInSubjectEvent.Id,
                CreatedAt = teacherInSubjectEvent.CreatedAt,
                ChangedAt = teacherInSubjectEvent.ChangedAt,
                CreatedBy = teacherInSubjectEvent.CreatedBy,
                ChangedBy = teacherInSubjectEvent.ChangedBy,
                TeacherId = teacherInSubjectEvent.TeacherId,
                Teacher = TeacherMapper.MapFromInternal(teacherInSubjectEvent.Teacher),
                SubjectInScheduleId = teacherInSubjectEvent.SubjectInScheduleId,
                SubjectInSchedule = SubjectInScheduleMapper.MapFromInternal(teacherInSubjectEvent.SubjectInSchedule)
            };

            return res;
        }

        public static DalDto.TeacherInSubjectEvent MapFromExternal(BllDto.TeacherInSubjectEvent teacherInSubjectEvent)
        {
            var res = teacherInSubjectEvent == null ? null : new DalDto.TeacherInSubjectEvent
            {
                Id = teacherInSubjectEvent.Id,
                CreatedAt = teacherInSubjectEvent.CreatedAt,
                ChangedAt = teacherInSubjectEvent.ChangedAt,
                CreatedBy = teacherInSubjectEvent.CreatedBy,
                ChangedBy = teacherInSubjectEvent.ChangedBy,
                TeacherId = teacherInSubjectEvent.TeacherId,
                Teacher = TeacherMapper.MapFromExternal(teacherInSubjectEvent.Teacher),
                SubjectInScheduleId = teacherInSubjectEvent.SubjectInScheduleId,
                SubjectInSchedule = SubjectInScheduleMapper.MapFromExternal(teacherInSubjectEvent.SubjectInSchedule)
            };
            return res;
        }
    }
}
