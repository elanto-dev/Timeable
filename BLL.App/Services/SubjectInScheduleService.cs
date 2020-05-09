using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using SubjectInSchedule = BLL.DTO.SubjectInSchedule;

namespace BLL.App.Services
{
    public class SubjectInScheduleService : BaseEntityService<SubjectInSchedule, DAL.DTO.SubjectInSchedule, IAppUnitOfWork>, ISubjectInScheduleService
    {
        public SubjectInScheduleService(IAppUnitOfWork uow) : base(uow, new SubjectInScheduleMapper())
        {
            ServiceRepository = Uow.SubjectInSchedules;
        }

        public async Task<IEnumerable<SubjectInSchedule>> GetAllSubjectsForScheduleAsync(int scheduleId)
        {
            return (await Uow.SubjectInSchedules.GetAllSubjectsForScheduleAsync(scheduleId)).Select(SubjectInScheduleMapper.MapFromInternal).ToList();
        }

        public async Task<IEnumerable<SubjectInSchedule>> GetAllSubjectsForScheduleWithoutFinishedAsync(int scheduleId, DateTime now)
        {
            return (await Uow.SubjectInSchedules.GetAllSubjectsForScheduleWithoutFinishedAsync(scheduleId, now)).Select(SubjectInScheduleMapper.MapFromInternal).ToList();
        }

        public async Task<SubjectInSchedule> FindByUniqueIdentifierAsync(string uniqueId)
        {
            return SubjectInScheduleMapper.MapFromInternal(await Uow.SubjectInSchedules.FindByUniqueIdentifierAsync(uniqueId));
        }

        public async Task<bool> SubjectsInScheduleExistForScheduleAsync(int scheduleId)
        {
            return (await Uow.SubjectInSchedules.SubjectsInScheduleExistForScheduleAsync(scheduleId));
        }

        public async Task<IEnumerable<SubjectForTimetableAndSettings>> GetAllSubjectsForTimetableOrSettingsByScheduleIdAsync(int scheduleId)
        {
            return (await Uow.SubjectInSchedules.GetAllSubjectsForScheduleAsync(scheduleId)).Select(SubjectInScheduleToSTimetableMapper.MapFromInternal).ToList();
        }

        public async Task<IEnumerable<SubjectForTimetableAndSettings>> GetAllSubjectsForTimetableByScheduleIdWithoutFinishedAsync(int scheduleId, DateTime now)
        {
            return (await Uow.SubjectInSchedules.GetAllSubjectsForScheduleWithoutFinishedAsync(scheduleId, now)).Select(SubjectInScheduleToSTimetableMapper.MapFromInternal).ToList();
        }
    }
}
