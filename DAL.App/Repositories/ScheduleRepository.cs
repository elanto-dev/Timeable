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
    public class ScheduleRepository : BaseRepository<Schedule, Domain.Schedule, AppDbContext>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ScheduleMapper())
        {
        }

        public async Task<bool> ScheduleForDayExistsAsync(DateTime date)
        {
            return await RepositoryDbSet.AnyAsync(e => e.Date.Equals(date));
        }
    }
}
