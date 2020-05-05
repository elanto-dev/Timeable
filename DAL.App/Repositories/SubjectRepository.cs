using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class SubjectRepository : BaseRepository<Subject, Domain.Subject, AppDbContext>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new SubjectMapper())
        {
        }

        public async Task<Subject> FindBySubjectNameAndCodeAsync(string name, string code)
        {
            return SubjectMapper.MapFromDomain(await RepositoryDbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SubjectName == name && s.SubjectCode == code));
        }
    }
}
