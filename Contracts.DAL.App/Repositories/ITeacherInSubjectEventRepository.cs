using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITeacherInSubjectEventRepository : ITeacherInSubjectEventRepository<DalAppDTO.TeacherInSubjectEvent>
    {
    }
    
    public interface ITeacherInSubjectEventRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        /// <summary>
        /// Returns all TeacherInSubject entities for the SubjectInSchedule entity.
        /// </summary>
        /// <param name="subjectEventId">SubjectInSchedule ID</param>
        /// <returns>TeacherInSubjectEvent entity</returns>
        Task<IEnumerable<TDalEntity>> GetAllTeachersForSubjectEventWithoutSubjInclude(int subjectEventId);

        /// <summary>
        /// Removes TeacherInSubjectEvent entity by SubjectInSchedule and Teacher IDs.
        /// </summary>
        /// <param name="subjectEventId">SubjectInSchedule ID</param>
        /// <param name="teacherId">Teacher ID</param>
        void RemoveBySubjectEventAndTeacherIds(int subjectEventId, int teacherId);
    }
}
