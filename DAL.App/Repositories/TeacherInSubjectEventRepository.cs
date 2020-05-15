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
    public class TeacherInSubjectEventRepository : BaseRepository<TeacherInSubjectEvent, Domain.TeacherInSubjectEvent, AppDbContext>, ITeacherInSubjectEventRepository
    {
        public TeacherInSubjectEventRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new TeacherInSubjectEventMapper())
        {
        }

        public async Task<IEnumerable<TeacherInSubjectEvent>> GetAllTeachersForSubjectEventWithoutSubjInclude(int subjectEventId)
        {
            return await RepositoryDbSet
                .Include(t => t.Teacher)
                .AsNoTracking()
                .Where(e => e.SubjectInScheduleId == subjectEventId)
                .AsNoTracking()
                .Select(t => TeacherInSubjectEventMapper.MapFromDomain(t))
                .ToListAsync();
        }

        public void RemoveBySubjectEventAndTeacherIds(int subjectEventId, int teacherId)
        {
            RepositoryDbSet.Remove(RepositoryDbSet.First(entity =>
                entity.TeacherId == teacherId && entity.SubjectInScheduleId == subjectEventId));
        }
    }
}
