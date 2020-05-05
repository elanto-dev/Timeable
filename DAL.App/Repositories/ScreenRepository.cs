using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class ScreenRepository : BaseRepository<Screen, Domain.Screen, AppDbContext>, IScreenRepository
    {
        public ScreenRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ScreenMapper())
        {
            repositoryDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public override async Task<Screen> FindAsync(params object[] id)
        {
            if (!(id[0] is int))
            {
                return null;
            }
            return ScreenMapper.MapFromDomain(await RepositoryDbSet.AsNoTracking().FirstOrDefaultAsync(screen => screen.Id == (int)id[0]));
        }

        public async Task<Screen> GetFirstAndActiveScreenAsync()
        {
            return ScreenMapper.MapFromDomain(await RepositoryDbSet.AsNoTracking().FirstOrDefaultAsync(s => s.IsActive));
        }
    }
}
