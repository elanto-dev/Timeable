using BLL.App.Services;
using BLL.Base.Helpers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Helpers
{
    public class AppServiceFactory : ServiceFactoryBase<IAppUnitOfWork>
    {
        public AppServiceFactory()
        {
            RegisterServices();
        }

        private void RegisterServices()
        {
            // Register all your custom services here!
            AddToCreationMethods<IAppUsersScreenService>(uow => new AppUsersScreenService(uow));
            AddToCreationMethods<IEventInScheduleService>(uow => new EventInScheduleService(uow));
            AddToCreationMethods<IEventService>(uow => new EventService(uow));
            AddToCreationMethods<IPictureInScreenService>(uow => new PictureInScreenService(uow));
            AddToCreationMethods<IPictureService>(uow => new PictureService(uow));
            AddToCreationMethods<IScheduleInScreenService>(uow => new ScheduleInScreenService(uow));
            AddToCreationMethods<IScheduleService>(uow => new ScheduleService(uow));
            AddToCreationMethods<IScreenService>(uow => new ScreenService(uow));
            AddToCreationMethods<ISubjectInScheduleService>(uow => new SubjectInScheduleService(uow));
            AddToCreationMethods<ISubjectService>(uow => new SubjectService(uow));
            AddToCreationMethods<ITeacherInSubjectEventService>(uow => new TeacherInSubjectEventService(uow));
            AddToCreationMethods<ITeacherService>(uow => new TeacherService(uow));
        }

    }
}
