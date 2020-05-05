using Contracts.DAL.App.Repositories;
using BLL.DTO;
using Contracts.BLL.Base.Services;

namespace Contracts.BLL.App.Services
{
    public interface ISubjectService : IEntityServiceBase<Subject>, ISubjectRepository<Subject>
    {
    }
}
