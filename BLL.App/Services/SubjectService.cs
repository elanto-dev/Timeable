using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class SubjectService : BaseEntityService<Subject, DAL.DTO.Subject, IAppUnitOfWork>, ISubjectService
    {
        public SubjectService(IAppUnitOfWork uow) : base(uow, new SubjectMapper())
        {
            ServiceRepository = Uow.Subjects;
        }

        public async Task<Subject> FindBySubjectNameAndCodeAsync(string name, string code)
        {
            return SubjectMapper.MapFromInternal(await Uow.Subjects.FindBySubjectNameAndCodeAsync(name, code));
        }
    }
}
