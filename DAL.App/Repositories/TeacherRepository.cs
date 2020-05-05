using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class TeacherRepository : BaseRepository<Teacher, Domain.Teacher, AppDbContext>, ITeacherRepository
    {
        public TeacherRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new TeacherMapper())
        {
        }

        public async Task<Teacher> FindTeacherByNameAndRoleAsync(string name, string? role)
        {
            return TeacherMapper.MapFromDomain(await RepositoryDbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.FullName.Equals(name) && t.Role == role));
        }
    }
}
