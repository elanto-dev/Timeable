using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Schedule : DomainEntity
    {
        public DateTime Date { get; set; }

        public int WeekNumber { get; set; }

        [MaxLength(20)]
        public string Prefix { get; set; } = default!;

        public ICollection<SubjectInSchedule>? SubjectsInSchedules { get; set; }

        public ICollection<ScheduleInScreen>? ScheduleInScreens { get; set; }

        public ICollection<EventInSchedule>? EventInSchedules { get; set; }
    }
}
