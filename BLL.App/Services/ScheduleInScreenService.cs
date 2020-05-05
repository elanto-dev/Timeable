using System;
using System.Threading.Tasks;
using BLL.App.Mappers;
using BLL.Base.Services;
using BLL.DTO;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;

namespace BLL.App.Services
{
    public class ScheduleInScreenService : BaseEntityService<DTO.ScheduleInScreen, DAL.DTO.ScheduleInScreen, IAppUnitOfWork>, IScheduleInScreenService
    {
        public ScheduleInScreenService(IAppUnitOfWork uow) : base(uow, new ScheduleInScreenMapper())
        {
            ServiceRepository = Uow.ScheduleInScreens;
        }

        public async Task<ScheduleInScreen> FindForScreenForDateWithoutIncludesAsync(int screenId, string prefix, DateTime date)
        {
            return ScheduleInScreenMapper.MapFromInternal(
                await Uow.ScheduleInScreens.FindForScreenForDateWithoutIncludesAsync(screenId, prefix, date));
        }
    }
}
