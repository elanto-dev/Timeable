using System;

namespace BLL.DTO
{
    public class Schedule
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public DateTime Date { get; set; }
        public int WeekNumber { get; set; }
        public string Prefix { get; set; } = default!;
    }
}
