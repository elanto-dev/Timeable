using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
         IAppUsersScreenRepository AppUsersScreens { get; }
         IEventInScheduleRepository EventInSchedules { get; }
         IEventRepository Events { get; }
         IPictureInScreenRepository PictureInScreens { get; }
         IPictureRepository Pictures { get; }
         IScheduleInScreenRepository ScheduleInScreens { get; }
         IScheduleRepository Schedules { get; }
         IScreenRepository Screens { get; }
         ISubjectInScheduleRepository SubjectInSchedules { get; }
         ISubjectRepository Subjects { get; }
         ITeacherInSubjectEventRepository TeacherInSubjectEvents { get; }
         ITeacherRepository Teachers { get; }
    }
}
