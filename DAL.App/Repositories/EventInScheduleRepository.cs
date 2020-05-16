using System;
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
    public class EventInScheduleRepository : BaseRepository<EventInSchedule, Domain.EventInSchedule, AppDbContext>, IEventInScheduleRepository
    {
        public EventInScheduleRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new EventInScheduleMapper())
        {
        }

        public async Task<IEnumerable<EventInSchedule>> GetAllEventsForScheduleAsync(int scheduleId)
        {
            return await RepositoryDbSet
                .Include(e => e.Schedule)
                .Include(e => e.Event)
                .AsNoTracking()
                .Where(e => e.Event.ShowStartDateTime <= DateTime.Now && e.Event.ShowEndDateTime > DateTime.Now && e.ScheduleId == scheduleId)
                .OrderBy(e => e.Event.StartTime).ThenBy(e => e.Event.EndDateTime)
                .Select(e => EventInScheduleMapper.MapFromDomain(e))
                .ToListAsync();
        }
    }
}
