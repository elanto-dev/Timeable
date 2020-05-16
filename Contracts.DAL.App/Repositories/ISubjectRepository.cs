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
        /// <summary>
        /// Find subject ny its name and code
        /// </summary>
        /// <param name="name">Subject name</param>
        /// <param name="code">Subject code</param>
        /// <returns>Subject entity</returns>
        Task<TDalEntity> FindBySubjectNameAndCodeAsync(string name, string code);
    }
}
