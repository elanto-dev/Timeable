using System;
using Contracts.DAL.Base.Mappers;
using Domain;

namespace DAL.App.Mappers
{
    public class TeacherMapper : IBaseDalMapper
    {
        public TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(DTO.Teacher))
            {
                return MapFromDomain((Teacher)inObject) as TOutObject ?? default!;
            }

            if (typeof(TOutObject) == typeof(Teacher))
            {
                return MapFromDal((DTO.Teacher)inObject) as TOutObject ?? default!;
            }

            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static DTO.Teacher MapFromDomain(Teacher teacher)
        {
            var res = teacher == null ? null : new DTO.Teacher
            {
                Id = teacher.Id,
                CreatedAt = teacher.CreatedAt,
                ChangedAt = teacher.ChangedAt,
                CreatedBy = teacher.CreatedBy,
                ChangedBy = teacher.ChangedBy,
                FullName = teacher.FullName,
                Role = teacher.Role
            };

            return res ?? default!;
        }

        public static Teacher MapFromDal(DTO.Teacher teacher)
        {
            var res = teacher == null ? null : new Teacher
            {
                Id = teacher.Id,
                CreatedAt = teacher.CreatedAt,
                ChangedAt = teacher.ChangedAt,
                CreatedBy = teacher.CreatedBy,
                ChangedBy = teacher.ChangedBy,
                FullName = teacher.FullName,
                Role = teacher.Role
            };

            return res ?? default!;
        }
    }
}
