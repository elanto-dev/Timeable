using System.Threading.Tasks;

namespace Contracts.BLL.Base
{
    public interface IBLLBase
    {
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
