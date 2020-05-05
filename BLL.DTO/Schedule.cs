using System;
using System.ComponentModel;

namespace BLL.DTO
{
    public class Schedule
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [DisplayName("Week number")]
        public int WeekNumber { get; set; }
        public string Prefix { get; set; } = default!;
    }
}
