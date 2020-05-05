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
        Task<TDalEntity> FindForScreenForDateWithoutIncludesAsync(int screenId, string screenPrefix, DateTime date);
    }
}
