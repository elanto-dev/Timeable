using System.Threading.Tasks;
using Contracts.DAL.App.Repositories;
using DAL.App.Mappers;
using DAL.Base.Repositories;
using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.Repositories
{
    public class PictureRepository : BaseRepository<Picture, Domain.Picture, AppDbContext>, IPictureRepository
    {
        public PictureRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new PictureMapper())
        {
        }

        public override async Task<Picture> FindAsync(params object[] id)
        {
            if (!(id[0] is int))
            {
                return null;
            }
            return PictureMapper.MapFromDomain(await RepositoryDbSet.AsNoTracking().FirstOrDefaultAsync(pic => pic.Id == (int)id[0]));
        }
    }
}
