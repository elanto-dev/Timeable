using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IAppUsersScreenRepository : IAppUsersScreenRepository<DalAppDTO.AppUsersScreen>
    {
    }

    public interface IAppUsersScreenRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        Task<TDalEntity> GetScreenForUserAsync(string userId);
    }
}
