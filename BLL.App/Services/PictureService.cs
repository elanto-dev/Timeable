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
    public class PictureService : BaseEntityService<DTO.Picture, DAL.DTO.Picture, IAppUnitOfWork>, IPictureService
    {
        public PictureService(IAppUnitOfWork uow) : base(uow, new PictureMapper())
        {
            ServiceRepository = Uow.Pictures;
        }

        public async Task<IEnumerable<Picture>> FindPicturesByPathAsync(string path)
        {
            return (await Uow.Pictures.FindPicturesByPathAsync(path)).Select(PictureMapper.MapFromInternal).ToList();
        }
    }
}
