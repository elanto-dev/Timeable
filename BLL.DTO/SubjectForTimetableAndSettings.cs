using System;

namespace BLL.DTO
{
    public class SubjectForTimetableAndSettings
    {
        public int SubjectInScheduleId { get; set; }
        public int SubjectId { get; set; }
        public DateTime StartTime { get; set; }
        public string StartToEndString { get; set; } = default!;
        public string LectureType { get; set; } = default!;
        public string LectureNameWithCode { get; set; } = default!;
        public string Lecturers { get; set; } = default!;
        public string Rooms { get; set; } = default!;
    }
}
