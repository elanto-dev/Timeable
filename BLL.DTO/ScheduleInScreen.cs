using System;

namespace BLL.DTO
{
    public class ScheduleInScreen
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = default!;
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = default!;
    }
}
