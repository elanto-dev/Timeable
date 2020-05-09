using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class TeacherInSubjectEventService : BaseEntityService<TeacherInSubjectEvent, DAL.DTO.TeacherInSubjectEvent, IAppUnitOfWork>, ITeacherInSubjectEventService
    {
        public TeacherInSubjectEventService(IAppUnitOfWork uow) : base(uow, new TeacherInSubjectEventMapper())
        {
            ServiceRepository = Uow.TeacherInSubjectEvents;
        }

        public async Task<IEnumerable<TeacherInSubjectEvent>> GetAllTeachersForSubjectEventWithoutSubjInclude(int subjectEventId)
        {
            return (await Uow.TeacherInSubjectEvents.GetAllTeachersForSubjectEventWithoutSubjInclude(subjectEventId))
                .Select(TeacherInSubjectEventMapper.MapFromInternal).ToList();
        }
    }
}
