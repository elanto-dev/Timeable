using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITeacherRepository : ITeacherRepository<DalAppDTO.Teacher>
    {
    }

    public interface ITeacherRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        Task<TDalEntity> FindTeacherByNameAndRoleAsync(string name, string? role);
    }
}
