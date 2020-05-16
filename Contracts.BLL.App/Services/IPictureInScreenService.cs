using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLL.DTO;

namespace Contracts.BLL.App.Services
{
    public interface IPictureInScreenService : IEntityServiceBase<PictureInScreen>, IPictureInScreenRepository<PictureInScreen>
    {
        /// <summary>
        /// Returns all promotions that are with the screen.
        /// </summary>
        /// <param name="screenId">Screen ID</param>
        /// <returns></returns>
        Task<IEnumerable<PromotionsForTimetable>> GetAllPromotionsForTimetableAsync(int screenId);
    }
}
