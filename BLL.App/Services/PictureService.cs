using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class PictureService : BaseEntityService<DTO.Picture, DAL.DTO.Picture, IAppUnitOfWork>, IPictureService
    {
        public PictureService(IAppUnitOfWork uow) : base(uow, new PictureMapper())
        {
            ServiceRepository = Uow.Pictures;
        }
    }
}
