using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.DTO
{
    public class SubjectForTimetableAndSettings
    {
        public int SubjectInScheduleId { get; set; }
        public int SubjectId { get; set; }
        public DateTime StartTime { get; set; }

        [Display(Name = nameof(StartToEndString), Prompt = nameof(StartToEndString), ResourceType = typeof(Resources.Domain.ScheduleAndEventView.ScheduleAndEvent))]
        public string StartToEndString { get; set; } = default!;

        [Display(Name = nameof(LectureType), Prompt = nameof(LectureType), ResourceType = typeof(Resources.Domain.ScheduleAndEventView.ScheduleAndEvent))]
        public string LectureType { get; set; } = default!;

        [Display(Name = nameof(LectureNameWithCode), Prompt = nameof(LectureNameWithCode), ResourceType = typeof(Resources.Domain.ScheduleAndEventView.ScheduleAndEvent))]
        public string LectureNameWithCode { get; set; } = default!;

        [Display(Name = nameof(Lecturers), Prompt = nameof(Lecturers), ResourceType = typeof(Resources.Domain.ScheduleAndEventView.ScheduleAndEvent))]
        public string Lecturers { get; set; } = default!;

        [Display(Name = nameof(Rooms), Prompt = nameof(Rooms), ResourceType = typeof(Resources.Domain.ScheduleAndEventView.ScheduleAndEvent))]
        public string Rooms { get; set; } = default!;
    }
}
