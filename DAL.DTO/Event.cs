using System;

namespace DAL.DTO
{
    public class Event
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Name { get; set; } = default!;
        public string Place { get; set; } = default!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime ShowStartDateTime { get; set; }
        public DateTime ShowEndDateTime { get; set; }
    }
}
