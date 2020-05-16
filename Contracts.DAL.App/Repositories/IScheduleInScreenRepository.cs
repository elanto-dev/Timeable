using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IScheduleInScreenRepository : IScheduleInScreenRepository<DalAppDTO.ScheduleInScreen>
    {
    }
    
    public interface IScheduleInScreenRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        /// <summary>
        /// Find ScheduleInScreen by date, screen ID and building prefix. 
        /// </summary>
        /// <param name="screenId">Screen ID</param>
        /// <param name="screenPrefix">Building (location) prefix</param>
        /// <param name="date">Date to search for</param>
        /// <returns>ScheduleInScreen entity</returns>
        Task<TDalEntity> FindForScreenForDateWithoutIncludesAsync(int screenId, string screenPrefix, DateTime date);

        /// <summary>
        /// Find ScheduleInScreen entity connected to the schedule. 
        /// </summary>
        /// <param name="scheduleId">Schedule ID</param>
        /// <returns>PScheduleInScreen entity</returns>
        Task<TDalEntity> FindByScheduleIdAsync(int scheduleId);
    }
}
