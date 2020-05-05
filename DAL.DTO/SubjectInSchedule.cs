using System;

namespace DAL.DTO
{
    public class SubjectInSchedule
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = default!;
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = default!;
        public string UniqueIdentifier { get; set; } = default!;
        public int SubjectType { get; set; } = default!;
        public string Rooms { get; set; } = default!;
        public string Groups { get; set; } = default!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
