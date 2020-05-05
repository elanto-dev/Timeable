using System.Threading.Tasks;
using Contracts.BLL.Base;
using Contracts.BLL.Base.Helpers;
using Contracts.DAL.Base;

namespace Bll.Base
{
    public class BLLBase<TUnitOfWork> : IBLLBase
        where TUnitOfWork : IBaseUnitOfWork
    {

        protected readonly TUnitOfWork UnitOfWork;
        protected readonly IServiceProviderBase ServiceProvider;

        public BLLBase(TUnitOfWork unitOfWork, IServiceProviderBase serviceProvider)
        {
            UnitOfWork = unitOfWork;
            ServiceProvider = serviceProvider;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await UnitOfWork.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return UnitOfWork.SaveChanges();
        }
    }
}
