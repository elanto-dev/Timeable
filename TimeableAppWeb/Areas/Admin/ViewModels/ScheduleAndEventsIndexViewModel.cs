using System;
using System.Collections.Generic;
using System.Globalization;
using BLL.DTO;

namespace TimeableAppWeb.Areas.Admin.ViewModels
{
    public class ScheduleAndEventsIndexViewModel
    {
        public int ScheduleId { get; set; }
        public List<SubjectForTimetableAndSettings> Subjects { get; set; } = default!;
        public List<EventForSettings> FutureEvents { get; set; } = default!;
        public string TodayDate { get; set; } = DateTime.Today.ToString("d");
        public int WeekNumber { get; set; } = 0;
        public bool UserHasRightsToEditSchedule { get; set; }
        public bool UserHasRightsToEditEvents { get; set; }
    }
}
