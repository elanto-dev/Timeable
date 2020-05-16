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
    public class EventRepository : BaseRepository<Event, Domain.Event, AppDbContext>, IEventRepository
    {
        public EventRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new EventMapper())
        {
        }


        public async Task<IEnumerable<Event>> GetAllFutureEventsAsync(DateTime currentTime)
        {
            return await RepositoryDbSet
                .AsNoTracking()
                .Where(e => e.EndDateTime.Date >= currentTime.Date || e.ShowEndDateTime.Date >= currentTime.Date)
                .OrderBy(e => e.StartTime).ThenBy(e => e.EndDateTime)
                .Select(e => EventMapper.MapFromDomain(e))
                .ToListAsync();
        }
    }
}
