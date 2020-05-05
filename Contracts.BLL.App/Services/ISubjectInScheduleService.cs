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
        Task<IEnumerable<SubjectForTimetableAndSettings>> GetAllSubjectsForTimetableOrSettingsByScheduleIdAsync(int scheduleId);
        Task<IEnumerable<SubjectForTimetableAndSettings>> GetAllSubjectsForTimetableByScheduleIdWithoutFinishedAsync(int scheduleId, DateTime now);

    }
}
