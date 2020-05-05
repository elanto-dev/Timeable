using DAL.Base;

namespace Domain
{
    public class ScheduleInScreen : DomainEntity
    {
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = default!;

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = default!;
    }
}
