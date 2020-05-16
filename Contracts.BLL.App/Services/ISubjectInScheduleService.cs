using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLL.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ISubjectInScheduleService : IEntityServiceBase<SubjectInSchedule>, ISubjectInScheduleRepository<SubjectInSchedule>
    {
        /// <summary>
        /// Returns all subject connected to the schedule.
        /// This subjects are shown on client side timetable view and in admin settings.
        /// </summary>
        /// <param name="scheduleId">Schedule ID</param>
        /// <returns></returns>
        Task<IEnumerable<SubjectForTimetableAndSettings>> GetAllSubjectsForTimetableOrSettingsByScheduleIdAsync(int scheduleId);

        /// <summary>
        /// Returns all subjects connected to the schedule except finished ones. It is used on client side for timetable.
        /// </summary>
        /// <param name="scheduleId">Schedule ID</param>
        /// <param name="now">Current DateTime</param>
        /// <returns></returns>
        Task<IEnumerable<SubjectForTimetableAndSettings>> GetAllSubjectsForTimetableByScheduleIdWithoutFinishedAsync(int scheduleId, DateTime now);

    }
}
