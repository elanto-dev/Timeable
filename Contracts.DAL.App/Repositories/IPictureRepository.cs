using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IPictureRepository : IPictureRepository<DalAppDTO.Picture>
    {
    }
    
    public interface IPictureRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
    }
}
