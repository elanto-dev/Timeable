using Contracts.DAL.App.Repositories;
using DAL.App.Repositories;
using DAL.Base.Helpers;

namespace DAL.App.Helpers
{
    public class AppRepositoryFactory : BaseRepositoryFactory<AppDbContext>
    {
        public AppRepositoryFactory()
        {
            RegisterRepositories();
        }

        private void RegisterRepositories()
        {
            AddToCreationMethods<IAppUsersScreenRepository>(dataContext => new AppUsersScreenRepository(dataContext));
            AddToCreationMethods<IEventInScheduleRepository>(dataContext => new EventInScheduleRepository(dataContext));
            AddToCreationMethods<IEventRepository>(dataContext => new EventRepository(dataContext));
            AddToCreationMethods<IPictureInScreenRepository>(dataContext => new PictureInScreenRepository(dataContext));
            AddToCreationMethods<IPictureRepository>(dataContext => new PictureRepository(dataContext));
            AddToCreationMethods<IScheduleInScreenRepository>(dataContext => new ScheduleInScreenRepository(dataContext));
            AddToCreationMethods<IScheduleRepository>(dataContext => new ScheduleRepository(dataContext));
            AddToCreationMethods<IScreenRepository>(dataContext => new ScreenRepository(dataContext));
            AddToCreationMethods<ISubjectInScheduleRepository>(dataContext => new SubjectInScheduleRepository(dataContext));
            AddToCreationMethods<ISubjectRepository>(dataContext => new SubjectRepository(dataContext));
            AddToCreationMethods<ITeacherInSubjectEventRepository>(dataContext => new TeacherInSubjectEventRepository(dataContext));
            AddToCreationMethods<ITeacherRepository>(dataContext => new TeacherRepository(dataContext));
        }
    }
}
