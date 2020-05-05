using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLL.DTO;

namespace Contracts.BLL.App.Services
{
    public interface IPictureService : IEntityServiceBase<Picture>, IPictureRepository<Picture>
    {
    }
}
