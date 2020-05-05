using System;

namespace BLL.DTO
{
    public class EventInSchedule
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = default!;
        public int EventId { get; set; }
        public Event Event { get; set; } = default!;
    }
}
