using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;

namespace DAL.App.Repositories
{
    public class ScheduleRepository : BaseRepository<Schedule, Domain.Schedule, AppDbContext>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ScheduleMapper())
        {
        }
    }
}
