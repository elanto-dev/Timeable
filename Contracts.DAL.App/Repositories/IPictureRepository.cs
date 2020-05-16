using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// <summary>
        /// Find pictures with the path specified.
        /// </summary>
        /// <param name="path">Path string.</param>
        /// <returns>Picture entities</returns>
        Task<IEnumerable<TDalEntity>> FindPicturesByPathAsync(string path);
    }
}
