using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IPictureInScreenRepository : IPictureInScreenRepository<DalAppDTO.PictureInScreen>
    {
    }
    public interface IPictureInScreenRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        /// <summary>
        /// Returns all PictureInScreen entities that are marked as promotions and are connected to the screen.
        /// </summary>
        /// <param name="screenId">Screen ID</param>
        /// <returns>PictureInScreen entities</returns>
        Task<IEnumerable<TDalEntity>> GetAllPromotionsForScreenAsync(int screenId);

        /// <summary>
        /// Returns the background PictureInScreen entity that is connected to the screen.
        /// </summary>
        /// <param name="screenId">Screen ID</param>
        /// <returns>PictureInScreen entity</returns>
        Task<TDalEntity> GetBackgroundPictureForScreen(int screenId);
    }
}
