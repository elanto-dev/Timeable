using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class ScheduleInScreenRepository : BaseRepository<ScheduleInScreen, Domain.ScheduleInScreen, AppDbContext>, IScheduleInScreenRepository
    {
        public ScheduleInScreenRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ScheduleInScreenMapper())
        {
        }

        public async Task<ScheduleInScreen> FindForScreenForDateWithoutIncludesAsync(int screenId, string prefix, DateTime date)
        {
            var schedulesInScreen = await RepositoryDbSet
                .Include(s => s.Schedule)
                .AsNoTracking()
                .Where(s => s.ScreenId == screenId && s.Schedule.Prefix == prefix && s.Schedule.Date == date)
                .Select(s => ScheduleInScreenMapper.MapFromDomain(s)).ToListAsync();
            return schedulesInScreen.LastOrDefault();
        }
    }
}
