using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Helpers;
using DAL.Base;

namespace DAL.App
{
    public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
    {

        public AppUnitOfWork(AppDbContext dbContext, IBaseRepositoryProvider repositoryProvider) : base(dbContext, repositoryProvider)
        {
        }
        public IAppUsersScreenRepository AppUsersScreens =>
            RepositoryProvider.GetRepository<IAppUsersScreenRepository>();
        public IEventInScheduleRepository EventInSchedules =>
            RepositoryProvider.GetRepository<IEventInScheduleRepository>();
        public IEventRepository Events =>
            RepositoryProvider.GetRepository<IEventRepository>();
        public IPictureInScreenRepository PictureInScreens =>
            RepositoryProvider.GetRepository<IPictureInScreenRepository>();
        public IPictureRepository Pictures =>
            RepositoryProvider.GetRepository<IPictureRepository>();
        public IScheduleInScreenRepository ScheduleInScreens =>
            RepositoryProvider.GetRepository<IScheduleInScreenRepository>();
        public IScheduleRepository Schedules =>
            RepositoryProvider.GetRepository<IScheduleRepository>();
        public IScreenRepository Screens =>
            RepositoryProvider.GetRepository<IScreenRepository>();
        public ISubjectInScheduleRepository SubjectInSchedules =>
            RepositoryProvider.GetRepository<ISubjectInScheduleRepository>();
        public ISubjectRepository Subjects =>
            RepositoryProvider.GetRepository<ISubjectRepository>();
        public ITeacherInSubjectEventRepository TeacherInSubjectEvents =>
            RepositoryProvider.GetRepository<ITeacherInSubjectEventRepository>();
        public ITeacherRepository Teachers =>
            RepositoryProvider.GetRepository<ITeacherRepository>();

    }
}
