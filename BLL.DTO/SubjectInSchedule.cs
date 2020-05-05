using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
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

        [DisplayName("Unique key")]
        public string UniqueIdentifier { get; set; } = default!;

        [DisplayName("Subject type")]
        public int SubjectType { get; set; } = default!;

        [MaxLength(150)]
        public string Rooms { get; set; } = default!;

        [MaxLength(500)]
        public string Groups { get; set; } = default!;

        [DisplayName("Starts")]
        public DateTime StartDateTime { get; set; }

        [DisplayName("Ends")]
        public DateTime EndDateTime { get; set; }
    }
}
