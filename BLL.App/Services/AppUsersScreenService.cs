using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class AppUsersScreenService : BaseEntityService<AppUsersScreen, DAL.DTO.AppUsersScreen, IAppUnitOfWork>, IAppUsersScreenService
    {
        public AppUsersScreenService(IAppUnitOfWork uow) : base(uow, new AppUsersScreenMapper())
        {
            ServiceRepository = Uow.AppUsersScreens;
        }

        public async Task<AppUsersScreen> GetScreenForUserAsync(string userId)
        {
            return AppUsersScreenMapper.MapFromInternal(await Uow.AppUsersScreens.GetScreenForUserAsync(userId));
        }
    }
}
