using System;

namespace DAL.DTO
{
    public class TeacherInSubjectEvent
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public int SubjectInScheduleId { get; set; }
        public SubjectInSchedule SubjectInSchedule { get; set; } = default!;
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = default!;
    }
}
