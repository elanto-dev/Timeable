using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLL.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ITeacherService : IEntityServiceBase<Teacher>, ITeacherRepository<Teacher>
    {
    }
}
