using Contracts.DAL.Base.Repositories;

namespace Contracts.BLL.Base.Services
{
    public interface IEntityServiceBase<TBllEntity> : IBaseRepository<TBllEntity>
        where TBllEntity : class, new()
    {
    }
}
