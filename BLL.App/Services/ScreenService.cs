using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class ScreenService : BaseEntityService<Screen, DAL.DTO.Screen, IAppUnitOfWork>, IScreenService
    {
        public ScreenService(IAppUnitOfWork uow) : base(uow, new ScreenMapper())
        {
            ServiceRepository = Uow.Screens;
        }

        public async Task<Screen> GetFirstAndActiveScreenAsync()
        {
            return ScreenMapper.MapFromInternal(await Uow.Screens.GetFirstAndActiveScreenAsync());
        }
    }
}
