using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class PictureInScreenRepository : BaseRepository<PictureInScreen, Domain.PictureInScreen, AppDbContext>, IPictureInScreenRepository
    {
        public PictureInScreenRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new PictureInScreenMapper())
        {
            repositoryDbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // Screen include removed due to tracking problem
        public async Task<IEnumerable<PictureInScreen>> GetAllPromotionsForScreen(int screenId)
        {
            return await RepositoryDbSet
                .Where(p => p.ScreenId == screenId && p.IsBackgroundPicture == false)
                .AsNoTracking()
                .Include(p => p.Picture)
                .AsNoTracking()
                .Select(p => PictureInScreenMapper.MapFromDomain(p))
                .ToListAsync();
        }

        public async Task<PictureInScreen> GetBackgroundPictureForScreen(int screenId)
        {
            return PictureInScreenMapper.MapFromDomain(await RepositoryDbSet
                .Include(p => p.Picture)
                .Include(p => p.Screen)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ScreenId == screenId && p.IsBackgroundPicture));
        }

        // Screen include removed due to tracking problem
        public override async Task<PictureInScreen> FindAsync(params object[] id)
        {
            if (!(id[0] is int))
            {
                return null;
            }
            return PictureInScreenMapper.MapFromDomain(
                await RepositoryDbSet
                    .Include(p => p.Picture)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(screen => screen.Id == (int)id[0]));
        }
    }
}
