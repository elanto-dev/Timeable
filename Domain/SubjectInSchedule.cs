using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class SubjectInSchedule : DomainEntity
    {
        public int SubjectId { get; set; } 
        public Subject Subject { get; set; } = default!;

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = default!;

        public string UniqueIdentifier { get; set; } = default!;

        public int SubjectType { get; set; } = default!;

        [MaxLength(150)]
        public string Rooms { get; set; } = default!;

        [MaxLength(500)]
        public string Groups { get; set; } = default!;

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public ICollection<TeacherInSubjectEvent>? TeacherInSubjectEvents { get; set; }
    }
}
