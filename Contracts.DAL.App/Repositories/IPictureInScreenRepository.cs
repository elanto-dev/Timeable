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
        Task<IEnumerable<TDalEntity>> GetAllPromotionsForScreenAsync(int screenId);
        Task<TDalEntity> GetBackgroundPictureForScreen(int screenId);
    }
}
