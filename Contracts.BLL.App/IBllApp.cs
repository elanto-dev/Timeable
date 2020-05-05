using Contracts.BLL.App.Services;
using Contracts.BLL.Base;

namespace Contracts.BLL.App
{
    public interface IBLLApp : IBLLBase
    {
        IAppUsersScreenService AppUsersScreens { get; }
        IEventInScheduleService EventInSchedules { get; }
        IEventService Events { get; }
        IPictureService Pictures { get; }
        IPictureInScreenService PictureInScreens { get; }
        IScheduleInScreenService ScheduleInScreens { get; }
        IScheduleService Schedules { get; }
        IScreenService Screens { get; }
        ISubjectInScheduleService SubjectInSchedules { get; }
        ISubjectService Subjects { get; }
        ITeacherInSubjectEventService TeacherInSubjectEvents { get; }
        ITeacherService Teachers { get; }
    }
}
