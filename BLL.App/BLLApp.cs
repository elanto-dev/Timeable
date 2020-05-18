
using Bll.Base;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Helpers;
using Contracts.DAL.App;

namespace BLL.App
{
    public class BLLApp : BLLBase<IAppUnitOfWork>, IBLLApp
    {
        protected readonly IAppUnitOfWork AppUnitOfWork;

        public BLLApp(IAppUnitOfWork appUnitOfWork, IServiceProviderBase serviceProvider) : base(appUnitOfWork,
            serviceProvider)
        {
            AppUnitOfWork = appUnitOfWork;
        }

        public IAppUsersScreenService AppUsersScreens => ServiceProvider.GetService<IAppUsersScreenService>();
        public IEventInScheduleService EventInSchedules => ServiceProvider.GetService<IEventInScheduleService>();
        public IEventService Events => ServiceProvider.GetService<IEventService>();
        public IPictureService Pictures => ServiceProvider.GetService<IPictureService>();
        public IPictureInScreenService PictureInScreens => ServiceProvider.GetService<IPictureInScreenService>();
        public IScheduleInScreenService ScheduleInScreens => ServiceProvider.GetService<IScheduleInScreenService>();
        public IScheduleService Schedules => ServiceProvider.GetService<IScheduleService>();
        public IScreenService Screens => ServiceProvider.GetService<IScreenService>();
        public ISubjectInScheduleService SubjectInSchedules => ServiceProvider.GetService<ISubjectInScheduleService>();
        public ISubjectService Subjects => ServiceProvider.GetService<ISubjectService>();
        public ITeacherInSubjectEventService TeacherInSubjectEvents => ServiceProvider.GetService<ITeacherInSubjectEventService>();
        public ITeacherService Teachers => ServiceProvider.GetService<ITeacherService>();
    }
}
