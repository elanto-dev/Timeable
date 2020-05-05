using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DalAppDTO = DAL.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITeacherInSubjectEventRepository : ITeacherInSubjectEventRepository<DalAppDTO.TeacherInSubjectEvent>
    {
    }
    
    public interface ITeacherInSubjectEventRepository<TDalEntity> : IBaseRepository<TDalEntity>
        where TDalEntity : class, new()
    {
        Task<IEnumerable<TDalEntity>> GetAllTeachersForSubjectEventWithoutSubjInclude(int subjectEventId);
    }
}
