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
        Task<IEnumerable<TDalEntity>> GetAllSubjectsForScheduleAsync(int scheduleId);
        Task<IEnumerable<TDalEntity>> GetAllSubjectsForScheduleWithoutFinishedAsync(int scheduleId, DateTime now);
        Task<TDalEntity> FindByUniqueIdentifierAsync(string uniqueId);
        Task<bool> SubjectsInScheduleExistForScheduleAsync(int scheduleId);
    }
}
