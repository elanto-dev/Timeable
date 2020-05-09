using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class TeacherService : BaseEntityService<Teacher, DAL.DTO.Teacher, IAppUnitOfWork>, ITeacherService
    {
        public TeacherService(IAppUnitOfWork uow) : base(uow, new TeacherMapper())
        {
            ServiceRepository = Uow.Teachers;
        }

        public async Task<Teacher> FindTeacherByNameAndRoleAsync(string name, string? role)
        {
            return TeacherMapper.MapFromInternal(await Uow.Teachers.FindTeacherByNameAndRoleAsync(name, role));
        }
    }
}
