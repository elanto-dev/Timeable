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
        /// <summary>
        /// Returns screen that is connected with the user.
        /// </summary>
        /// <param name="userId">AppUser ID</param>
        /// <returns>Screen entity</returns>
        Task<TDalEntity?> GetScreenForUserAsync(string userId);
    }
}
