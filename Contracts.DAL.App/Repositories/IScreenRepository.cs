using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IScreenRepository : IScreenRepository<DalAppDTO.Screen>
    {
    }

    public interface IScreenRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        Task<TDalEntity> GetFirstAndActiveScreenAsync();
    }
}
