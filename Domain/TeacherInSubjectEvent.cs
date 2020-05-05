using DAL.Base;

namespace Domain
{
    public class TeacherInSubjectEvent : DomainEntity
    {
        public int SubjectInScheduleId { get; set; }
        public SubjectInSchedule SubjectInSchedule { get; set; } = default!;

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = default!;
    }
}
