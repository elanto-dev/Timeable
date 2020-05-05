using System;

namespace DAL.DTO
{
    public class Screen
    {
        public int Id { get; set; }
        public string UniqueIdentifier { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public string Prefix { get; set; } = default!;
        public bool IsActive { get; set; }
        public int? ShowScheduleSeconds { get; set; }
    }
}
