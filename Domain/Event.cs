using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Base;

namespace Domain
{
    public class Event : DomainEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(50)]
        public string Place { get; set; } = default!;

        public DateTime StartTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public DateTime ShowStartDateTime { get; set; }

        public DateTime ShowEndDateTime { get; set; }

        public ICollection<EventInSchedule>? EventsInSchedules { get; set; }
    }
}
