using System;

namespace BLL.DTO
{
    public class AppUsersScreen
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
        public Guid AppUserId { get; set; }
        public int ScreenId { get; set; }
        public Screen Screen { get; set; } = default!;
    }
}
