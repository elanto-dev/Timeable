using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ISubjectRepository : ISubjectRepository<DalAppDTO.Subject>
    {
    }
    
    public interface ISubjectRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        Task<TDalEntity> FindBySubjectNameAndCodeAsync(string name, string code);
    }
}
