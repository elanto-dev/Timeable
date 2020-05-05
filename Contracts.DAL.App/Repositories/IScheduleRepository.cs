using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IScheduleRepository : IScheduleRepository<DalAppDTO.Schedule>
    {
    }
    
    public interface IScheduleRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        Task<bool> ScheduleForDayExistsAsync(DateTime date);
    }
}
