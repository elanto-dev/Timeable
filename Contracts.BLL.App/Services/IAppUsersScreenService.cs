using BLL.DTO;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;

namespace Contracts.BLL.App.Services
{
    public interface IAppUsersScreenService : IEntityServiceBase<AppUsersScreen>, IAppUsersScreenRepository<AppUsersScreen>
    {
    }
}
