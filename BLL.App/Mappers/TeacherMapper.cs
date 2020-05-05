using System;
using Contracts.BLL.Base.Mappers;
using BllDto = BLL.DTO;
using DalDto = DAL.DTO;

namespace BLL.App.Mappers
{
    public class TeacherMapper : IBLLMapperBase
    {
        public TOutObject Map<TOutObject>(object inObject) where TOutObject : class
        {
            if (typeof(TOutObject) == typeof(BllDto.Teacher))
            {
                return MapFromInternal((DalDto.Teacher)inObject) as TOutObject;
            }

            if (typeof(TOutObject) == typeof(DalDto.Teacher))
            {
                return MapFromExternal((BllDto.Teacher)inObject) as TOutObject;
            }
            throw new InvalidCastException($"No conversion from {inObject.GetType().FullName} to {typeof(TOutObject).FullName}");
        }

        public static BllDto.Teacher MapFromInternal(DalDto.Teacher teacher)
        {
            var res = teacher == null ? null : new BllDto.Teacher
            {
                Id = teacher.Id,
                CreatedAt = teacher.CreatedAt,
                ChangedAt = teacher.ChangedAt,
                CreatedBy = teacher.CreatedBy,
                ChangedBy = teacher.ChangedBy,
                FullName = teacher.FullName,
                Role = teacher.Role
            };

            return res;
        }

        public static DalDto.Teacher MapFromExternal(BllDto.Teacher teacher)
        {
            var res = teacher == null ? null : new DalDto.Teacher
            {
                Id = teacher.Id,
                CreatedAt = teacher.CreatedAt,
                ChangedAt = teacher.ChangedAt,
                CreatedBy = teacher.CreatedBy,
                ChangedBy = teacher.ChangedBy,
                FullName = teacher.FullName,
                Role = teacher.Role
            };
            return res;
        }
    }
}
