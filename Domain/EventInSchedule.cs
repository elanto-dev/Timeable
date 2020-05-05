using DAL.Base;

namespace Domain
{
    public class EventInSchedule : DomainEntity
    {
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = default!;
        public int EventId { get; set; }
        public Event Event { get; set; } = default!;
    }
}
