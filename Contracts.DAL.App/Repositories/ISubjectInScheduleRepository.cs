using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ISubjectInScheduleRepository : ISubjectInScheduleRepository<DalAppDTO.SubjectInSchedule>
    {
    }
    
    public interface ISubjectInScheduleRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        /// <summary>
        /// Returns all SubjectInSchedule that are connected to the schedule.
        /// </summary>
        /// <param name="scheduleId">Schedule ID</param>
        /// <returns>SubjectInSchedule entities</returns>
        Task<IEnumerable<TDalEntity>> GetAllSubjectsForScheduleAsync(int scheduleId);

        /// <summary>
        /// Returns all SubjectInSchedule entities which was not still finished and which is connected to the schedule.
        /// </summary>
        /// <param name="scheduleId">Schedule ID</param>
        /// <param name="now">Current time</param>
        /// <returns>SubjectInSchedule entities</returns>
        Task<IEnumerable<TDalEntity>> GetAllSubjectsForScheduleWithoutFinishedAsync(int scheduleId, DateTime now);

        /// <summary>
        /// Finds SubjectInSchedule entity by UniqueIdentifier. 
        /// </summary>
        /// <param name="uniqueId">UniqueIdentifier property</param>
        /// <returns>SubjectInSchedule entity</returns>
        Task<TDalEntity> FindByUniqueIdentifierAsync(string uniqueId);

        /// <summary>
        /// Checks whether if there is any SubjectInSchedule in the schedule.
        /// </summary>
        /// <param name="scheduleId">Schedule ID</param>
        /// <returns>Boolean</returns>
        Task<bool> SubjectsInScheduleExistForScheduleAsync(int scheduleId);
    }
}
