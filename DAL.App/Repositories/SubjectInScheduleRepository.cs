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
    public class SubjectInScheduleRepository : BaseRepository<SubjectInSchedule, Domain.SubjectInSchedule, AppDbContext>, ISubjectInScheduleRepository
    {
        public SubjectInScheduleRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new SubjectInScheduleMapper())
        {
        }

        public async Task<IEnumerable<SubjectInSchedule>> GetAllSubjectsForScheduleAsync(int scheduleId)
        {
            return await RepositoryDbSet
                .Include(s => s.Schedule)
                .Include(s => s.Subject)
                .Where(s => s.ScheduleId == scheduleId)
                .OrderBy(s => s.StartDateTime)
                .Select(s => SubjectInScheduleMapper.MapFromDomain(s))
                .ToListAsync();
        }

        public async Task<IEnumerable<SubjectInSchedule>> GetAllSubjectsForScheduleWithoutFinishedAsync(int scheduleId, DateTime now)
        {
            return await RepositoryDbSet
                .Include(s => s.Schedule)
                .Include(s => s.Subject)
                .Where(s => s.ScheduleId == scheduleId && s.EndDateTime > now)
                .OrderBy(s => s.StartDateTime)
                .Select(s => SubjectInScheduleMapper.MapFromDomain(s))
                .ToListAsync();
        }

        public async Task<SubjectInSchedule> FindByUniqueIdentifierAsync(string uniqueId)
        {
            return SubjectInScheduleMapper.MapFromDomain(await RepositoryDbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UniqueIdentifier.Equals(uniqueId)));
        }

        public async Task<bool> SubjectsInScheduleExistForScheduleAsync(int scheduleId)
        {
            return await RepositoryDbSet.AnyAsync(s => s.ScheduleId == scheduleId);
        }
    }
}
