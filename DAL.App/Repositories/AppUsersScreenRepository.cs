using System;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class AppUsersScreenRepository : BaseRepository<AppUsersScreen, Domain.AppUsersScreen, AppDbContext>, IAppUsersScreenRepository
    {
        public AppUsersScreenRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new AppUsersScreenMapper())
        {
        }

        public async Task<AppUsersScreen?> GetScreenForUserAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var userGuid))
            {
                return null;
            }

            return AppUsersScreenMapper.MapFromDomain(await RepositoryDbSet
                .AsNoTracking()
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(s => s.AppUserId.Equals(userGuid)));
        }
    }
}
