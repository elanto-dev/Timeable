using System.Collections.Generic;
using BLL.DTO;

namespace TimeableAppWeb.ViewModels
{
    public class TimetableIndexViewModel
    {
        public List<EventForTimetable> Events { get; set; } = default!;
        public List<SubjectForTimetableAndSettings> LecturesForTimetable { get; set; } = default!;
        public List<PromotionsForTimetable> Promotions { get; set; } = default!;
        public Picture? BackgroundPicture { get; set; }
        public int WeekNumber { get; set; } = default!;
        public int ShowScheduleSeconds { get; set; }
    }
}
