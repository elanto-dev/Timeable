using System;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class ScheduleService : BaseEntityService<DTO.Schedule, DAL.DTO.Schedule, IAppUnitOfWork>, IScheduleService
    {
        public ScheduleService(IAppUnitOfWork uow) : base(uow, new ScheduleMapper())
        {
            ServiceRepository = Uow.Schedules;
        }

        public Task<bool> ScheduleForDayExistsAsync(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
