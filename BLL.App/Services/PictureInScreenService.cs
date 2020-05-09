using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class PictureInScreenService : BaseEntityService<PictureInScreen, DAL.DTO.PictureInScreen, IAppUnitOfWork>, IPictureInScreenService
    {
        public PictureInScreenService(IAppUnitOfWork uow) : base(uow, new PictureInScreenMapper())
        {
            ServiceRepository = Uow.PictureInScreens;
        }

        public async Task<IEnumerable<PictureInScreen>> GetAllPromotionsForScreen(int screenId)
        {
            return (await Uow.PictureInScreens.GetAllPromotionsForScreen(screenId))
                .Select(PictureInScreenMapper.MapFromInternal)
                .ToList();
        }

        public async Task<PictureInScreen> GetBackgroundPictureForScreen(int screenId)
        {
            return PictureInScreenMapper.MapFromInternal(await Uow.PictureInScreens.GetBackgroundPictureForScreen(screenId));
        }
    }
}
